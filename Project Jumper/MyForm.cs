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

        public MyForm()
        {
            InitializeComponent();

            timer1.Interval = 10;
            timer1.Tick += new EventHandler(Update);

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
                case Keys.Space:
                    player.IsJumping = false;
                    player.JumpStart = null;
                    break;
            }
            if (!player.IsLeftMoving && !player.IsRightMoving)
                player.IsMoving = false;
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
            if (player.IsLeftMoving || player.IsRightMoving)
                player.IsMoving = true;
        }

        void Initialise()
        {
            currentPath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName.ToString();
            player = new Player(1000, 400);
            playerSkin = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Cube.png")));
            Border = FitInSize(new Bitmap(Path.Combine(currentPath, "Resources\\Border.png")));
            map = new Map(38, 21);
            timer1.Start();
        }

        public void Update(object sender, EventArgs e)
        {
            if (player.IsMoving || player.IsJumping)
                player.Move();
            Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (var i = 0; i < map.Width; i++)
                for (var j = 0; j < map.Height; j++)
                    if (map.Level[i, j] == 'X')
                        g.DrawImage(Border, i * SizeValue, j * SizeValue);
            g.DrawImage(playerSkin, player.X, player.Y);
        }

        private Image FitInSize(Image image) =>
            new Bitmap(image, BlockSize);


    }
}
