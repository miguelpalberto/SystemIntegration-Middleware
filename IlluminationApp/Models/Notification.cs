
using System.Xml.Serialization;

namespace IlluminationApp.Models
{
	public class Notification
	{

		[XmlElement("EventType")]
		public string EventType { get; set; }

		[XmlElement("Data")]
		public Data Data { get; set; }
	}
}
