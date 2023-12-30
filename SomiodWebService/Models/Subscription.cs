using SomiodWebService.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SomiodWebService.Models
{
	public class Subscription : ChildResource<Container>
	{
		[Required]
		public string Event { get; set; }

		[Required]
		public string Endpoint { get; set; }
	}
}
