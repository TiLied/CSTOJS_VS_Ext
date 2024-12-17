using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell.Interop;

namespace CSTOJS_VS_Ext
{
	[Command(PackageIds.MyCommand)]
	internal sealed class MyCommand : BaseCommand<MyCommand>
	{
		protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
		{
			PhysicalFile file = await VS.Solutions.GetActiveItemAsync() as PhysicalFile;

			if (file == null)
			{
				var model = new InfoBarModel(new[] {
					new InfoBarTextSpan("File path is null! Save it first!")
				}, KnownMonikers.PlayStepGroup,	true);

				InfoBar infoBar = await VS.InfoBar.CreateAsync(ToolWindowGuids80.SolutionExplorer, model);
				await infoBar.TryShowInfoBarUIAsync();
				return;
			}

			string strPath = file.FullPath;

			if(!strPath.EndsWith(".cs"))
			{
				var model = new InfoBarModel(new[] {
					new InfoBarTextSpan("File is NOT CSHARP!")
				}, KnownMonikers.PlayStepGroup, true);

				InfoBar infoBar = await VS.InfoBar.CreateAsync(ToolWindowGuids80.SolutionExplorer, model);
				await infoBar.TryShowInfoBarUIAsync();
				return;
			}

			string directory = Path.GetDirectoryName(strPath);

			General options = await General.GetLiveInstanceAsync();
			
			Process p = new();

			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.CreateNoWindow = true;

			if (options.PathToCSTOJS_CLI != null && options.PathToCSTOJS_CLI != "") 
				p.StartInfo.FileName = options.PathToCSTOJS_CLI;
			else
				p.StartInfo.FileName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\CSTOJS_CLI\\CSTOJS_CLI.exe";

			List<string> args = new(){ "f", $"\"{strPath}\"", "-OutPutPath", $"\"{directory}\"", "-DisableConsoleColors", "true" };

			if (options.Debug) 
			{
				args.Add("-Debug");
				args.Add($"true");
			}
			if (options.DisableConsoleOutput)
			{
				args.Add("-DisableConsoleOutput");
				args.Add($"true");
			}
			if (options.UseVarOverLet)
			{
				args.Add("-UseVarOverLet");
				args.Add($"true");
			}
			if (options.KeepBraceOnTheSameLine)
			{
				args.Add("-KeepBraceOnTheSameLine");
				args.Add($"true");
			}
			if (options.NormalizeWhitespace)
			{
				args.Add("-NormalizeWhitespace");
				args.Add($"true");
			}

			p.StartInfo.Arguments = string.Join(" ", args);

			try
			{
				p.Start();
			}
			catch (Exception ex)
			{
				ex.Log();
			}
			
			while (!p.StandardError.EndOfStream)
			{
				string line = await p.StandardError.ReadLineAsync();
				await CSTOJS_VS_ExtPackage.Pane.WriteLineAsync(line);
			}
			while (!p.StandardOutput.EndOfStream)
			{
				string line = await p.StandardOutput.ReadLineAsync();
				await CSTOJS_VS_ExtPackage.Pane.WriteLineAsync(line);
			}

			p.WaitForExit();
		}
	}
}
