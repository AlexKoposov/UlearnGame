using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Project_Jumper
{
    public class Player
    {
        readonly int Velocity = 10;
        public int X { get; private set; }
        public int Y { get; private set; }
        private int VelX { get; set; }
        private int VelY { get; set; }
        public bool IsMoving { get; set; }
        public bool IsRightMoving { get; set; }
        public bool IsLeftMoving { get; set; }
        public bool IsJumping { get; set; }
        public DateTime? JumpStart { get; set; }

        public Player(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Move()
        {
            if (!IsJumping)
                VelY = 0;
            else Jump();
            if (!IsRightMoving && !IsLeftMoving)
                VelX = 0;
            if (IsRightMoving && VelX < Velocity)
                VelX += Velocity;
            if (IsLeftMoving && VelX > -Velocity)
                VelX -= Velocity;
            X += VelX;
            Y -= VelY;
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
            if (JumpStart == null)
                JumpStart = DateTime.Now;
            var t = ((TimeSpan)(DateTime.Now - JumpStart)).TotalSeconds;
            VelY = (int)(Velocity * 1.5 - 30 * t);
        }
    }
}
