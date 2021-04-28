using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Project_Jumper
{
    public class Player
    {
        readonly int Velocity = 10;
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
        public DateTime? FallStart { get; set; }


        public Player(int x, int y, int gravity = 1)
        {
            X = x;
            Y = y;
            Gravity = gravity;
            IsFalling = true;
        }

        public void Move(Map map, int size)
        {
            ProcessCollisionX(map, size);
            ProcessCollisionY(map, size);
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
            VelY += Gravity * (int)(-10 * t);
        }

        public void SwitchGravity() =>
            Gravity = Gravity == 1 ? -1 : 1;

        private void ProcessCollisionX(Map map, int size)
        {
            var plPos = new Point((int)Math.Round((double)X / size), (int)Math.Round((double)Y / size) - 1);

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
                IsLeftMoving = false;
            }
            else
            {
                var left1 = new Point((int)Math.Floor((double)dirX / size) - 1, (int)Math.Floor((double)Y / size));
                var left2 = new Point((int)Math.Floor((double)dirX / size) - 1, (int)Math.Ceiling((double)Y / size));
                if (left1.X >= 0)
                    if (map.Level[left1.X, left1.Y].Collision || map.Level[left2.X, left2.Y].Collision)
                     {
                        X = (left1.X + 1) * size;
                        IsLeftMoving = false;
                    }



            }




            if (dirX > (map.Width - 1) * size)
            {
                X = (map.Width - 1) * size;
                IsRightMoving = false;
            }
            var mapCellX = dirX / size;



            X += VelX;
        }

        private void ProcessCollisionY(Map map, int size)
        {
            if (IsJumping)
                Jump();
            if (IsFalling)
                Fall();
            var dirY = Y + VelY;
            var plPos = new Point((int)Math.Round((double)X / size), (int)Math.Round((double)Y / size) - 1);
            var plPosY = new Point((int)Math.Round((double)X / size), (int)Math.Round((double)dirY / size) - 1);

            if (dirY < size)
            {
                Y = size;
                IsFalling = false;
                IsJumping = false;
                Jumped = false;
                FallStart = null;
            }
            else
            {
                var down1 = new Point((int)Math.Floor((double)X / size), (int)Math.Floor((double)dirY / size) - 1);
                var down2 = new Point((int)Math.Ceiling((double)X / size), (int)Math.Floor((double)dirY / size) - 1);
                if (down1.Y >= 0)
                    if (map.Level[down1.X, down1.Y].Collision || map.Level[down2.X, down2.Y].Collision)
                    {
                        Y = (down1.Y + 2) * size;
                        IsFalling = false;
                        IsJumping = false;
                        Jumped = false;
                        FallStart = null;
                    }
                    else
                    {
                        IsFalling = true;
                    }
            }

            var upStuck = false;
            if (dirY > map.Height * size)
            {
                Y = map.Height * size;
                IsFalling = false;
                IsJumping = false;
                Jumped = false;
                FallStart = null;
                upStuck = true;
            }
            else
            {
                var up1 = new Point((int)Math.Floor((double)X / size), (int)Math.Ceiling((double)dirY / size) - 1);
                var up2 = new Point((int)Math.Ceiling((double)X / size), (int)Math.Ceiling((double)dirY / size) - 1);
                if (up1.Y < map.Height)
                    if (map.Level[up1.X, up1.Y].Collision || map.Level[up2.X, up2.Y].Collision)
                    {
                        Y = (up1.Y - 0) * size;
                        IsFalling = false;
                        IsJumping = false;
                        Jumped = false;
                        FallStart = null;
                        upStuck = true;
                    }

            }


            if (!IsJumping && !IsFalling)
                VelY = 0;
            Y += VelY;
            if (upStuck) IsFalling = true;
        }
    }
}
