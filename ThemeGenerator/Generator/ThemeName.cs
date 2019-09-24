using System.Collections.Generic;
using System.Windows.Forms;

namespace ThemeGenerator.Generator
{
	public partial class ThemeInfo : Form
	{
		public static string ThemeName { get; set; }
		public static string BaseTheme { get; set; }
		public ThemeInfo()
		{
			InitializeComponent();
			cbxBaseTheme.DataSource = new List<string> { Constants.Dark, Constants.Light };
		}

		private void TxtThemeName_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)13)
			{
				ThemeName = txtThemeName.Text;
				BaseTheme = cbxBaseTheme.SelectedItem as string;
				this.Close();
			}
		}
	}
}
