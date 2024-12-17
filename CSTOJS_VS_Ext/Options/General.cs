using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CSTOJS_VS_Ext
{
	internal partial class OptionsProvider
	{
		// Register the options with this attribute on your package class:
		// [ProvideOptionPage(typeof(OptionsProvider.GeneralOptions), "VSIXProjectTest", "General", 0, 0, true, SupportsProfiles = true)]
		[ComVisible(true)]
		public class GeneralOptions : BaseOptionPage<General> { }
	}

	public class General : BaseOptionModel<General>
	{
		[Category("Extension options")]
		[DisplayName("Path To CSTOJS_CLI")]
		[Description("Path To CSTOJS_CLI. Default is null(empty string), means using Embedded CSTOJS_CLI.")]
		[DefaultValue("")]
		public string PathToCSTOJS_CLI { get; set; } = string.Empty;

		[Category("CSharpToJavaScript options")]
		[DisplayName("Debug")]
		[Description("Debug. When set to true prints additional info to console, cs lines to js file.")]
		[DefaultValue(false)]
		public bool Debug { get; set; } = false;
	}
}
