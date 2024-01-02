using IlluminationApp.BaseModels;
using System.Xml.Serialization;

namespace IlluminationApp.Models
{
	[XmlRoot("Application")]
	public class Application : Resource
	{
		public Application()
		{
		}

		public Application(string name) : base(name)
		{
		}
	}
}
