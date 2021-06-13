using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Jumper
{
    public partial class PauseWindow : Form
    {
        private Rectangle screen;
        private new readonly GameWindow Parent;
        private Image continueBtnSprite, nextBtnSprite, prevBtnSprite;

        public PauseWindow()
        {
            Parent = (GameWindow)Application.OpenForms["GameWindow"];
            screen = Parent.Screen;
            InitializeComponent();
            UpdateComponent();
        }

        private void UpdateComponent()
        {
            Title.Font = new Font("Arial", 160F * screen.Width / 1920, FontStyle.Bold, GraphicsUnit.Point);
            Title.Location = new Point((screen.Width - Title.Size.Width) / 2 + 1, 0);

            var contBtnSize = screen.Height / 3;
            ContinueButton.Size = new Size(contBtnSize, contBtnSize);
            ContinueButton.Location = new Point((screen.Width - ContinueButton.Size.Width) / 2,
                (screen.Height - ContinueButton.Height) / 2);
            ImageExtensions.GetSprite(ref continueBtnSprite, "Continue", Parent.CurrentPath, ContinueButton.Size);
            ContinueButton.Image = continueBtnSprite;

            var nextLvlBtnSize = ContinueButton.Height * 2 / 3;
            NextLevelButton.Size = new Size(nextLvlBtnSize, nextLvlBtnSize);
            NextLevelButton.Location = new Point(ContinueButton.Right + screen.Width / 10,
                ContinueButton.Top + (ContinueButton.Height - NextLevelButton.Height) / 2);
            ImageExtensions.GetSprite(ref nextBtnSprite, "Next", Parent.CurrentPath, NextLevelButton.Size);
            NextLevelButton.Image = nextBtnSprite;

            PrevLevelButton.Size = NextLevelButton.Size;
            PrevLevelButton.Location = new Point(ContinueButton.Left - (screen.Width / 10 + PrevLevelButton.Size.Width),
                ContinueButton.Top + (ContinueButton.Height - NextLevelButton.Height) / 2);
            prevBtnSprite = new Bitmap(nextBtnSprite);
            prevBtnSprite.RotateFlip(RotateFlipType.RotateNoneFlipX);
            PrevLevelButton.Image = prevBtnSprite;

            ExitButton.Font = new Font("Arial", 48F * screen.Width / 1920, FontStyle.Bold, GraphicsUnit.Point);
            ExitButton.Location = new Point((screen.Width - ExitButton.Size.Width) / 2,
                screen.Bottom - (ExitButton.Height + screen.Height / 20));

            ResetTimeButton.Font = new Font("Arial", 25F * screen.Width / 1920, FontStyle.Bold, GraphicsUnit.Point);
            ResetTimeButton.Location = new Point(screen.Width -
                (screen.Width / 20 + ResetTimeButton.Width), ExitButton.Top);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Parent.Close();
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            ExitForm();
        }

        private void NextLevelButton_Click(object sender, EventArgs e)
        {
            Parent.LoadNextLevel();
            ExitForm();
        }

        private void PrevLevelButton_Click(object sender, EventArgs e)
        {
            Parent.LoadPrevLevel();
            ExitForm();
        }

        private void PauseWindow_Load(object sender, EventArgs e)
        {

        }

        private void ResetTimeButton_Click(object sender, EventArgs e)
        {
            Parent.Map.ResetBestTime();
            ExitForm();
        }

        private void ExitForm()
        {
            Parent.PauseIsOpened = false;
            Parent.GameTime.Start();
            if (Parent.Map.LevelTimeSeconds != 0)
                Parent.LevelTime.Start();
            Cursor.Hide();
            Parent.Show();
            Hide();
        }
    }
}
