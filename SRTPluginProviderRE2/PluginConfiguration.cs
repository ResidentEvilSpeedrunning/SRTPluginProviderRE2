using SRTPluginBase.Interfaces;

namespace SRTPluginProviderRE2
{
	public class PluginConfiguration : IPluginConfiguration
	{
		public bool Debug { get; set; }

		public PluginConfiguration()
		{
			Debug = false;
		}
	}
}
