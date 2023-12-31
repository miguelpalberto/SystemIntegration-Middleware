using System.Xml.Serialization;

namespace SomiodWebService.Models
{
	[XmlRoot("Notification")]
	public class Notification
	{
		[XmlElement("EventType")]
		public string EventType { get; set; }

		[XmlElement("Content")]
		public string Content { get; set; }
	}
}
