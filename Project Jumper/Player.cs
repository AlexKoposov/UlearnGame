using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Project_Jumper
{
    public class Player
    {
        private int Velocity { get; set; }
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
        public DateTime? FallStart { get; set; }
        public DateTime? TriggerStart { get; set; }

        public Player(int x, int y, int size, int gravity = 1)
        {
            X = x;
            Y = y;
            Gravity = gravity;
            Velocity = size / 5;
            IsFalling = true;
        }

        public Player(Point startPos, int size, int gravity = 1)
        {
            X = startPos.X * size;
            Y = startPos.Y * size;
            Gravity = gravity;
            Velocity = size / 5;
            IsFalling = true;
        }

        public void Move(Map map, int size)
        {
            ProcessCollisionX(map, size);
            ProcessCollisionY(map, size);
        }

        public void ReactToOrbs(Map map, int size)
        {
            if (TriggerStart != null)
            {
                if ((DateTime.Now - TriggerStart).Value.TotalMilliseconds <= 100)
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
                                    FallStart = null;
                                    TriggerStart = null;
                                    Jump();
                                    break;
                            }
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
                VelY = Gravity * (int)(Velocity * 1.5);
            Jumped = true;
        }

        public void Fall()
        {
            if (FallStart == null)
                FallStart = DateTime.Now;
            var t = ((TimeSpan)(DateTime.Now - FallStart)).TotalSeconds;
            VelY += Gravity * (int)(-Velocity * t);
        }

        public void SwitchGravity() =>
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
                if (map.Level[left1.X, left1.Y].Collision
                    || map.Level[left2.X, left2.Y].Collision)
                {
                    X = (left1.X + 1) * size;
                    VelX = 0;
                }
                if (!map.Level[left1.X, left1.Y].IsFriendly
                    || !map.Level[left2.X, left2.Y].IsFriendly)
                    IsDead = true;
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
                if (map.Level[right1.X, right1.Y].Collision
                    || map.Level[right2.X, right2.Y].Collision)
                {
                    X = (right1.X - 1) * size;
                    VelX = 0;
                }
                if (!map.Level[right1.X, right1.Y].IsFriendly
                    || !map.Level[right2.X, right2.Y].IsFriendly)
                    IsDead = true;
            }

            X += VelX;
        }

        private void ProcessCollisionY(Map map, int size)
        {
            if (IsJumping)
                Jump();
            if (IsFalling)
                Fall();

            var dirY = Y + VelY;

            if (dirY < 0)
            {
                Y = 0;
                IsFalling = false;
                IsJumping = false;
                Jumped = false;
                FallStart = null;
            }
            else
            {
                var down1 = new Point((int)Math.Floor((double)X / size), (int)Math.Floor((double)dirY / size));
                var down2 = new Point((int)Math.Ceiling((double)X / size), (int)Math.Floor((double)dirY / size));
                if (down1.Y >= 0)
                    if (map.Level[down1.X, down1.Y].Collision || map.Level[down2.X, down2.Y].Collision)
                    {
                        Y = (down1.Y + 1) * size;
                        IsFalling = false;
                        IsJumping = false;
                        Jumped = false;
                        FallStart = null;
                        if (!map.Level[down1.X, down1.Y].IsFriendly
                            || !map.Level[down2.X, down2.Y].IsFriendly)
                            IsDead = true;
                    }
                    else
                    {
                        IsFalling = true;
                    }
            }

            var isUpStuck = false;
            if (dirY > (map.Height - 1) * size)
            {
                Y = (map.Height - 1) * size;
                IsFalling = false;
                IsJumping = false;
                FallStart = null;
                isUpStuck = true;
            }
            else
            {
                var up1 = new Point((int)Math.Floor((double)X / size), (int)Math.Ceiling((double)dirY / size));
                var up2 = new Point((int)Math.Ceiling((double)X / size), (int)Math.Ceiling((double)dirY / size));
                if (up1.Y < map.Height - 1)
                    if (map.Level[up1.X, up1.Y].Collision
                        || map.Level[up2.X, up2.Y].Collision)
                    {
                        Y = (up1.Y - 1) * size;
                        IsFalling = false;
                        IsJumping = false;
                        FallStart = null;
                        isUpStuck = true;
                        if (!map.Level[up1.X, up1.Y].IsFriendly
                            || !map.Level[up2.X, up2.Y].IsFriendly)
                            IsDead = true;
                    }
            }

            if (!IsJumping && !IsFalling)
                VelY = 0;
            Y += VelY;
            if (isUpStuck) IsFalling = true;
        }
    }
}
