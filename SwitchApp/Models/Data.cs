
using SwitchApp.Models.BaseModels;
using System.Xml.Serialization;

namespace SwitchApp.Models
{
	[XmlRoot(ElementName = "Data")]
	public class Data : ChildResource<Container>
	{
		[XmlElement(ElementName = "Name")]
		public new string Name { get; set; }

		[XmlElement(ElementName = "Content")]
		public string Content { get; set; }

		public Data() { }
		public Data(string name, string content)
		{
			Name = name;
			Content = content;
		}
	}
}
