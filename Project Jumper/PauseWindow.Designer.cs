
namespace Project_Jumper
{
    partial class PauseWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ExitButton = new System.Windows.Forms.Button();
            this.ContinueButton = new System.Windows.Forms.Button();
            this.NextLevelButton = new System.Windows.Forms.Button();
            this.PrevLevelButton = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.ResetTimeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.AutoSize = true;
            this.ExitButton.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ExitButton.Location = new System.Drawing.Point(532, 393);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(300, 92);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "ВЫЙТИ";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // ContinueButton
            // 
            this.ContinueButton.AutoSize = true;
            this.ContinueButton.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ContinueButton.Location = new System.Drawing.Point(441, 154);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(507, 177);
            this.ContinueButton.TabIndex = 1;
            this.ContinueButton.Text = "ПРОДОЛЖИТЬ";
            this.ContinueButton.UseVisualStyleBackColor = true;
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // NextLevelButton
            // 
            this.NextLevelButton.Location = new System.Drawing.Point(1038, 413);
            this.NextLevelButton.Name = "NextLevelButton";
            this.NextLevelButton.Size = new System.Drawing.Size(239, 126);
            this.NextLevelButton.TabIndex = 2;
            this.NextLevelButton.Text = ">";
            this.NextLevelButton.UseVisualStyleBackColor = true;
            this.NextLevelButton.Click += new System.EventHandler(this.NextLevelButton_Click);
            // 
            // PrevLevelButton
            // 
            this.PrevLevelButton.Location = new System.Drawing.Point(138, 413);
            this.PrevLevelButton.Name = "PrevLevelButton";
            this.PrevLevelButton.Size = new System.Drawing.Size(239, 126);
            this.PrevLevelButton.TabIndex = 3;
            this.PrevLevelButton.Text = "<";
            this.PrevLevelButton.UseVisualStyleBackColor = true;
            this.PrevLevelButton.Click += new System.EventHandler(this.PrevLevelButton_Click);
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Arial", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Title.Location = new System.Drawing.Point(476, 9);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(437, 111);
            this.Title.TabIndex = 4;
            this.Title.Text = "PAUSED";
            this.Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ResetTimeButton
            // 
            this.ResetTimeButton.Location = new System.Drawing.Point(565, 521);
            this.ResetTimeButton.Name = "ResetTimeButton";
            this.ResetTimeButton.Size = new System.Drawing.Size(239, 126);
            this.ResetTimeButton.TabIndex = 5;
            this.ResetTimeButton.Text = "Reset best time on current level";
            this.ResetTimeButton.UseVisualStyleBackColor = true;
            this.ResetTimeButton.Click += new System.EventHandler(this.ResetTimeButton_Click);
            // 
            // PauseWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1415, 668);
            this.Controls.Add(this.ResetTimeButton);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.PrevLevelButton);
            this.Controls.Add(this.NextLevelButton);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.ExitButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PauseWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PauseWindow";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PauseWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button ContinueButton;
        private System.Windows.Forms.Button NextLevelButton;
        private System.Windows.Forms.Button PrevLevelButton;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Button ResetTimeButton;
    }
}