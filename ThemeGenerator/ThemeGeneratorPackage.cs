﻿using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace ThemeGenerator
{
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[Guid(ThemeGeneratorPackage.PackageGuidString)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	public sealed class ThemeGeneratorPackage : AsyncPackage
	{
		public static DTE2 dte2;
		public static DTE dte;
		public const string PackageGuidString = "6acb733a-89d0-4556-b1af-4cba4d649daa";

		#region Package Members

		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
			await ImportSettings.InitializeAsync(this);
			dte2 = (DTE2)GetService(typeof(DTE));
		}


		#endregion
	}
}
