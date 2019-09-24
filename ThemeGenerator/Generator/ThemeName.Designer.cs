namespace ThemeGenerator.Generator
{
	partial class ThemeInfo
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtThemeName = new System.Windows.Forms.TextBox();
			this.cbxBaseTheme = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(33, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Theme Name";
			// 
			// txtThemeName
			// 
			this.txtThemeName.Location = new System.Drawing.Point(187, 53);
			this.txtThemeName.Name = "txtThemeName";
			this.txtThemeName.Size = new System.Drawing.Size(463, 26);
			this.txtThemeName.TabIndex = 1;
			this.txtThemeName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtThemeName_KeyPress);
			// 
			// cbxBaseTheme
			// 
			this.cbxBaseTheme.FormattingEnabled = true;
			this.cbxBaseTheme.Location = new System.Drawing.Point(187, 13);
			this.cbxBaseTheme.Name = "cbxBaseTheme";
			this.cbxBaseTheme.Size = new System.Drawing.Size(223, 28);
			this.cbxBaseTheme.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(33, 21);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(99, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "Base Theme";
			// 
			// ThemeName
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 102);
			this.ControlBox = false;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cbxBaseTheme);
			this.Controls.Add(this.txtThemeName);
			this.Controls.Add(this.label1);
			this.Name = "ThemeName";
			this.Text = "Theme Name";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtThemeName;
		private System.Windows.Forms.ComboBox cbxBaseTheme;
		private System.Windows.Forms.Label label2;
	}
}