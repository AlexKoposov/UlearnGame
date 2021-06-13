using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Media;

namespace Project_Jumper
{
    //Управление: A, D, LEFT, RIGHT
    //Прыжок: W, SPACE, UP
    //Взаимодействие со сферами: ЛКМ
    //Пауза: ESCAPE
    //Перезапуск уровня: R
    //Сообщение о прохождении уровня можно закрыть: ЛКМ, ESCAPE, SPACE, ENTER

    public partial class GameWindow : Form
    {
        public bool IsLastLevelCompleted { get; set; }
        public bool PauseIsOpened { get; set; }
        public Rectangle Screen { get; private set; }
        public Map Map { get; private set; }
        public string CurrentPath { get; private set; }
        private Player player;
        private Image playerSkin, cubeSkin, ballSkin, jetSkin, border, block, spike, saw, rotatedSaw,
            jumpOrb, gravityOrb, finish, timerBackground, cubePortal, rotatedCubePortal, ballPortal,
            rotatedBallPortal, jetPortal, rotatedJetPortal;
        private int degrees, lastSawRotation, lastCubePortalRotation,
            lastBallPortalRotation, lastJetPortalRotation;
        private Rectangle camera;
        private bool playerLastMoveWasRight = true, isMessageShowed;
        private PauseWindow pause;
        private readonly int sizeValue;
        private readonly Size blockSize;
        public readonly SoundPlayer SP;

        public GameWindow()
        {
            Screen = System.Windows.Forms.Screen.FromControl(this).WorkingArea;
            Screen = new Rectangle(0, 0, Screen.Width, Screen.Height + 40);
            sizeValue = Screen.Height / 11;
            blockSize = new Size(new Point(sizeValue, sizeValue));

            InitializeComponent();
            InitialiseForm();

            SP = new SoundPlayer(Path.Combine(CurrentPath, "Resources\\MainTheme.wav"));
            SP.PlayLooping();
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
                    if (isMessageShowed)
                        FinishMessageAction();
                    break;
                case Keys.Up:
                    JumpAction();
                    break;
                case Keys.R:
                    Restart();
                    break;
                case Keys.Escape:
                    if (!isMessageShowed)
                        PauseGame();
                    else FinishMessageAction();
                    break;
                case Keys.Enter:
                    if (isMessageShowed)
                        FinishMessageAction();
                    break;

                    //DevTools

                    //case Keys.D1:
                    //    player.GameMode = Gamemodes.Cube;
                    //    DisableFlying();
                    //    break;
                    //case Keys.D2:
                    //    player.GameMode = Gamemodes.Ball;
                    //    DisableFlying();
                    //    break;
                    //case Keys.D3:
                    //    player.GameMode = Gamemodes.Jetpack;
                    //    break;
                    //case Keys.D0:
                    //    map.ResetBestTime();
                    //    break;
                    //case Keys.NumPad6:
                    //    LoadNextLevel();
                    //    break;
                    //case Keys.NumPad4:
                    //    LoadPrevLevel();
                    //    return;
                    //    break;
            }
        }

        public void LoadPrevLevel()
        {
            Map.ChangeToPrevLevel();
            Restart();
        }

        public void LoadNextLevel()
        {
            Map.ChangeToNextLevel();
            Restart();
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

            if (player.IsLevelCompleted && !isMessageShowed)
            {
                LevelTime.Stop();
                GameTime.Stop();
                Map.UpdateBestTime();
                ShowMessage();
                isMessageShowed = true;
            }

            if (player.Dead) Restart();

            if (player.TriggerTicks != 0) player.ReactToOrbs(Map, sizeValue);

            if (player.Moving)
            {
                if (LevelTime.Enabled)
                    player.Move(Map, sizeValue);
                else LevelTime.Start();
            }

            UpdateTimeLabel();
            Invalidate();
        }

        private void PauseGame()
        {
            if (!PauseIsOpened)
            {
                if (pause == null)
                    pause = new PauseWindow();
                PauseIsOpened = true;
                GameTime.Stop();
                LevelTime.Stop();
                pause.Show();
                Cursor.Show();
            }
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

        private void Restart()
        {
            player = new Player(Map.StartPosition, sizeValue);
            LevelTime.Stop();
            Map.ResetTime();
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

        private void InitialiseForm()
        {
            CurrentPath = new DirectoryInfo(Directory.GetCurrentDirectory()).FullName.ToString();
            GetAllSprites();
            Map = new Map();
            player = new Player(Map.StartPosition, sizeValue);
            camera = new Rectangle(new Point(0, 0), Screen.Size);
            GameTime.Start();
            Cursor.Hide();
            TimeLabel.Font = new Font("Calibri", 48F * Screen.Width / 1920, FontStyle.Bold, GraphicsUnit.Point);
        }

        private void GetAllSprites()
        {
            GetSprite(ref cubeSkin, "Cube");
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
            GetSprite(ref cubeSkin, "Cube");
            GetSprite(ref ballSkin, "Ball");
            GetSprite(ref jetSkin, "Jetpack");
        }

        private void GetSprite(ref Image inGameSprite, string spriteName)
        {
            ImageExtensions.GetSprite(ref inGameSprite, spriteName, CurrentPath, blockSize);
        }

        private void UpdateTimeLabel()
        {
            var time = Map.LevelTimeSeconds;
            TimeLabel.Text = LevelConverter.ConvertToDefaultTime(time);
            TimeLabel.Location = new Point(Screen.Width - TimeLabel.Size.Width + 1, 0);
            TimeLabel.Image = ImageExtensions.FitInSize(timerBackground, TimeLabel.Size);
        }

        private void LevelTime_Tick(object sender, EventArgs e)
        {
            Map.IncreaseTime();
        }

        private void ShowMessage()
        {
            UpdateFinishMessage();
            FinishMessage.Visible = true;
        }

        private void UpdateFinishMessage()
        {
            var time = Map.LevelTimeSeconds;
            FinishMessage.Text = @$"{(IsLastLevelCompleted ? "ИГРА ПРОЙДЕНА!" : "УРОВЕНЬ ПРОЙДЕН!")}

ВАШЕ ВРЕМЯ: {LevelConverter.ConvertToDefaultTime(time)}
ЛУЧШЕЕ ВРЕМЯ: {LevelConverter.ConvertToDefaultTime(Map.BestLevelTime)}";
            FinishMessage.Size = Screen.Size;
            FinishMessage.BackColor = Color.FromArgb(230, 64, 64, 64);
            FinishMessage.Font = new Font("Arial", 100F * Screen.Width / 1920, FontStyle.Bold, GraphicsUnit.Point);
        }

        private void FinishMessage_Click(object sender, EventArgs e)
        {
            if (isMessageShowed)
                FinishMessageAction();
        }

        private void FinishMessageAction()
        {
            FinishMessage.Visible = false;
            LoadNextLevel();
            isMessageShowed = false;
            GameTime.Start();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            UpdateCamera();
            DrawMap(g);
        }

        private void DrawMap(Graphics g)
        {
            var converted = ConvertMathToWorld(camera.X, camera.Y);
            var mapY = (converted.Y - camera.Height) / sizeValue;
            if (mapY < 0) mapY = 0;
            for (var i = converted.X / sizeValue; i < Map.Width
                && i <= (converted.X + camera.Width) / sizeValue; i++)
                for (var j = mapY; j < Map.Height && j <= converted.Y / sizeValue; j++)
                    switch (Map.Level[i, j].Type)
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
                            UpdateRotatedImage(saw, ref rotatedSaw, ref lastSawRotation, 3);
                            DrawMapElement(rotatedSaw, g, i, j);
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
                            UpdateRotatedImage(cubePortal, ref rotatedCubePortal, ref lastCubePortalRotation, 1);
                            DrawMapElement(rotatedCubePortal, g, i, j);
                            break;
                        case "BallPortal":
                            UpdateRotatedImage(ballPortal, ref rotatedBallPortal, ref lastBallPortalRotation, 1);
                            DrawMapElement(rotatedBallPortal, g, i, j);
                            break;
                        case "JetPortal":
                            UpdateRotatedImage(jetPortal, ref rotatedJetPortal, ref lastJetPortalRotation, 1);
                            DrawMapElement(rotatedJetPortal, g, i, j);
                            break;
                    }
            UpdatePlayerSkin();
            g.DrawImage(playerSkin, ApplyCameraOffset(player.X, player.Y));
        }

        private void UpdateRotatedImage(Image originalImg, ref Image rotatedImg, ref int lastRotation, int degrees)
        {
            if (this.degrees != lastRotation)
            {
                rotatedImg = ImageExtensions.RotateImage(originalImg, this.degrees * degrees);
                lastRotation = this.degrees;
            }
        }

        private void UpdatePlayerSkin()
        {
            switch (player.GameMode)
            {
                case Gamemodes.Ball:
                    playerSkin = ballSkin;
                    break;
                case Gamemodes.Jetpack:
                    playerSkin = new Bitmap(jetSkin);
                    if (playerLastMoveWasRight)
                        playerSkin.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    if (player.Gravity == -1)
                        playerSkin.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    break;
                default:
                    playerSkin = cubeSkin;
                    break;
            }
        }

        private void DrawMapElement(Image e, Graphics g, int i, int j)
        {
            g.DrawImage(e, ApplyCameraOffset(i * sizeValue, j * sizeValue));
        }

        private Point ConvertMathToWorld(int x, int y) =>
            new Point(x, Map.Height * sizeValue - y);

        private Point ApplyCameraOffset(int x, int y)
        {
            var point = ConvertMathToWorld(x, y);
            point.X -= camera.X;
            point.Y -= camera.Y + sizeValue;
            return point;
        }

        private void UpdateCamera()
        {
            camera.X = player.X - camera.Width / 2 + sizeValue;
            if (camera.X < 0) camera.X = 0;
            else if (camera.X + camera.Width > Map.Width * sizeValue)
                camera.X = Map.Width * sizeValue - camera.Width;

            camera.Y = Map.Height * sizeValue - player.Y - camera.Height / 2 + sizeValue / 2;
            if (camera.Y < 0) camera.Y = 0;
            else if (camera.Y + camera.Height > Map.Height * sizeValue)
                camera.Y = Map.Height * sizeValue - camera.Height;
        }
    }
}
