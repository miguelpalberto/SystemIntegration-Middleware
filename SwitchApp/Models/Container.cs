
using System.Xml.Serialization;

namespace SwitchApp.Models
{
	[XmlRoot("Container")]
	public class Container : App
	{
		[XmlElement("Parent")]
		public string Parent { get; set; }
		public Container() : base()
		{
		}

		public Container(string name, string parent) : base(name)
		{
			Parent = parent;
		}
	}
}
