using System.Xml.Serialization;

namespace IlluminationApp.BaseModels
{
	public abstract class Resource
	{
		[XmlElement("Id")]
		public int Id { get; set; }

		[XmlElement("Name")]
		public string Name { get; set; }

		[XmlElement("CreatedDate")]
		public string CreatedDate { get; set; }

		protected Resource()
		{
		}

		protected Resource(string name)
		{
			Name = name;
		}
	}
}
