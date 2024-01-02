
using System.Xml.Serialization;

namespace IlluminationApp.Models
{
	[XmlRoot("Error")]
	public class Error
	{
		[XmlElement("Message")]
		public string Message { get; set; }
	}
}
