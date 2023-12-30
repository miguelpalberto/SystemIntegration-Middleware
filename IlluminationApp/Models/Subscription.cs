
using System.Xml.Serialization;

namespace IlluminationApp.Models
{
    [XmlRoot("Subscription")]
    public class Subscription : Container
    {
        [XmlElement("EventType")]
        public string EventType { get; set; }

        [XmlElement("Endpoint")]
        public string Endpoint { get; set; }

        public Subscription(string name, string parent, string eventType, string endpoint) : base(name, parent)
        {
            EventType = eventType;
            Endpoint = endpoint;
        }
    }
}
