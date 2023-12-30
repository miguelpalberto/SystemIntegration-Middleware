using SomiodWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace SomiodWebService.Controllers
{
	[Route("api/somiod")]
	public class ApplicationController : ApiController
	{
		[HttpGet]
		public IEnumerable<Application> GetApplications()
		{
			using (var context = new SomiodDbContext())
			{
				return context.Applications.AsNoTracking().ToList();
			}
		}

		[HttpPost]
		public HttpResponseMessage PostApplication([FromBody] Application application)
		{
			if (application == null)
			{
				return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Body is empty.");
			}

			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Invalid application format.");
			}

			using (var context = new SomiodDbContext())
			{
				application.Creation_Dt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
				_ = context.Applications.Add(application);
				_ = context.SaveChanges();

				return Request.CreateResponse(System.Net.HttpStatusCode.Created, application, Configuration.Formatters.XmlFormatter);
			}
		}
	}
}
