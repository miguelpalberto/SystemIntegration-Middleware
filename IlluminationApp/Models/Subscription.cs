
using System.Xml.Serialization;

namespace IlluminationApp.Models
{
	[XmlRoot("Subscription")]
	public class Subscription : Container
	{
		[XmlElement("Event")]
		public string Event { get; set; }

		[XmlElement("Endpoint")]
		public string Endpoint { get; set; }

		public Subscription(string name, string parent, string eventType, string endpoint) : base(name, parent)
		{
			Event = eventType;
			Endpoint = endpoint;
		}
	}
}
