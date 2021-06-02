
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PauseWindow));
            this.Title = new System.Windows.Forms.Label();
            this.ContinueButton = new System.Windows.Forms.PictureBox();
            this.ExitButton = new System.Windows.Forms.Label();
            this.NextLevelButton = new System.Windows.Forms.PictureBox();
            this.PrevLevelButton = new System.Windows.Forms.PictureBox();
            this.ResetTimeButton = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ContinueButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextLevelButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrevLevelButton)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Arial", 100F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Title.ForeColor = System.Drawing.Color.White;
            this.Title.Location = new System.Drawing.Point(476, 9);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(511, 155);
            this.Title.TabIndex = 4;
            this.Title.Text = "ПАУЗА";
            this.Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ContinueButton
            // 
            this.ContinueButton.Location = new System.Drawing.Point(541, 167);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(384, 165);
            this.ContinueButton.TabIndex = 6;
            this.ContinueButton.TabStop = false;
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.AutoSize = true;
            this.ExitButton.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ExitButton.ForeColor = System.Drawing.Color.White;
            this.ExitButton.Location = new System.Drawing.Point(400, 560);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(587, 75);
            this.ExitButton.TabIndex = 7;
            this.ExitButton.Text = "ВЫХОД ИЗ ИГРЫ";
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // NextLevelButton
            // 
            this.NextLevelButton.Location = new System.Drawing.Point(1012, 221);
            this.NextLevelButton.Name = "NextLevelButton";
            this.NextLevelButton.Size = new System.Drawing.Size(192, 111);
            this.NextLevelButton.TabIndex = 8;
            this.NextLevelButton.TabStop = false;
            this.NextLevelButton.Click += new System.EventHandler(this.NextLevelButton_Click);
            // 
            // PrevLevelButton
            // 
            this.PrevLevelButton.Location = new System.Drawing.Point(214, 203);
            this.PrevLevelButton.Name = "PrevLevelButton";
            this.PrevLevelButton.Size = new System.Drawing.Size(210, 129);
            this.PrevLevelButton.TabIndex = 9;
            this.PrevLevelButton.TabStop = false;
            this.PrevLevelButton.Click += new System.EventHandler(this.PrevLevelButton_Click);
            // 
            // ResetTimeButton
            // 
            this.ResetTimeButton.AutoSize = true;
            this.ResetTimeButton.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ResetTimeButton.ForeColor = System.Drawing.Color.White;
            this.ResetTimeButton.Location = new System.Drawing.Point(1110, 571);
            this.ResetTimeButton.Name = "ResetTimeButton";
            this.ResetTimeButton.Size = new System.Drawing.Size(293, 64);
            this.ResetTimeButton.TabIndex = 10;
            this.ResetTimeButton.Text = "СБРОСИТЬ ЛУЧШЕЕ\n    ВРЕМЯ УРОВНЯ";
            this.ResetTimeButton.Click += new System.EventHandler(this.ResetTimeButton_Click);
            // 
            // PauseWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1415, 668);
            this.Controls.Add(this.ResetTimeButton);
            this.Controls.Add(this.PrevLevelButton);
            this.Controls.Add(this.NextLevelButton);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PauseWindow";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Jumper";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.ContinueButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextLevelButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PrevLevelButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.PictureBox ContinueButton;
        private System.Windows.Forms.Label ExitButton;
        private System.Windows.Forms.PictureBox NextLevelButton;
        private System.Windows.Forms.PictureBox PrevLevelButton;
        private System.Windows.Forms.Label ResetTimeButton;
    }
}