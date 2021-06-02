using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Project_Jumper
{
    public partial class GameWindow : Form
    {
        public Rectangle screen;
        public Map map { get; private set; }
        private readonly int SizeValue;
        private readonly Size BlockSize;
        private string currentPath;
        private Player player;
        private Image playerSkin, border, block, spike, saw, jumpOrb, gravityOrb, finish, timerBackground, cubePortal, ballPortal, jetPortal;
        private int degrees;
        private Rectangle camera;
        private bool playerLastMoveWasRight = true;

        public GameWindow()
        {
            InitializeComponent();

            screen = Screen.FromControl(this).WorkingArea;
            screen.Height += 40;
            SizeValue = Screen.FromControl(this).WorkingArea.Height / 11;
            BlockSize = new Size(new Point(SizeValue, SizeValue));

            Initialise();
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    StopMovingLeft();
                    break;
                case Keys.Left:
                    StopMovingLeft();
                    break;
                case Keys.D:
                    StopMovingRight();
                    break;
                case Keys.Right:
                    StopMovingRight();
                    break;
                case Keys.Space:
                    DisableFlying();
                    break;
                case Keys.W:
                    DisableFlying();
                    break;
                case Keys.Up:
                    DisableFlying();
                    break;
            }
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    GoLeft();
                    break;
                case Keys.Left:
                    GoLeft();
                    break;
                case Keys.D:
                    GoRight();
                    break;
                case Keys.Right:
                    GoRight();
                    break;
                case Keys.W:
                    JumpAction();
                    break;
                case Keys.Space:
                    JumpAction();
                    break;
                case Keys.Up:
                    JumpAction();
                    break;
                case Keys.R:
                    Restart();
                    break;
                case Keys.Escape:
                    GameTime.Stop();
                    LevelTime.Stop();
                    PauseGame();
                    break;

                //DevTools
                
                case Keys.D1:
                    player.GameMode = Gamemodes.Cube;
                    DisableFlying();
                    break;
                case Keys.D2:
                    player.GameMode = Gamemodes.Ball;
                    DisableFlying();
                    break;
                case Keys.D3:
                    player.GameMode = Gamemodes.Jetpack;
                    break;
                //case Keys.D0:
                //    map.ResetBestTime();
                //    break;
                //case Keys.NumPad6:
                //    map.ChangeToNextLevel();
                //    Restart();
                //    break;
                //case Keys.NumPad4:
                //    map.ChangeToPrevLevel();
                //    Restart();
                //    break;
            }
        }

        private void PauseGame()
        {
            var pause = new PauseWindow();
            pause.Show();
        }

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                TriggerReadyAction();
        }

        public void Update(object sender, EventArgs e)
        {
            degrees += 1;
            player.TriggerTicks++;
            if (player.IsLevelCompleted
                && !player.IsMessageShowed)
            {
                player.IsMessageShowed = true;
                player.Stop();
                LevelTime.Stop();
                map.UpdateBestTime();
                ShowMessage();
                map.ChangeToNextLevel();
                Restart();
            }
            if (player.Dead)
                Restart();
            if (player.TriggerTicks != 0)
                player.ReactToOrbs(map, SizeValue);
            if (player.Moving)
            {
                if (LevelTime.Enabled)
                    player.Move(map, SizeValue);
                else LevelTime.Start();
            }

            UpdateTimeLabel();
            Invalidate();
        }

        private void GoRight()
        {
            player.MovingRight = true;
            playerLastMoveWasRight = true;
        }

        private void GoLeft()
        {
            player.MovingLeft = true;
            playerLastMoveWasRight = false;
        }

        private void StopMovingRight()
        {
            player.MovingRight = false;
        }

        private void StopMovingLeft()
        {
            player.MovingLeft = false;
        }

        private void DisableFlying()
        {
            player.Flying = false;
        }

        private void TriggerReadyAction()
        {
            player.TriggerTicks = 0;
        }

        private void JumpAction()
        {
            if (player.GameMode == Gamemodes.Jetpack)
                player.Flying = true;
            else player.Jumping = true;
        }

        private void Initialise()
        {
            currentPath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName.ToString();
            GetAllSprites();
            map = new Map();
            player = new Player(map.StartPosition, SizeValue);
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
            GetSprite(ref jumpOrb, "YellowOrb");
            GetSprite(ref gravityOrb, "BlueOrb");
            GetSprite(ref finish, "Finish");
            GetSprite(ref timerBackground, "TimerBackground");
            GetSprite(ref cubePortal, "GreenPortal");
            GetSprite(ref ballPortal, "RedPortal");
            GetSprite(ref jetPortal, "PurplePortal");
        }

        private void GetSprite(ref Image inGameSprite, string spriteName)
        {
            inGameSprite = FitInSize(new Bitmap(Path.Combine(currentPath, $"Resources\\{spriteName}.png")));
        }

        private void UpdateTimeLabel()
        {
            var time = map.LevelTimeSeconds;
            TimeLabel.Text = LevelConverter.ConvertToDefaultTime(time);
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
Ваше время: {LevelConverter.ConvertToDefaultTime(time)}
Лучшее время: {LevelConverter.ConvertToDefaultTime(map.BestLevelTime)}");
        }

        private void Restart()
        {
            player = new Player(map.StartPosition, SizeValue);
            LevelTime.Stop();
            map.ResetTime();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawMap(g);
        }

        private void DrawMap(Graphics g)
        {
            var converted = ConvertMathToWorld(camera.X, camera.Y);
            var futureJ = (converted.Y - camera.Height) / SizeValue;
            if (futureJ < 0)
                futureJ = 0;
            for (var i = converted.X / SizeValue; i < map.Width && i <= (converted.X + camera.Width) / SizeValue; i++)
                for (var j = futureJ; j < map.Height && j <= converted.Y / SizeValue + 1; j++)
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
                            DrawRotatedElement(saw, 4, g, i, j);
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
                        case "CubePortal":
                            DrawRotatedElement(cubePortal, 2, g, i, j);
                            break;
                        case "BallPortal":
                            DrawRotatedElement(ballPortal, 2, g, i, j);
                            break;
                        case "JetPortal":
                            DrawRotatedElement(jetPortal, 2, g, i, j);
                            break;
                    }
            UpdatePlayerSkin();
            g.DrawImage(playerSkin, ApplyCameraOffset(player.X, player.Y));
        }

        private void DrawRotatedElement(Image image, int degreesMultiplier, Graphics g, int i, int j)
        {
            DrawMapElement(RotateImage(image, degrees * degreesMultiplier), g, i, j);
        }

        private void UpdatePlayerSkin()
        {
            switch (player.GameMode)
            {
                case Gamemodes.Ball:
                    GetSprite(ref playerSkin, "Ball");
                    break;
                case Gamemodes.Jetpack:
                    GetSprite(ref playerSkin, "Jetpack");
                    if (playerLastMoveWasRight)
                        playerSkin.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    if (player.Gravity == -1)
                        playerSkin.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    break;
                default:
                    GetSprite(ref playerSkin, "Cube");
                    break;
            }
        }

        private static Bitmap RotateImage(Image image, float angle)
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
            var point = ConvertMathToWorld(x, y);

            camera.X = player.X - camera.Width / 2 + SizeValue;
            if (camera.X < 0) camera.X = 0;
            else if (camera.X + camera.Width > map.Width * SizeValue)
                camera.X = map.Width * SizeValue - camera.Width;

            camera.Y = (map.Height - 1) * SizeValue - player.Y - camera.Height / 2 + SizeValue / 2;
            if (camera.Y < 0) camera.Y = 0;
            else if (camera.Y + camera.Height > map.Height * SizeValue)
                camera.Y = map.Height * SizeValue - camera.Height;

            point.X -= camera.X;
            point.Y -= camera.Y;
            return point;
        }
    }
}
