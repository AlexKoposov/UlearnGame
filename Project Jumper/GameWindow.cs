using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Project_Jumper
{
    public partial class GameWindow : Form
    {
        Rectangle screen;
        readonly int SizeValue;
        readonly Size BlockSize;
        string currentPath;
        Player player;
        Map map;
        Image playerSkin, border, block, spike, saw, jumpOrb, gravityOrb, finish, timerBackground;
        int degrees;
        Rectangle camera;

        public GameWindow()
        {
            InitializeComponent();
            screen = Screen.FromControl(this).WorkingArea;

            GameTime.Interval = 10;
            GameTime.Tick += new EventHandler(Update);

            KeyDown += new KeyEventHandler(OnPress);
            KeyUp += new KeyEventHandler(OnKeyUp);

            SizeValue = Screen.FromControl(this).WorkingArea.Height / 11;
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
                case Keys.W:
                    player.IsJumping = true;
                    break;
                case Keys.Space:
                    player.IsJumping = true;
                    break;
                case Keys.Up:
                    player.TriggerTicks = 0;
                    break;
            }
        }

        public void OnMouseClick(object sender, MouseEventArgs e)
        {
            player.TriggerTicks = 0;
        }

        void Initialise()
        {
            currentPath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName.ToString();
            GetSprite(ref playerSkin, "Cube");
            GetSprite(ref border, "Border");
            GetSprite(ref block, "Block");
            GetSprite(ref spike, "Spike");
            GetSprite(ref saw, "Saw");
            GetSprite(ref jumpOrb, "Yellow_Orb");
            GetSprite(ref gravityOrb, "Blue_Orb");
            GetSprite(ref finish, "Finish");
            GetSprite(ref timerBackground, "Timer_Background");
            map = new Map();
            player = new Player(map.Start, SizeValue);
            camera = new Rectangle(new Point(0, SizeValue * 5), screen.Size);
            GameTime.Start();
            LevelTime.Start();
        }

        private void GetSprite(ref Image inGameSprite, string spriteName)
        {
            inGameSprite = FitInSize(new Bitmap(Path.Combine(currentPath, $"Resources\\{spriteName}.png")));
        }

        public void Update(object sender, EventArgs e)
        {
            degrees += 1;
            player.TriggerTicks++;
            if (player.IsLevelCompleted && !player.IsMessageShowed)
            {
                player.IsMessageShowed = true;
                player.Stop();
                LevelTime.Stop();
                ShowMessage();
            }
            if (player.IsDead)
                Restart();
            if (player.TriggerTicks != 0)
                player.ReactToOrbs(map, SizeValue);
            if (player.IsMoving)
                player.Move(map, SizeValue);

            //Text = $"Position: X = {player.X}, Y = {player.Y}, " +
            //    $"Map pos: ({Math.Round((double)player.X / SizeValue)}; {Math.Round((double)player.Y / SizeValue)})" +
            //    $" Down: ({Math.Floor((double)player.X / SizeValue)}, {Math.Ceiling((double)player.X / SizeValue)}; {Math.Floor((double)player.Y / SizeValue) - 1})" +
            //    $" Up: ({Math.Floor((double)player.X / SizeValue)}, {Math.Ceiling((double)player.X / SizeValue)}; {Math.Ceiling((double)player.Y / SizeValue) + 1})" +
            //    $" Left: ({Math.Ceiling((double)player.X / SizeValue) - 1}; {Math.Floor((double)player.Y / SizeValue)}, {Math.Ceiling((double)player.Y / SizeValue)})" +
            //    $" Right: ({Math.Floor((double)player.X / SizeValue) + 1}; {Math.Floor((double)player.Y / SizeValue)}, {Math.Ceiling((double)player.Y / SizeValue)})";

            //Text = $"Camera: X = {camera.X}, Y = {camera.Y}";

            UpdateTimeLabel();
            Invalidate();
        }

        private void UpdateTimeLabel()
        {
            var time = map.LevelTimeSeconds;
            TimeLabel.Text = $"{time / 60}:{string.Format("{0:00}", time % 60)}".ToString();
            TimeLabel.Location = new Point(screen.Width - TimeLabel.Size.Width + 1, 0);
            TimeLabel.Image = new Bitmap(timerBackground, TimeLabel.Size);
        }

        private void LevelTime_Tick(object sender, EventArgs e)
        {
            map.IncreaseTime();
        }

        private void ShowMessage()
        {
            var time = map.LevelTimeSeconds;
            MessageBox.Show(@$"Вы победили!
Ваше время: {time / 60}:{string.Format("{0:00}", time % 60)}");
        }

        private void Restart()
        {
            player = new Player(map.Start, SizeValue);
            LevelTime.Stop();
            LevelTime.Start();
            map.ResestTime();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawMap(g);
        }

        private void DrawMap(Graphics g)
        {
            var converted = ConvertMathToWorld(camera.X, camera.Y);
            for (var i = converted.X / SizeValue; i < map.Width && i <= (converted.X + camera.Width) / SizeValue; i++)
                for (var j = (converted.Y - camera.Height) / SizeValue; j < map.Height && j <= converted.Y / SizeValue + 1; j++)
                    switch (map.Level[i, j].Type)
                    {
                        case "Border":
                            DrawElement(border, g, i, j);
                            break;
                        case "Block":
                            DrawElement(block, g, i, j);
                            break;
                        case "Spike":
                            DrawElement(spike, g, i, j);
                            break;
                        case "Saw":
                            DrawElement(RotateImage(saw, degrees * 4), g, i, j);
                            break;
                        case "JumpOrb":
                            DrawElement(jumpOrb, g, i, j);
                            break;
                        case "GravityOrb":
                            DrawElement(gravityOrb, g, i, j);
                            break;
                        case "Finish":
                            DrawElement(finish, g, i, j);
                            break;
                    }
            g.DrawImage(playerSkin, ApplyCameraOffset(player.X, player.Y));
        }

        public static Bitmap RotateImage(Image image, float angle)
        {
            if (image == null) throw new ArgumentNullException();
            PointF offset = new PointF((float)image.Width / 2, (float)image.Height / 2);
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            Graphics g = Graphics.FromImage(rotatedBmp);
            g.TranslateTransform(offset.X, offset.Y);
            g.RotateTransform(angle);
            g.TranslateTransform(-offset.X, -offset.Y);
            g.DrawImage(image, new PointF(0, 0));
            return rotatedBmp;
        }

        private void DrawElement(Image e, Graphics g, int i, int j)
        {
            g.DrawImage(e, ApplyCameraOffset(i * SizeValue, j * SizeValue));
        }

        private Image FitInSize(Image image) =>
            new Bitmap(image, BlockSize);

        private Point ConvertMathToWorld(int x, int y) =>
            new Point(x, (map.Height - 1) * SizeValue - y);

        private Point ApplyCameraOffset(int x, int y)
        {
            var titleHeight = 17;
            var point = ConvertMathToWorld(x, y);
            camera.X = player.X - camera.Width / 2 + SizeValue;
            if (camera.X < 0) camera.X = 0;
            else if (camera.X + camera.Width > map.Width * SizeValue)
                camera.X = map.Width * SizeValue - camera.Width;

            camera.Y = (map.Height - 1) * SizeValue - player.Y - camera.Height / 2 + SizeValue / 2;
            if (camera.Y < 0) camera.Y = 0;
            else if (camera.Y + camera.Height + titleHeight > map.Height * SizeValue)
                camera.Y = map.Height * SizeValue - camera.Height - titleHeight;

            point.X -= camera.X;
            point.Y -= camera.Y;
            return point;
        }
    }
}
