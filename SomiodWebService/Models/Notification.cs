using System.Xml.Serialization;

namespace SomiodWebService.Models
{
	[XmlRoot("Notification")]
	public class Notification
	{
		[XmlElement("EventType")]
		public string EventType { get; set; }

		[XmlElement("Data")]
		public Data Data { get; set; }

		public Notification()
		{
		}

		public Notification(string eventType, Data data)
		{
			EventType = eventType;
			Data = data;
		}
	}
}
