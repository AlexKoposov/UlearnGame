﻿
namespace Project_Jumper
{
    partial class GameWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameWindow));
            this.GameTime = new System.Windows.Forms.Timer(this.components);
            this.LevelTime = new System.Windows.Forms.Timer(this.components);
            this.TimeLabel = new System.Windows.Forms.Label();
            this.FinishMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // GameTime
            // 
            this.GameTime.Enabled = true;
            this.GameTime.Interval = 10;
            this.GameTime.Tick += new System.EventHandler(this.Update);
            // 
            // LevelTime
            // 
            this.LevelTime.Interval = 1000;
            this.LevelTime.Tick += new System.EventHandler(this.LevelTime_Tick);
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.Font = new System.Drawing.Font("Calibri", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.TimeLabel.Location = new System.Drawing.Point(1018, 9);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(154, 78);
            this.TimeLabel.TabIndex = 0;
            this.TimeLabel.Text = "time";
            // 
            // FinishMessage
            // 
            this.FinishMessage.Font = new System.Drawing.Font("Arial", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.FinishMessage.ForeColor = System.Drawing.Color.White;
            this.FinishMessage.Location = new System.Drawing.Point(0, 0);
            this.FinishMessage.Name = "FinishMessage";
            this.FinishMessage.Size = new System.Drawing.Size(304, 111);
            this.FinishMessage.TabIndex = 1;
            this.FinishMessage.Text = "finish";
            this.FinishMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.FinishMessage.Visible = false;
            this.FinishMessage.Click += new System.EventHandler(this.FinishMessage_Click);
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.FinishMessage);
            this.Controls.Add(this.TimeLabel);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GameWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Jumper";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Update);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Timer GameTime;
        public System.Windows.Forms.Timer LevelTime;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Label FinishMessage;
    }
}
