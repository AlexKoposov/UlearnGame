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
        static readonly int SizeValue = 50;
        static readonly Size BlockSize = new Size(new Point(SizeValue, SizeValue));
        string currentPath;
        Player player;
        Map map;
        Image playerSkin;
        Image Border;
        Image Block;

        public MyForm()
        {
            InitializeComponent();

            gameTime.Interval = 10;
            gameTime.Tick += new EventHandler(Update);

            KeyDown += new KeyEventHandler(OnPress);
            KeyUp += new KeyEventHandler(OnKeyUp);

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
            player = new Player(300, 100);
            playerSkin = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Cube.png")));
            Border = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Border.png")));
            Block = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Block.png")));
            map = new Map(38, 21);
            gameTime.Start();
        }

        public void Update(object sender, EventArgs e)
        {
            if (player.IsMoving)
                player.Move(map, SizeValue);
            Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (var i = 0; i < map.Width; i++)
                for (var j = 0; j < map.Height; j++)
                {
                    if (map.Level[i, j].Name == "Border")
                        g.DrawImage(Border, i * SizeValue, j * SizeValue);
                    if (map.Level[i, j].Name == "Block")
                        g.DrawImage(Block, i * SizeValue, j * SizeValue);
                }
            g.DrawImage(playerSkin, ConvertMathToWorld(player.X, player.Y));
        }

        private Image FitInSize(Image image) =>
            new Bitmap(image, BlockSize);

        private Point ConvertMathToWorld(int x, int y) =>
            new Point(x, map.Height * SizeValue - y);

    }
}
