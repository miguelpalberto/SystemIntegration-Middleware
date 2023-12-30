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
		public HttpResponseMessage GetApplications()
		{
			using (var context = new SomiodDbContext())
			{
				if (Request.Headers.Any(h => h.Key == "somiod-discover" && h.Value.Contains("application")))
				{
					var containerNames = context.Applications.AsNoTracking().Select(c => c.Name).ToList();
					return Request.CreateResponse(System.Net.HttpStatusCode.OK, containerNames);
				}

				return Request.CreateResponse(System.Net.HttpStatusCode.OK, context.Applications.AsNoTracking().ToList());
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

		[HttpPut, Route("api/somiod/{application}")]
		public HttpResponseMessage PutApplication(string application, [FromBody] Application updatedApplication)
		{
			if (updatedApplication == null)
			{
				return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Body is empty.");
			}

			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Invalid application format.");
			}

			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Application not found.");
				}

				var storedEntity = queryable.First();
				storedEntity.Name = updatedApplication.Name;
				_ = context.SaveChanges();

				return Request.CreateResponse(System.Net.HttpStatusCode.OK, storedEntity);
			}
		}

		[HttpDelete, Route("api/somiod/{application}")]
		public HttpResponseMessage DeleteApplication(string application)
		{
			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Application not found.");
				}

				var storedEntity = queryable.First();
				_ = context.Applications.Remove(storedEntity);
				_ = context.SaveChanges();

				return Request.CreateResponse(System.Net.HttpStatusCode.OK);
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

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Container not found.");
				}

				if (Request.Headers.Any(h => h.Key == "somiod-discover" && h.Value.Contains("subscription")))
				{
					var subscriptionNames = context.Subscriptions.AsNoTracking().Where(s => s.Parent == containerEntity.Id).Select(s => s.Name).ToList();
					return Request.CreateResponse(System.Net.HttpStatusCode.OK, subscriptionNames);
				}

				if (Request.Headers.Any(h => h.Key == "somiod-discover" && h.Value.Contains("data")))
				{
					//todo: implement data discovery
				}

				return Request.CreateResponse(System.Net.HttpStatusCode.OK, containerEntity);
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

		[HttpPut, Route("api/somiod/{application}/{container}")]
		public HttpResponseMessage PutContainer(string application, string container, [FromBody] Container updatedContainer)
		{
			if (updatedContainer == null)
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
				var containerEntity = context.Containers.AsNoTracking().Where(c => c.Parent == applicationEntity.Id && c.Name == container).FirstOrDefault();

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Container not found.");
				}

				containerEntity.Name = updatedContainer.Name;
				_ = context.SaveChanges();

				return Request.CreateResponse(System.Net.HttpStatusCode.OK, containerEntity);
			}
		}

		[HttpDelete, Route("api/somiod/{application}/{container}")]
		public HttpResponseMessage DeleteContainer(string application, string container)
		{
			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containerEntity = context.Containers.Where(c => c.Parent == applicationEntity.Id && c.Name == container).FirstOrDefault();

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Container not found.");
				}

				_ = context.Containers.Remove(containerEntity);
				_ = context.SaveChanges();

				return Request.CreateResponse(System.Net.HttpStatusCode.OK);
			}
		}

		[HttpGet, Route("api/somiod/{application}/{container}/subscriptions")]
		public HttpResponseMessage GetSubscriptions(string application, string container)
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

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Container not found.");
				}

				var subscriptions = context.Subscriptions.AsNoTracking().Where(s => s.Parent == containerEntity.Id).ToList();
				return Request.CreateResponse(System.Net.HttpStatusCode.OK, subscriptions);
			}
		}

		[HttpGet, Route("api/somiod/{application}/{container}/{subscription}")]
		public HttpResponseMessage GetSubscription(string application, string container, string subscription)
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

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Container not found.");
				}

				var subscriptionEntity = context.Subscriptions.AsNoTracking().Where(s => s.Parent == containerEntity.Id && s.Name == subscription).FirstOrDefault();

				return subscriptionEntity == null
					? Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Subscription not found.")
					: Request.CreateResponse(System.Net.HttpStatusCode.OK, subscriptionEntity);
			}
		}

		[HttpPost, Route("api/somiod/{application}/{container}/subscriptions")]
		public HttpResponseMessage PostSubscription(string application, string container, [FromBody] Subscription subscription)
		{
			if (subscription == null)
			{
				return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Body is empty.");
			}

			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Invalid subscription format.");
			}

			var availableEvents = new List<string> { "1", "2", "12" };

			if (string.IsNullOrEmpty(subscription.Event) || !availableEvents.Contains(subscription.Event))
			{
				return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Invalid subscription event. Use 1 for creation, 2 for deletion and 12 for both");
			}

			using (var context = new SomiodDbContext())
			{

				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containerEntity = context.Containers.AsNoTracking().Where(c => c.Parent == applicationEntity.Id && c.Name == container).FirstOrDefault();

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Container not found.");
				}

				var uniqueName = subscription.Name.ToLowerInvariant();

				if (context.Subscriptions.Any(a => a.Name == uniqueName && a.Parent == containerEntity.Id))
				{
					uniqueName = $"{uniqueName}-{Guid.NewGuid()}";
				}

				subscription.Name = uniqueName;
				subscription.Parent = containerEntity.Id;
				subscription.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				_ = context.Subscriptions.Add(subscription);
				_ = context.SaveChanges();

				return Request.CreateResponse(System.Net.HttpStatusCode.Created, subscription);
			}
		}

		[HttpPost, Route("api/somiod/{application}/{container}/subscriptions/{subscription}")]
		public HttpResponseMessage DeleteSubscription(string subscription)
		{
			using (var context = new SomiodDbContext())
			{
				var storedEntity = context.Subscriptions.Where(s => s.Name == subscription).FirstOrDefault();

				if (storedEntity == null)
				{
					return Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, "Subscription not found.");
				}

				_ = context.Subscriptions.Remove(storedEntity);
				_ = context.SaveChanges();

				return Request.CreateResponse(System.Net.HttpStatusCode.OK);
			}
		}

	}
}
