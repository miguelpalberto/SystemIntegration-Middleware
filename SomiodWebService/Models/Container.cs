using SomiodWebService.Models.BaseModels;
using System.Collections.Generic;

namespace SomiodWebService.Models
{
	public class Container : ChildResource<Application>
	{
		internal virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
		internal virtual ICollection<Data> Data { get; set; } = new List<Data>();
	}
}
