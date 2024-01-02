using IlluminationApp.BaseModels;
using System.Xml.Serialization;

namespace IlluminationApp.Models
{
	[XmlRoot("Container")]
	public class Container : ChildResource<Application>
	{
		public Container()
		{
		}

		public Container(string name) : base(name)
		{
		}
	}
}
