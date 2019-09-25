using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Windows.Forms;


using Task = System.Threading.Tasks.Task;

namespace ThemeGenerator
{
	internal sealed class ImportSettings
	{
		public const int CommandId = 0x0100;

		public static readonly Guid CommandSet = new Guid("ff67bee8-6e4f-43bc-a10c-70cc3e442f81");

		private readonly AsyncPackage package;

		private ImportSettings(AsyncPackage package, OleMenuCommandService commandService)
		{
			this.package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

			var menuCommandID = new CommandID(CommandSet, CommandId);
			var menuItem = new MenuCommand(this.Execute, menuCommandID);
			commandService.AddCommand(menuItem);
		}

		public static ImportSettings Instance
		{
			get;
			private set;
		}

		private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
		{
			get
			{
				return this.package;
			}
		}

		public static async Task InitializeAsync(AsyncPackage package)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

			OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
			Instance = new ImportSettings(package, commandService);
		}

		private void Execute(object sender, EventArgs e)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			var filePath = PickSettingsFile();
			var directoryToSave = (this.package as ThemeGeneratorPackage).ThemeOutputDirectory;
			if (!string.IsNullOrEmpty(filePath))
			{
				CollecteThemeInfo();
				var generator = new ThemeGenerator.Generator.Generator(filePath,directoryToSave);
				generator.GenerateTheme();				
				MessageBox.Show("Theme Created");
			}
		}

		string PickSettingsFile()
		{
			var filePath = string.Empty;
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "VS Settings files (*.vssettings)|*.vssettings";
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					filePath = openFileDialog.FileName;
				}
				return filePath;
			}
		}
		void CollecteThemeInfo()
		{
			ThemeGenerator.Generator.ThemeInfo form = new ThemeGenerator.Generator.ThemeInfo();
			form.BringToFront();
			form.ShowDialog();
		}
	}
}
