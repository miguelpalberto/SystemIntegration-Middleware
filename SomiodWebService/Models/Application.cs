using SomiodWebService.Models.BaseModels;
using System.Collections.Generic;

namespace SomiodWebService.Models
{
	public class Application : Resource
	{

		internal virtual ICollection<Container> Containers { get; set; } = new List<Container>();
	}
}
