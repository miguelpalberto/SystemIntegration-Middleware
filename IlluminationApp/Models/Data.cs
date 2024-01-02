using IlluminationApp.BaseModels;
using System.Xml.Serialization;

namespace IlluminationApp.Models
{
	[XmlRoot("Data")]
	public class Data : ChildResource<Container>
	{
		[XmlElement("Content")]
		public string Content { get; set; }
	}
}
