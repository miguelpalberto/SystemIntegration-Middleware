using System.ComponentModel.DataAnnotations;

namespace SwitchApp.Models.BaseModels
{
	public abstract class Resource
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(255)]
		public string Name { get; set; }

		public string CreatedDate { get; set; }
	}
}
