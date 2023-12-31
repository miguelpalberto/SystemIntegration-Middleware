using System.Xml;
using System.Xml.Serialization;

namespace SomiodWebService.Services
{
	public static class XMLConversionService
	{
		public static XmlDocument ConvertToXMLDocument(object obj)
		{
			var xmlDocument = new XmlDocument();
			var xmlSerializer = new XmlSerializer(obj.GetType());

			using (var xmlStream = new System.IO.StringWriter())
			{
				xmlSerializer.Serialize(xmlStream, obj);
				xmlDocument.LoadXml(xmlStream.ToString());
			}

			return xmlDocument;
		}
	}
}
