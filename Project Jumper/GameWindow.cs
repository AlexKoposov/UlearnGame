using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Project_Jumper
{
    public partial class GameWindow : Form
    {
        private readonly int SizeValue;
        private readonly Size BlockSize;
        private Rectangle screen;
        private string currentPath;
        private Player player;
        private Map map;
        private Image playerSkin, border, block, spike, saw, jumpOrb, gravityOrb, finish, timerBackground;
        private int degrees;
        private Rectangle camera;

        public GameWindow()
        {
            InitializeComponent();

            screen = Screen.FromControl(this).WorkingArea;
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
                case Keys.Space:
                    if (player.GameMode == Gamemodes.Jetpack)
                        player.IsFlying = false;
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
                    JumpAction();
                    break;
                case Keys.Space:
                    JumpAction();
                    break;
                case Keys.Up:
                    TriggerReadyAction();
                    break;
                case Keys.R:
                    Restart();
                    break;
                case Keys.D1:
                    player.GameMode = Gamemodes.Cube;
                    break;
                case Keys.D2:
                    player.GameMode = Gamemodes.Ball;
                    break;
                case Keys.D3:
                    player.GameMode = Gamemodes.Jetpack;
                    break;
            }
        }

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                TriggerReadyAction();
        }

        private void TriggerReadyAction()
        {
            player.TriggerTicks = 0;
        }

        private void JumpAction()
        {
            if (player.GameMode == Gamemodes.Jetpack)
                player.IsFlying = true;
            else player.IsJumping = true;
        }

        void Initialise()
        {
            currentPath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName.ToString();
            GetAllSprites();
            map = new Map();
            player = new Player(map.Start, SizeValue);
            camera = new Rectangle(new Point(0, 0), screen.Size);
            GameTime.Start();
        }

        private void GetAllSprites()
        {
            GetSprite(ref playerSkin, "Cube");
            GetSprite(ref border, "Border");
            GetSprite(ref block, "Block");
            GetSprite(ref spike, "Spike");
            GetSprite(ref saw, "Saw");
            GetSprite(ref jumpOrb, "Yellow_Orb");
            GetSprite(ref gravityOrb, "Blue_Orb");
            GetSprite(ref finish, "Finish");
            GetSprite(ref timerBackground, "Timer_Background");
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
                if (LevelTime.Enabled)
                    player.Move(map, SizeValue);
                else LevelTime.Start();

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
                            DrawMapElement(border, g, i, j);
                            break;
                        case "Block":
                            DrawMapElement(block, g, i, j);
                            break;
                        case "Spike":
                            DrawMapElement(spike, g, i, j);
                            break;
                        case "Saw":
                            DrawMapElement(RotateImage(saw, degrees * 4), g, i, j);
                            break;
                        case "JumpOrb":
                            DrawMapElement(jumpOrb, g, i, j);
                            break;
                        case "GravityOrb":
                            DrawMapElement(gravityOrb, g, i, j);
                            break;
                        case "Finish":
                            DrawMapElement(finish, g, i, j);
                            break;
                    }
            UpdatePlayerSkin();
            g.DrawImage(playerSkin, ApplyCameraOffset(player.X, player.Y));
        }

        private void UpdatePlayerSkin()
        {
            switch (player.GameMode)
            {
                case Gamemodes.Ball:
                    GetSprite(ref playerSkin, "Ball");
                    break;
                default:
                    GetSprite(ref playerSkin, "Cube");
                    break;
            }
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

        private void DrawMapElement(Image e, Graphics g, int i, int j)
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
