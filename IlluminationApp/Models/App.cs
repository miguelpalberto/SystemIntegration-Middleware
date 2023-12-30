using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IlluminationApp.Models
{
    public class App
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        public App(string name)
        {
            Name = name;
        }
    }
}
