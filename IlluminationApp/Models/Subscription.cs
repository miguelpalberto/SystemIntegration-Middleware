
using IlluminationApp.BaseModels;
using System.Xml.Serialization;

namespace IlluminationApp.Models
{
	[XmlRoot("Subscription")]
	public class Subscription : ChildResource<Container>
	{
		[XmlElement("Event")]
		public string Event { get; set; }

		[XmlElement("Endpoint")]
		public string Endpoint { get; set; }

		public Subscription(string name, string eventType, string endpoint) : base(name)
		{
			Event = eventType;
			Endpoint = endpoint;
		}
	}
}
