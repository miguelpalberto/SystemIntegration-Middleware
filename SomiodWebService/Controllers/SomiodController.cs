using SomiodWebService.Models;
using SomiodWebService.Services;
using SomiodWebService.Validations;
using SomiodWebService.Validations.Validators;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SomiodWebService.Controllers
{
	public class SomiodController : ApiController
	{

		//discovers application names through the somiod-discover header
		[HttpGet, Route("api/somiod")]
		public HttpResponseMessage GetApplications()
		{
			using (var context = new SomiodDbContext())
			{
				if (Request.Headers.Any(h => h.Key == "somiod-discover" && h.Value.Contains("application")))
				{
					var containerNames = context.Applications.AsNoTracking().Select(c => c.Name).ToList();
					return Request.CreateResponse(HttpStatusCode.OK, containerNames);
				}

				return Request.CreateResponse(HttpStatusCode.OK, context.Applications.AsNoTracking().ToList());
			}
		}

		//discovers container names through the somiod-discover header
		[HttpGet, Route("api/somiod/{application}")]
		public HttpResponseMessage GetApplication(string application)
		{
			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
				}

				var storedEntity = queryable.First();

				if (Request.Headers.Any(h => h.Key == "somiod-discover" && h.Value.Contains("container")))
				{
					var containerNames = context.Containers.AsNoTracking().Where(c => c.Parent == storedEntity.Id).Select(c => c.Name).ToList();
					return Request.CreateResponse(HttpStatusCode.OK, containerNames);
				}
				else
				{
					return Request.CreateResponse(HttpStatusCode.OK, storedEntity);
				}

			}
		}

		[HttpPost, Route("api/somiod")]
		public HttpResponseMessage PostApplication([FromBody] Application application)
		{
			if (application == null)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Body is empty.");
			}

			var validator = new ApplicationValidator();
			var validationResult = validator.Validate(application);

			if (!validationResult.IsValid)
			{
				return Request.CreateErrorResponse((HttpStatusCode)422, validationResult.ErrorMessages.First().Message);
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

				return Request.CreateResponse(HttpStatusCode.Created, application, Configuration.Formatters.XmlFormatter);
			}
		}

		[HttpPut, Route("api/somiod/{application}")]
		public HttpResponseMessage PutApplication(string application, [FromBody] Application updatedApplication)
		{
			if (updatedApplication == null)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Body is empty.");
			}

			var validator = new ApplicationValidator();
			var validationResult = validator.Validate(updatedApplication);

			if (!validationResult.IsValid)
			{
				return Request.CreateErrorResponse((HttpStatusCode)422, validationResult.ErrorMessages.First().Message);
			}

			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
				}

				var storedEntity = queryable.First();
				storedEntity.Name = updatedApplication.Name;
				_ = context.SaveChanges();

				return Request.CreateResponse(HttpStatusCode.OK, storedEntity);
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
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
				}

				var storedEntity = queryable.First();
				_ = context.Applications.Remove(storedEntity);
				_ = context.SaveChanges();

				return Request.CreateResponse(HttpStatusCode.OK);
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
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containers = context.Containers.AsNoTracking().Where(c => c.Parent == applicationEntity.Id).ToList();
				return Request.CreateResponse(HttpStatusCode.OK, containers);
			}
		}

		//discovers subscription or data names through the somiod-discover header
		[HttpGet, Route("api/somiod/{application}/{container}")]
		public HttpResponseMessage GetContainer(string application, string container)
		{
			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containerEntity = context.Containers.AsNoTracking().Where(c => c.Parent == applicationEntity.Id && c.Name == container).FirstOrDefault();

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Container not found.");
				}

				if (Request.Headers.Any(h => h.Key == "somiod-discover" && h.Value.Contains("subscription")))
				{
					var subscriptionNames = context.Subscriptions.AsNoTracking().Where(s => s.Parent == containerEntity.Id).Select(s => s.Name).ToList();
					return Request.CreateResponse(HttpStatusCode.OK, subscriptionNames);
				}

				if (Request.Headers.Any(h => h.Key == "somiod-discover" && h.Value.Contains("data")))
				{
					var dataNames = context.Data.AsNoTracking().Where(d => d.Parent == containerEntity.Id).Select(d => d.Name).ToList();
					return Request.CreateResponse(HttpStatusCode.OK, dataNames);
				}

				return Request.CreateResponse(HttpStatusCode.OK, containerEntity);
			}
		}

		[HttpPost, Route("api/somiod/{application}")]
		public HttpResponseMessage PostContainer(string application, [FromBody] Container container)
		{
			if (container == null)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Body is empty.");
			}

			var validator = new ContainerValidator();
			var validationResult = validator.Validate(container);

			if (!validationResult.IsValid)
			{
				return Request.CreateErrorResponse((HttpStatusCode)422, validationResult.ErrorMessages.First().Message);
			}

			using (var context = new SomiodDbContext())
			{

				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
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

				return Request.CreateResponse(HttpStatusCode.Created, container);
			}
		}

		[HttpPut, Route("api/somiod/{application}/containers/{container}")]
		public HttpResponseMessage PutContainer(string application, string container, [FromBody] Container updatedContainer)
		{
			if (updatedContainer == null)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Body is empty.");
			}

			var validator = new ContainerValidator();
			var validationResult = validator.Validate(updatedContainer);

			if (!validationResult.IsValid)
			{
				return Request.CreateErrorResponse((HttpStatusCode)422, validationResult.ErrorMessages.First().Message);
			}

			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containerEntity = context.Containers.AsNoTracking().Where(c => c.Parent == applicationEntity.Id && c.Name == container).FirstOrDefault();

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Container not found.");
				}

				containerEntity.Name = updatedContainer.Name;
				_ = context.SaveChanges();

				return Request.CreateResponse(HttpStatusCode.OK, containerEntity);
			}
		}

		[HttpDelete, Route("api/somiod/{application}/containers/{container}")]
		public HttpResponseMessage DeleteContainer(string application, string container)
		{
			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containerEntity = context.Containers.Where(c => c.Parent == applicationEntity.Id && c.Name == container).FirstOrDefault();

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Container not found.");
				}

				_ = context.Containers.Remove(containerEntity);
				_ = context.SaveChanges();

				return Request.CreateResponse(HttpStatusCode.OK);
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
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containerEntity = context.Containers.AsNoTracking().Where(c => c.Parent == applicationEntity.Id && c.Name == container).FirstOrDefault();

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Container not found.");
				}

				var subscriptions = context.Subscriptions.AsNoTracking().Where(s => s.Parent == containerEntity.Id).ToList();
				return Request.CreateResponse(HttpStatusCode.OK, subscriptions);
			}
		}

		[HttpGet, Route("api/somiod/{application}/{container}/subscriptions/{subscription}")]
		public HttpResponseMessage GetSubscription(string application, string container, string subscription)
		{
			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containerEntity = context.Containers.AsNoTracking().Where(c => c.Parent == applicationEntity.Id && c.Name == container).FirstOrDefault();

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Container not found.");
				}

				var subscriptionEntity = context.Subscriptions.AsNoTracking().Where(s => s.Parent == containerEntity.Id && s.Name == subscription).FirstOrDefault();

				return subscriptionEntity == null
					? Request.CreateErrorResponse(HttpStatusCode.NotFound, "Subscription not found.")
					: Request.CreateResponse(HttpStatusCode.OK, subscriptionEntity);
			}
		}

		[HttpPost, Route("api/somiod/{application}/{container}/subscriptions")]
		public HttpResponseMessage PostSubscription(string application, string container, [FromBody] Subscription subscription)
		{
			if (subscription == null)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Body is empty.");
			}

			var validator = new SubscriptionValidator();
			var validationResult = validator.Validate(subscription);

			if (!validationResult.IsValid)
			{
				return Request.CreateErrorResponse((HttpStatusCode)422, validationResult.ErrorMessages.First().Message);
			}

			using (var context = new SomiodDbContext())
			{

				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containerEntity = context.Containers.AsNoTracking().Where(c => c.Parent == applicationEntity.Id && c.Name == container).FirstOrDefault();

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Container not found.");
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

				return Request.CreateResponse(HttpStatusCode.Created, subscription);
			}
		}

		[HttpDelete, Route("api/somiod/{application}/{container}/subscriptions/{subscription}")]
		public HttpResponseMessage DeleteSubscription(string application, string container, string subscription)
		{
			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containerEntity = context.Containers.AsNoTracking().Where(c => c.Parent == applicationEntity.Id && c.Name == container).FirstOrDefault();

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Container not found.");
				}

				var storedEntity = context.Subscriptions.Where(s => s.Name == subscription).FirstOrDefault();

				if (storedEntity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Subscription not found.");
				}

				_ = context.Subscriptions.Remove(storedEntity);
				_ = context.SaveChanges();

				return Request.CreateResponse(HttpStatusCode.OK);
			}
		}

		[HttpGet, Route("api/somiod/{application}/{container}/data")]
		public HttpResponseMessage GetData(string application, string container)
		{
			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containerEntity = context.Containers.AsNoTracking().Where(c => c.Parent == applicationEntity.Id && c.Name == container).FirstOrDefault();

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Container not found.");
				}

				var data = context.Data.AsNoTracking().Where(d => d.Parent == containerEntity.Id).ToList();
				return Request.CreateResponse(HttpStatusCode.OK, data);
			}
		}

		[HttpGet, Route("api/somiod/{application}/{container}/data/{data}")]
		public HttpResponseMessage GetData(string application, string container, string data)
		{
			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containerEntity = context.Containers.AsNoTracking().Where(c => c.Parent == applicationEntity.Id && c.Name == container).FirstOrDefault();

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Container not found.");
				}

				var dataEntity = context.Data.AsNoTracking().Where(d => d.Parent == containerEntity.Id && d.Name == data).FirstOrDefault();

				return dataEntity == null
					? Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data not found.")
					: Request.CreateResponse(HttpStatusCode.OK, dataEntity);
			}
		}

		[HttpPost, Route("api/somiod/{application}/{container}/data")]
		public HttpResponseMessage PostData(string application, string container, [FromBody] Data data)
		{
			if (data == null)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Body is empty.");
			}

			var validator = new DataValidator();
			var validationResult = validator.Validate(data);

			if (!validationResult.IsValid)
			{
				return Request.CreateErrorResponse((HttpStatusCode)422, validationResult.ErrorMessages.First().Message);
			}

			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containerEntity = context.Containers.AsNoTracking().Where(c => c.Parent == applicationEntity.Id && c.Name == container).FirstOrDefault();

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Container not found.");
				}

				var uniqueName = data.Name.ToLowerInvariant();

				if (context.Data.Any(a => a.Name == uniqueName && a.Parent == containerEntity.Id))
				{
					uniqueName = $"{uniqueName}-{Guid.NewGuid()}";
				}

				data.Name = uniqueName;
				data.Parent = containerEntity.Id;
				data.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				_ = context.Data.Add(data);
				_ = context.SaveChanges();

				SendNotificationToSubscriptions(context, containerEntity.Id, containerEntity.Name, data.Content, "1"); //1 = creation

				return Request.CreateResponse(HttpStatusCode.Created, data);
			}
		}

		[HttpDelete, Route("api/somiod/{application}/{container}/data/{data}")]
		public HttpResponseMessage DeleteData(string application, string container, string data)
		{
			using (var context = new SomiodDbContext())
			{
				var queryable = context.Applications.AsNoTracking().Where(a => a.Name == application).AsQueryable();

				if (!queryable.Any())
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Application not found.");
				}

				var applicationEntity = queryable.First();
				var containerEntity = context.Containers.AsNoTracking().Where(c => c.Parent == applicationEntity.Id && c.Name == container).FirstOrDefault();

				if (containerEntity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Container not found.");
				}

				var storedEntity = context.Data.Where(d => d.Name == data).FirstOrDefault();

				if (storedEntity == null)
				{
					return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data not found.");
				}

				_ = context.Data.Remove(storedEntity);
				_ = context.SaveChanges();

				SendNotificationToSubscriptions(context, containerEntity.Id, containerEntity.Name, storedEntity.Content, "2"); //2 = deletion

				return Request.CreateResponse(HttpStatusCode.OK);
			}
		}

		private void SendNotificationToSubscriptions(SomiodDbContext context, int containerId, string topic, string content, string eventType)
		{
			var subscriptions = context.Subscriptions
				.AsNoTracking()
				.Where(s => s.Parent == containerId)
				.Where(s => s.Event == eventType || s.Event == "12") //eventType = 1 (creation) or 2 (deletion) or 12 (both)
				.ToList();

			if (!subscriptions.Any())
			{
				return;
			}

			var notification = new Notification
			{
				Content = content,
				EventType = eventType
			};

			foreach (var subscription in subscriptions)
			{
				try
				{
					if (subscription.Endpoint.StartsWith("http"))
					{
						RestSharpService.FireNotification(subscription.Endpoint, topic, notification);
					}

					if (subscription.Endpoint.StartsWith("mqtt"))
					{
						MqttService.FireNotification(subscription.Endpoint, topic, notification);
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
					continue;
				}
			}
		}
	}
}
