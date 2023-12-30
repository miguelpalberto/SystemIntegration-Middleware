using System.ComponentModel.DataAnnotations;

namespace SomiodWebService.Models
{
	public class Application
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(255)]
		public string Name { get; set; }

		public string Creation_Dt { get; set; }
	}
}
