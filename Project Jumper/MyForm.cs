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
        Image playerSkin, Border, Block, Spike;

        public MyForm()
        {
            InitializeComponent();

            gameTime.Interval = 10;
            gameTime.Tick += new EventHandler(Update);

            KeyDown += new KeyEventHandler(OnPress);
            KeyUp += new KeyEventHandler(OnKeyUp);

            SizeValue = Screen.FromControl(this).WorkingArea.Height / 20;
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
            }
        }

        void Initialise()
        {
            currentPath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName.ToString();
            playerSkin = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Cube.png")));
            Border = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Border.png")));
            Block = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Block.png")));
            Spike = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Spike.png")));
            map = new Map();
            player = new Player(map.Width / 12 * SizeValue, map.Height / 12 * SizeValue, SizeValue);
            gameTime.Start();
        }

        public void Update(object sender, EventArgs e)
        {
            if (player.IsMoving)
                player.Move(map, SizeValue);
            Text = $"Position: X = {player.X}, Y = {player.Y}, " +
                $"Map pos: ({Math.Round((double)player.X / SizeValue)}, {Math.Round((double)player.Y / SizeValue)})" +
                $" Down: ({Math.Floor((double)player.X / SizeValue)}, {Math.Ceiling((double)player.X / SizeValue)}; {Math.Floor((double)player.Y / SizeValue) - 1})" +
                $" Up: ({Math.Floor((double)player.X / SizeValue)}, {Math.Ceiling((double)player.X / SizeValue)}; {Math.Ceiling((double)player.Y / SizeValue) + 1})" +
                $" Left: ({Math.Ceiling((double)player.X / SizeValue) - 1}; {Math.Floor((double)player.Y / SizeValue)}, {Math.Ceiling((double)player.Y / SizeValue)})" +
                $" Right: ({Math.Floor((double)player.X / SizeValue) + 1}; {Math.Floor((double)player.Y / SizeValue)}, {Math.Ceiling((double)player.Y / SizeValue)})";
            if (player.IsDead)
                Restart();
            Invalidate();
        }

        private void Restart()
        {
            player = new Player(map.Width / 12 * SizeValue, map.Height / 12 * SizeValue, SizeValue);
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
                {
                    if (map.Level[i, j].Name == "Border")
                        g.DrawImage(Border, ConvertMathToWorld(i * SizeValue, j * SizeValue));
                    if (map.Level[i, j].Name == "Block")
                        g.DrawImage(Block, ConvertMathToWorld(i * SizeValue, j * SizeValue));
                    if (map.Level[i, j].Name == "Spike")
                        g.DrawImage(Spike, ConvertMathToWorld(i * SizeValue, j * SizeValue));
                }
            g.DrawImage(playerSkin, ConvertMathToWorld(player.X, player.Y));
        }

        private Image FitInSize(Image image) =>
            new Bitmap(image, BlockSize);

        private Point ConvertMathToWorld(int x, int y) =>
            new Point(x, (map.Height - 1) * SizeValue - y);
    }
}
