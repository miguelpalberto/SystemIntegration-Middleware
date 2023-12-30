using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SomiodWebService.Models.BaseModels
{
	/// <summary>
	/// Abstract class for resources that have a parent resource. <see cref="TParent"/> is the generic type of the parent resource.
	/// </summary>
	/// <typeparam name="TParent"></typeparam>
	public abstract class ChildResource<TParent> : Resource where TParent : class, new()
	{
		[Required]
		public int Parent { get; set; }

		[ForeignKey(nameof(Parent))]
		public TParent ParentResource { get; set; }
	}
}
