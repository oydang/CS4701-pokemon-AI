namespace TestInput
{
    partial class Form1
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
            this.Show_Input = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Show_Input
            // 
            this.Show_Input.Location = new System.Drawing.Point(12, 12);
            this.Show_Input.Name = "Show_Input";
            this.Show_Input.ReadOnly = true;
            this.Show_Input.Size = new System.Drawing.Size(100, 20);
            this.Show_Input.TabIndex = 0;
            this.Show_Input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(124, 44);
            this.Controls.Add(this.Show_Input);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Inputs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Show_Input;

        public void UpdateText(string m)
        {
            Show_Input.Text = m;
        }
    }
}

