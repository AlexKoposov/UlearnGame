using System;
using System.Collections.Generic;
using System.Drawing;

namespace Project_Jumper
{
    public class Player
    {
        public bool MovingRight { get; set; }
        public bool MovingLeft { get; set; }
        public bool Jumping { get; set; }
        public bool IsJumpOrbActive { get; set; }
        public bool Falling { get; set; }
        public bool Flying { get; set; }
        public bool Dead { get; set; }
        public bool IsLevelCompleted { get; set; }
        public bool IsMessageShowed { get; set; }
        public int FallTicks { get; set; }
        public int FlyTicks { get; set; }
        public int TriggerTicks { get; set; }
        public Gamemodes GameMode { get; set; }
        public int Gravity { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int VelX { get; private set; }
        public int VelY { get; private set; }
        public bool Moving => MovingRight
            || MovingLeft
            || Falling
            || Jumping
            || Flying;
        private int Velocity { get; set; }
        private int MaxFallingVel { get; set; }
        private int MaxFlyingVel { get; set; }

        public Player(int x, int y, int size, Gamemodes gameMode = Gamemodes.Cube, int gravity = 1)
        {
            X = x;
            Y = y;
            ApplyDefaultConditions(size, gravity, gameMode);
        }

        public Player(Point mapStartPos, int size, Gamemodes gameMode = Gamemodes.Cube, int gravity = 1)
        {
            X = mapStartPos.X * size;
            Y = mapStartPos.Y * size;
            ApplyDefaultConditions(size, gravity, gameMode);
        }

        public void Move(Map map, int size)
        {
            ProcessCollisionX(map, size);
            ProcessCollisionY(map, size);
            ReactToPortals(map, size);
        }

        public void Stop()
        {
            MovingLeft = false;
            MovingRight = false;
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
                                JumpOrbAction();
                                break;
                            case "GravityOrb":
                                GravityOrbAction();
                                break;
                        }
            }
        }

        private void ApplyDefaultConditions(int size, int gravity, Gamemodes gameMode)
        {
            Gravity = gravity;
            Velocity = size / 8;
            MaxFallingVel = size / 4;
            MaxFlyingVel = Velocity;
            GameMode = gameMode;
        }

        private void JumpOrbAction()
        {
            FallTicks = 0;
            TriggerTicks = 999;
            IsJumpOrbActive = true;
            Jump();
        }

        private void GravityOrbAction()
        {
            ChangeGravity();
            TriggerTicks = 999;
            Falling = true;
        }

        private void Jump()
        {
            if (!Falling || IsJumpOrbActive)
                VelY = Gravity * (int)(Velocity * 1.5);
            Falling = true;
            IsJumpOrbActive = false;
        }

        private void Fall()
        {
            var k = ++FallTicks * 0.01;
            VelY += Gravity * (int)(-Velocity * k);
            if (VelY == 0) VelY = -Gravity * Velocity / 3;
        }

        private void Fly()
        {
            if (!Flying) FlyTicks = 0;
            else
            {
                Falling = false;
                var k = ++FlyTicks * 0.04;
                VelY = Gravity * (int)(Velocity * k);
                if (VelY > MaxFlyingVel || VelY < MaxFlyingVel)
                    VelY = Gravity * MaxFlyingVel;
            }
        }

        private void ChangeGravity() =>
            Gravity = Gravity == 1 ? -1 : 1;

        private void ProcessCollisionX(Map map, int size)
        {
            if (!MovingRight && !MovingLeft)
                VelX = 0;
            if (MovingRight && VelX < Velocity)
                VelX += Velocity;
            if (MovingLeft && VelX > -Velocity)
                VelX -= Velocity;

            var dirX = X + VelX;

            if (dirX < 0)
            {
                X = 0;
                VelX = 0;
            }
            else
            {
                var leftDown = new Point((int)Math.Floor((double)dirX / size), (int)Math.Floor((double)Y / size));
                var leftUp = new Point((int)Math.Floor((double)dirX / size), (int)Math.Ceiling((double)Y / size));
                if (CheckCollision(map, leftDown, leftUp))
                {
                    X = (leftDown.X + 1) * size;
                    VelX = 0;
                }
                CheckFriendlyness(map, leftDown, leftUp);
                CheckLevelCompletion(map, leftDown, leftUp);
            }

            if (dirX > (map.Width - 1) * size)
            {
                X = (map.Width - 1) * size;
                VelX = 0;
            }
            else
            {
                var rightDown = new Point((int)Math.Ceiling((double)dirX / size), (int)Math.Floor((double)Y / size));
                var rightUp = new Point((int)Math.Ceiling((double)dirX / size), (int)Math.Ceiling((double)Y / size));
                if (CheckCollision(map, rightDown, rightUp))
                {
                    X = (rightDown.X - 1) * size;
                    VelX = 0;
                }
                CheckFriendlyness(map, rightDown, rightUp);
                CheckLevelCompletion(map, rightDown, rightUp);
            }

            X += VelX;
        }

        private void ProcessCollisionY(Map map, int size)
        {
            if (Jumping)
            {
                switch (GameMode)
                {
                    case Gamemodes.Cube:
                        Jump();
                        break;
                    case Gamemodes.Ball:
                        if (!Falling)
                            GravityOrbAction();
                        break;
                }
            }
            if (Flying) Fly();
            if (Falling) Fall();
            if (VelY > MaxFallingVel)
                VelY = MaxFallingVel;
            else if (VelY < -MaxFallingVel)
                VelY = -MaxFallingVel;
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
                var downLeft = new Point((int)Math.Floor((double)X / size), (int)Math.Floor((double)dirY / size));
                var downRight = new Point((int)Math.Ceiling((double)X / size), (int)Math.Floor((double)dirY / size));
                CollideY(map, size, downLeft, downRight, true, ref isUpStuck);
            }

            if (dirY > (map.Height - 1) * size)
            {
                Y = (map.Height - 1) * size;
                if (Gravity == 1) isUpStuck = ApplyUpStuck();
                else ApplyLanding();
            }
            else
            {
                var upLeft = new Point((int)Math.Floor((double)X / size), (int)Math.Ceiling((double)dirY / size));
                var upRight = new Point((int)Math.Ceiling((double)X / size), (int)Math.Ceiling((double)dirY / size));
                CollideY(map, size, upLeft, upRight, false, ref isUpStuck);
            }

            if (!Jumping && !Falling) VelY = 0;
            Y += VelY;
            if (isUpStuck) Falling = true;
        }

        private void CollideY(Map map, int size, Point first, Point second, bool defaultIsDown, ref bool isUpStuck)
        {
            if (VelY == 0
                && (defaultIsDown ? Gravity == 1 : Gravity == -1))
            {
                first.Y -= Gravity;
                second.Y -= Gravity;
            }
            if ((defaultIsDown ? first.Y >= 0 : first.Y <= map.Height - 1)
                && CheckCollision(map, first, second))
            {
                Y = (first.Y + (defaultIsDown ? 1 : -1)) * size;
                if (defaultIsDown)
                {
                    if (Gravity == 1) ApplyLanding();
                    else isUpStuck = ApplyUpStuck();
                }
                else
                {
                    if (Gravity == 1) isUpStuck = ApplyUpStuck();
                    else ApplyLanding();
                }
                CheckFriendlyness(map, first, second);
                CheckLevelCompletion(map, first, second);
            }
            else if (defaultIsDown)
            {
                Falling = true;
            }
        }

        private bool ApplyUpStuck()
        {
            StopJumping();
            return true;
        }

        private void ApplyLanding()
        {
            StopJumping();
        }

        private void ReactToPortals(Map map, int size)
        {
            var currentPos = new Point((int)Math.Round((double)X / size), (int)Math.Round((double)Y / size));
            if (map.Level[currentPos.X, currentPos.Y].IsPortal)
                switch (map.Level[currentPos.X, currentPos.Y].Type)
                {
                    case "CubePortal":
                        GameMode = Gamemodes.Cube;
                        break;
                    case "BallPortal":
                        GameMode = Gamemodes.Ball;
                        break;
                    case "JetPortal":
                        GameMode = Gamemodes.Jetpack;
                        break;
                }
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
            if (!Dead)
            {
                Dead = map.Level[p1.X, p1.Y].Collision && map.Level[p2.X, p2.Y].Collision
                    ? !map.Level[p1.X, p1.Y].IsFriendly && !map.Level[p2.X, p2.Y].IsFriendly
                    : !map.Level[p1.X, p1.Y].IsFriendly || !map.Level[p2.X, p2.Y].IsFriendly;
            }
        }

        private void StopJumping()
        {
            Falling = false;
            Jumping = false;
            FallTicks = 0;
        }
    }
}
