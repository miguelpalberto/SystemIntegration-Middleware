
using SwitchApp.Models.BaseModels;
using System.Xml.Serialization;


namespace SwitchApp.Models
{
	[XmlRoot(ElementName = "Data")]
	public class Data : ChildResource<Container>
	{
		[XmlElement(ElementName = "Content")]
		public string Content { get; set; }

		public Data() { }
		public Data(string content)
		{
			Content = content;
		}
	}
}
