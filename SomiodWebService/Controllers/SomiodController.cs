using SomiodWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace SomiodWebService.Controllers
{
	public class SomiodController : ApiController
	{
		[HttpGet, Route("api/somiod")]
		public IEnumerable<Application> GetApplications()
		{
			using (var context = new SomiodDbContext())
			{
				return context.Applications.AsNoTracking().ToList();
			}
		}

		[HttpGet, Route("api/somiod/{application}")]
		public HttpResponseMessage GetApplication(string application)
		{
			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Application not found.");
				}

				var storedEntity = queryable.First();

				if (Request.Headers.Any(h => h.Key == "somiod-discover" && h.Value.Contains("container")))
				{
					var containerNames = context.Containers.AsNoTracking().Where(c => c.Parent == storedEntity.Id).Select(c => c.Name).ToList();
					return Request.CreateResponse(System.Net.HttpStatusCode.OK, containerNames);
				}
				else
				{
					return Request.CreateResponse(System.Net.HttpStatusCode.OK, storedEntity);
				}

			}
		}

		[HttpPost, Route("api/somiod")]
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
				var uniqueName = application.Name.ToLowerInvariant();

				if (context.Applications.Any(a => a.Name == uniqueName))
				{
					uniqueName = $"{uniqueName}-{Guid.NewGuid()}";
				}

				application.Name = uniqueName;
				application.CreatedDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
				_ = context.Applications.Add(application);
				_ = context.SaveChanges();

				return Request.CreateResponse(System.Net.HttpStatusCode.Created, application, Configuration.Formatters.XmlFormatter);
			}
		}

		[HttpGet, Route("api/somiod/{application}/containers")]
		public HttpResponseMessage GetContainers(string application)
		{
			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containers = context.Containers.AsNoTracking().Where(c => c.Parent == applicationEntity.Id).ToList();
				return Request.CreateResponse(System.Net.HttpStatusCode.OK, containers);
			}
		}

		[HttpGet, Route("api/somiod/{application}/{container}")]
		public HttpResponseMessage GetContainer(string application, string container)
		{
			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containerEntity = context.Containers.AsNoTracking().Where(c => c.Parent == applicationEntity.Id && c.Name == container).FirstOrDefault();

				return containerEntity == null
					? Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Container not found.")
					: Request.CreateResponse(System.Net.HttpStatusCode.OK, containerEntity);
			}
		}

		[HttpPost, Route("api/somiod/{application}")]
		public HttpResponseMessage PostContainer(string application, [FromBody] Container container)
		{
			if (container == null)
			{
				return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Body is empty.");
			}

			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Invalid container format.");
			}

			using (var context = new SomiodDbContext())
			{

				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();

				var uniqueName = container.Name.ToLowerInvariant();

				if (context.Containers.Any(a => a.Name == uniqueName && a.Parent == applicationEntity.Id))
				{
					uniqueName = $"{uniqueName}-{Guid.NewGuid()}";
				}

				container.Name = uniqueName;
				container.Parent = applicationEntity.Id;
				container.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				_ = context.Containers.Add(container);
				_ = context.SaveChanges();

				return Request.CreateResponse(System.Net.HttpStatusCode.Created, container);
			}
		}
	}
}
