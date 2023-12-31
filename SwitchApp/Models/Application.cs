using System;
using System.Xml.Serialization;

namespace SwitchApp.Models
{
	[XmlRoot("Application")]
	public class Application
	{
		[XmlElement("Id")]
		public int Id { get; set; }

		[XmlElement("Name")]
		public string Name { get; set; }

		[XmlElement("CreationDate")]
		public DateTime CreationDate { get; set; }

		public Application()
		{
		}
		public Application(string name)
		{
			Name = name;
		}
	}
}
