using System;
using System.Collections.Generic;
using System.Drawing;

namespace Project_Jumper
{
    public class Player
    {
        private int Velocity { get; set; }
        private int MaxYVel { get; set; }
        private int Gravity { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int VelX { get; private set; }
        public int VelY { get; private set; }
        public bool IsMoving => IsRightMoving
            || IsLeftMoving
            || IsFalling
            || IsJumping;
        public bool IsRightMoving { get; set; }
        public bool IsLeftMoving { get; set; }
        public bool IsJumping { get; set; }
        public bool IsFalling { get; set; }
        public bool Jumped { get; set; }
        public bool IsDead { get; set; }
        public bool IsLevelCompleted { get; set; }
        public bool IsMessageShowed { get; set; }
        public int FallTicks { get; set; }
        public int TriggerTicks { get; set; }


        public Player(int x, int y, int size, int gravity = 1)
        {
            X = x;
            Y = y;
            ApplyDefaultConditions(size, gravity);
        }

        public Player(Point startPos, int size, int gravity = 1)
        {
            X = startPos.X * size;
            Y = startPos.Y * size;
            ApplyDefaultConditions(size, gravity);
        }

        private void ApplyDefaultConditions(int size, int gravity)
        {
            Gravity = gravity;
            Velocity = size / 6;
            MaxYVel = size / 4;
            IsFalling = true;
        }

        public void Move(Map map, int size)
        {
            ProcessCollisionX(map, size);
            ProcessCollisionY(map, size);
        }

        public void Stop()
        {
            IsLeftMoving = false;
            IsRightMoving = false;
            StopJumping();
        }

        public void ReactToOrbs(Map map, int size)
        {
            if (TriggerTicks != 0 && TriggerTicks <= 15)
            {
                var positions = new HashSet<MapCell>
                {
                    map.Level[(int)Math.Floor((double)X / size), (int)Math.Floor((double)Y / size)],
                    map.Level[(int)Math.Floor((double)X / size), (int)Math.Ceiling((double)Y / size)],
                    map.Level[(int)Math.Ceiling((double)X / size), (int)Math.Floor((double)Y / size)],
                    map.Level[(int)Math.Ceiling((double)X / size), (int)Math.Ceiling((double)Y / size)]
                };
                foreach (var currentPos in positions)
                    if (currentPos.IsOrb)
                        switch (currentPos.Type)
                        {
                            case "JumpOrb":
                                Jumped = false;
                                FallTicks = 0;
                                Jump();
                                break;
                            case "GravityOrb":
                                ChangeGravity();
                                break;
                        }
            }
        }

        public void MoveRight()
        {
            if (VelX < Velocity)
                VelX += Velocity;
        }

        public void MoveLeft()
        {
            if (VelX > -Velocity)
                VelX -= Velocity;
        }

        public void Jump()
        {
            IsFalling = true;
            if (!Jumped)
                VelY = Gravity * (int)(Velocity * 1.25);
            Jumped = true;
        }

        public void Fall()
        {
            var k = FallTicks++ * 0.01;
            VelY += Gravity * (int)(-Velocity * k);
        }

        public void ChangeGravity() =>
            Gravity = Gravity == 1 ? -1 : 1;

        private void ProcessCollisionX(Map map, int size)
        {
            if (!IsRightMoving && !IsLeftMoving)
                VelX = 0;
            if (IsRightMoving && VelX < Velocity)
                VelX += Velocity;
            if (IsLeftMoving && VelX > -Velocity)
                VelX -= Velocity;

            var dirX = X + VelX;

            if (dirX < 0)
            {
                X = 0;
                VelX = 0;
            }
            else
            {
                var left1 = new Point((int)Math.Floor((double)dirX / size), (int)Math.Floor((double)Y / size));
                var left2 = new Point((int)Math.Floor((double)dirX / size), (int)Math.Ceiling((double)Y / size));
                if (CheckCollision(map, left1, left2))
                {
                    X = (left1.X + 1) * size;
                    VelX = 0;
                }
                CheckFriendlyness(map, left1, left2);
                CheckLevelCompletion(map, left1, left2);
            }

            if (dirX > (map.Width - 1) * size)
            {
                X = (map.Width - 1) * size;
                VelX = 0;
            }
            else
            {
                var right1 = new Point((int)Math.Ceiling((double)dirX / size), (int)Math.Floor((double)Y / size));
                var right2 = new Point((int)Math.Ceiling((double)dirX / size), (int)Math.Ceiling((double)Y / size));
                if (CheckCollision(map, right1, right2))
                {
                    X = (right1.X - 1) * size;
                    VelX = 0;
                }
                CheckFriendlyness(map, right1, right2);
                CheckLevelCompletion(map, right1, right2);
            }

            X += VelX;
        }

        private void ProcessCollisionY(Map map, int size)
        {
            if (IsJumping) Jump();
            if (IsFalling) Fall();
            if (VelY > MaxYVel)
                VelY = MaxYVel;
            else if (VelY < -MaxYVel)
                VelY = -MaxYVel;
            var dirY = Y + VelY;
            var isUpStuck = false;

            if (dirY < 0)
            {
                Y = 0;
                if (Gravity == 1) ApplyLanding();
                else isUpStuck = ApplyUpStuck();
            }
            else
            {
                var down1 = new Point((int)Math.Floor((double)X / size), (int)Math.Floor((double)dirY / size));
                var down2 = new Point((int)Math.Ceiling((double)X / size), (int)Math.Floor((double)dirY / size));
                if (down1.Y >= 0)
                    if (map.Level[down1.X, down1.Y].Collision || map.Level[down2.X, down2.Y].Collision)
                    {
                        Y = (down1.Y + 1) * size;
                        if (Gravity == 1) ApplyLanding();
                        else isUpStuck = ApplyUpStuck();
                        CheckFriendlyness(map, down1, down2);
                        CheckLevelCompletion(map, down1, down2);
                    }
                    else
                    {
                        IsFalling = true;
                    }
            }

            if (dirY > (map.Height - 1) * size)
            {
                Y = (map.Height - 1) * size;
                if (Gravity == 1) isUpStuck = ApplyUpStuck();
                else ApplyLanding();
            }
            else
            {
                var up1 = new Point((int)Math.Floor((double)X / size), (int)Math.Ceiling((double)dirY / size));
                var up2 = new Point((int)Math.Ceiling((double)X / size), (int)Math.Ceiling((double)dirY / size));
                if (up1.Y <= map.Height - 1)
                    if (map.Level[up1.X, up1.Y].Collision
                        || map.Level[up2.X, up2.Y].Collision)
                    {
                        Y = (up1.Y - 1) * size;
                        if (Gravity == 1) isUpStuck = ApplyUpStuck();
                        else ApplyLanding();
                        CheckFriendlyness(map, up1, up2);
                        CheckLevelCompletion(map, up1, up2);
                    }
            }

            if (!IsJumping && !IsFalling) VelY = 0;
            Y += VelY;
            if (isUpStuck) IsFalling = true;
        }

        private bool ApplyUpStuck()
        {
            bool isUpStuck;
            StopJumping();
            isUpStuck = true;
            return isUpStuck;
        }

        private void ApplyLanding()
        {
            StopJumping();
            Jumped = false;
        }

        private static bool CheckCollision(Map map, Point p1, Point p2)
        {
            return map.Level[p1.X, p1.Y].Collision
                || map.Level[p2.X, p2.Y].Collision;
        }

        private void CheckLevelCompletion(Map map, Point p1, Point p2)
        {
            if (!IsLevelCompleted)
                IsLevelCompleted = map.Level[p1.X, p1.Y].Type == "Finish"
                                || map.Level[p2.X, p2.Y].Type == "Finish";
        }

        private void CheckFriendlyness(Map map, Point p1, Point p2)
        {
            if (!IsDead)
                IsDead = !map.Level[p1.X, p1.Y].IsFriendly
                      || !map.Level[p2.X, p2.Y].IsFriendly;
        }

        private void StopJumping()
        {
            IsFalling = false;
            IsJumping = false;
            FallTicks = 0;
        }
    }
}
