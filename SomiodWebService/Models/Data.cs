using SomiodWebService.Models.BaseModels;

namespace SomiodWebService.Models
{
	public class Data : ChildResource<Container>
	{
		public string Content { get; set; }
	}
}
