global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using System.Runtime.InteropServices;
using System.Threading;


namespace CSTOJS_VS_Ext
{
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[Guid(PackageGuids.CSTOJS_VS_ExtString)]
	[ProvideUIContextRule(PackageGuids.uiContextSupportedFilesString,
	name: "Supported Files",
	expression: "CSharp",
	termNames: new[] { "CSharp" },
	termValues: new[] { "HierSingleSelectionName:.cs$" })]
	[ProvideOptionPage(typeof(OptionsProvider.GeneralOptions), "CSTOJS VS Ext", "General", 0, 0, true, SupportsProfiles = true)]
	public sealed class CSTOJS_VS_ExtPackage : ToolkitPackage
	{
		public static OutputWindowPane Pane;
		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			Pane = await VS.Windows.CreateOutputWindowPaneAsync("CSTOJS_CLI output");
			await this.RegisterCommandsAsync();
		}
	}
}