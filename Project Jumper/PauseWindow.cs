using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Project_Jumper
{
    public partial class PauseWindow : Form
    {
        private Rectangle screen;
        private new readonly GameWindow Parent;

        public PauseWindow()
        {
            Parent = (GameWindow)Application.OpenForms["GameWindow"];
            screen = Parent.screen;
            InitializeComponent();
            UpdateComponent();
        }

        private void UpdateComponent()
        {
            //BackgroundImage = 

            Title.Location = new Point((screen.Width - Title.Size.Width) / 2, 0);

            ContinueButton.Size = new Size(screen.Height / 3, screen.Height / 3);
            ContinueButton.Location = new Point((screen.Width - ContinueButton.Size.Width) / 2, (screen.Height - ContinueButton.Size.Height) / 3);

            ExitButton.Location = new Point((screen.Width - ExitButton.Size.Width) / 2, ContinueButton.Bottom + screen.Height / 10);

            PrevLevelButton.Location = new Point(ExitButton.Left - (screen.Width / 5 + PrevLevelButton.Size.Width), ExitButton.Top);

            NextLevelButton.Location = new Point(ExitButton.Right + screen.Width / 5, ExitButton.Top);

            ResetTimeButton.Location = new Point((screen.Width - ResetTimeButton.Size.Width) / 2, ExitButton.Bottom + screen.Height / 10);
        }

        private void PauseWindow_Load(object sender, EventArgs e)
        {

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
            Parent.map.ChangeToNextLevel();
            ExitForm();
        }

        private void PrevLevelButton_Click(object sender, EventArgs e)
        {
            Parent.map.ChangeToPrevLevel();
            ExitForm();
        }

        private void ResetTimeButton_Click(object sender, EventArgs e)
        {
            Parent.map.ResetBestTime();
            ExitForm();
        }

        private void ExitForm()
        {
            Parent.GameTime.Start();
            Parent.LevelTime.Start();
            Close();
        }
    }
}
