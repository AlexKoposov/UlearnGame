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
            VelY = Gravity * (int)(Velocity * 1.5);
        }

        public void Fall()
        {
            if (FallStart == null)
                FallStart = DateTime.Now;
            var t = ((TimeSpan)(DateTime.Now - FallStart)).TotalSeconds;
            VelY += Gravity * (int)(-30 * t);
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
            if (dirX < size)
                X = size;
            else if (dirX > (map.Width - 2) * size)
                X = (map.Width - 2) * size;
            else X += VelX;
            var mapCellX = dirX / size;


        }

        private void ProcessCollisionY(Map map, int size)
        {
            if (IsJumping)
                Jump();
            if (IsFalling)
                Fall();
            var dirY = Y + VelY;
            var playerRect = new Rectangle(X, Y, size, size);
            var plTest = new Point((int)Math.Floor((double)X / size), (int)Math.Floor((double)Y / size));

            if (dirY < size)
            {
                var down1 = new Point((int)Math.Floor((double)X / size), (int)Math.Floor((double)dirY / size));

                Y = size;
                IsFalling = false;
                IsJumping = false;
                FallStart = null;
            }
            else
            {
                //var downCell1 = new Rectangle((int)Math.Floor((double)X / size) * size, (int)Math.Floor((double)dirY / size) * size, size, size);
                //var downCell2 = new Rectangle((int)Math.Ceiling((double)X / size) * size, (int)Math.Floor((double)dirY / size) * size, size, size);
                var down1 = new Point((int)Math.Floor((double)X / size), (int)Math.Floor((double)dirY / size));
                var down2 = new Point((int)Math.Ceiling((double)X / size), (int)Math.Floor((double)dirY / size));
                //if (map.Level[downCell1.X / size, downCell1.Y / size].Collision && playerRect.IntersectsWith(downCell1) || map.Level[downCell2.X / size, downCell2.Y / size].Collision && playerRect.IntersectsWith(downCell2))
                if (map.Level[down1.X, down1.Y].Collision || map.Level[down2.X, down2.Y].Collision)
                {
                    Y = (down1.Y + 2) * size;
                    IsFalling = false;
                    IsJumping = false;
                    FallStart = null;
                }
            }

            if (dirY > map.Height * size)
            {
                Y = map.Height;
                IsJumping = false;
            }
            else
            {
                var upCell1 = new Rectangle((int)Math.Floor((double)X / size) * size, (int)Math.Ceiling((double)dirY / size) * size, size, size);
                var upCell2 = new Rectangle((int)Math.Ceiling((double)X / size) * size, (int)Math.Ceiling((double)dirY / size) * size, size, size);
            }
            

            //var mapCellX = (int)Math.Round((double)X / size);
            //var mapCellY = (int)Math.Round((double)dirY / size);
            //if (dirY < 0)
            //{
            //    Y = 0;
            //    IsJumping = false;
            //}
            //else if (dirY > (map.Height - 1) * size)
            //{
            //    Y = (map.Height - 1) * size;
            //    IsJumping = false;
            //    IsFalling = false;
            //    FallStart = null;
            //}
            //else if (map.Level[mapCellX, mapCellY].Collision)
            //{
            //    Y = (mapCellY - 1) * size;
            //    IsJumping = false;
            //    IsFalling = false;
            //    FallStart = null;
            //}
            //else
            //{
            //    IsFalling = true;
            //    Y -= VelY;
            //}
            if (!IsJumping && !IsFalling)
                VelY = 0;
            Y += VelY;
        }
    }
}
