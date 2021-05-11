using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Jumper
{
    public partial class MyForm : Form
    {
        readonly int SizeValue;
        readonly Size BlockSize;
        string currentPath;
        Player player;
        Map map;
        Image playerSkin, border, block, spike, saw, jumpOrb, gravityOrb;

        public MyForm()
        {
            InitializeComponent();

            gameTime.Interval = 10;
            gameTime.Tick += new EventHandler(Update);

            KeyDown += new KeyEventHandler(OnPress);
            KeyUp += new KeyEventHandler(OnKeyUp);

            SizeValue = Screen.FromControl(this).WorkingArea.Height / 20 + 1;
            //SizeValue = 53;
            BlockSize = new Size(new Point(SizeValue, SizeValue));

            Initialise();
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    player.IsLeftMoving = false;
                    break;
                case Keys.D:
                    player.IsRightMoving = false;
                    break;
            }
        }

        public void OnPress(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    player.IsLeftMoving = true;
                    break;
                case Keys.D:
                    player.IsRightMoving = true;
                    break;
                case Keys.Space:
                    player.IsJumping = true;
                    break;
                case Keys.Up:
                    player.TriggerStart = DateTime.Now;
                    break;
            }
        }

        void Initialise()
        {
            currentPath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName.ToString();
            playerSkin = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Cube.png")));
            border = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Border.png")));
            block = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Block.png")));
            spike = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Spike.png")));
            saw = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Saw.png")));
            jumpOrb = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Yellow_Orb.png")));
            gravityOrb = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Blue_Orb.png")));
            map = new Map();
            player = new Player(map.startPos, SizeValue);

            gameTime.Start();
        }

        public void Update(object sender, EventArgs e)
        {
            if (player.IsDead)
                Restart();
            if (player.TriggerStart != null)
                player.ReactToOrbs(map, SizeValue);
            if (player.IsMoving)
                player.Move(map, SizeValue);

            Text = $"Position: X = {player.X}, Y = {player.Y}, " +
                $"Map pos: ({Math.Round((double)player.X / SizeValue)}; {Math.Round((double)player.Y / SizeValue)})" +
                $" Down: ({Math.Floor((double)player.X / SizeValue)}, {Math.Ceiling((double)player.X / SizeValue)}; {Math.Floor((double)player.Y / SizeValue) - 1})" +
                $" Up: ({Math.Floor((double)player.X / SizeValue)}, {Math.Ceiling((double)player.X / SizeValue)}; {Math.Ceiling((double)player.Y / SizeValue) + 1})" +
                $" Left: ({Math.Ceiling((double)player.X / SizeValue) - 1}; {Math.Floor((double)player.Y / SizeValue)}, {Math.Ceiling((double)player.Y / SizeValue)})" +
                $" Right: ({Math.Floor((double)player.X / SizeValue) + 1}; {Math.Floor((double)player.Y / SizeValue)}, {Math.Ceiling((double)player.Y / SizeValue)})";

            Invalidate();
        }

        private void Restart()
        {
            player = new Player(map.startPos, SizeValue);
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawMap(g);
        }

        private void DrawMap(Graphics g)
        {
            for (var i = 0; i < map.Width; i++)
                for (var j = 0; j < map.Height; j++)
                    switch (map.Level[i, j].Type)
                    {
                        case "Border":
                            g.DrawImage(border, ConvertMathToWorld(i * SizeValue, j * SizeValue));
                            break;
                        case "Block":
                            g.DrawImage(block, ConvertMathToWorld(i * SizeValue, j * SizeValue));
                            break;
                        case "Spike":
                            g.DrawImage(spike, ConvertMathToWorld(i * SizeValue, j * SizeValue));
                            break;
                        case "Saw":
                            g.DrawImage(saw, ConvertMathToWorld(i * SizeValue, j * SizeValue));
                            break;
                        case "JumpOrb":
                            g.DrawImage(jumpOrb, ConvertMathToWorld(i * SizeValue, j * SizeValue));
                            break;
                        case "GravityOrb":
                            g.DrawImage(gravityOrb, ConvertMathToWorld(i * SizeValue, j * SizeValue));
                            break;
                    }
            g.DrawImage(playerSkin, ConvertMathToWorld(player.X, player.Y));
        }

        private Image FitInSize(Image image) =>
            new Bitmap(image, BlockSize);

        private Point ConvertMathToWorld(int x, int y) =>
            new Point(x, (map.Height - 1) * SizeValue - y);
    }
}
