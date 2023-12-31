﻿
using System.Xml.Serialization;

namespace SwitchApp.Models
{
	[XmlRoot(ElementName = "Data")]
	public class Data
	{

		[XmlElement(ElementName = "Content")]
		public string Content { get; set; }

		public Data() { }

		public Data(string content)
		{
			Content = content;
		}
	}
}
