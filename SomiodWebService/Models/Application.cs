using System.Collections.Generic;

namespace SomiodWebService.Models
{
	public class Application : Resource
	{

		internal virtual ICollection<Container> Containers { get; set; }
	}
}
