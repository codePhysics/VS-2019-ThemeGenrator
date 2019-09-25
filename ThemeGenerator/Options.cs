using Microsoft.VisualStudio.Shell;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using System.ComponentModel;
using Task = System.Threading.Tasks.Task;

namespace ThemeGenerator
{
	public class OptionPageGrid : DialogPage
	{
		[Category("Options")]
		[DisplayName("Theme Output Directory")]
		[Description("Theme Output Directory")]
		public string ThemeOutputDirectory { get; set; } = @"C:\VS 2019 Themes";
	}
}