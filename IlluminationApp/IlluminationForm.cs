using IlluminationApp.Models;
using IlluminationApp.Properties;
using RestSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
//using System.Reflection;

namespace IlluminationApp
{
	public partial class IlluminationForm : Form
	{

		private static readonly string _brokerIp = Settings.Default.BrokerIp;
		private static readonly string[] _topic = { Settings.Default.Topic };
		private static readonly string _appName = Settings.Default.AppName;
		private static readonly string _apiBaseUri = Settings.Default.ApiBaseUri;
		private static readonly string _containerName = Settings.Default.ContainerName;

		private static readonly string _subscriptionName = Settings.Default.SubscriptionName;
		private static readonly string _eventType = Settings.Default.EventType;
		private static readonly string _endpoint = Settings.Default.Endpoint;

		private MqttClient _mosqClient;
		private bool _illuminationState;
		private readonly RestClient _restClient = new RestClient(_apiBaseUri);

		public IlluminationForm()
		{
			InitializeComponent();
		}

		private void IlluminationForm_Load(object sender, EventArgs e)
		{
			ConnectToBroker();
			SubscribeToTopics();
			CreateApp(_appName);
			CreateContainer(_containerName, _appName);
			CreateSubscription(_subscriptionName, _containerName, _appName, _eventType, _endpoint);
		}

		private void ConnectToBroker()
		{
			_mosqClient = new MqttClient(_brokerIp);
			_ = _mosqClient.Connect(Guid.NewGuid().ToString());

			if (!_mosqClient.IsConnected)
			{
				_ = MessageBox.Show("It wasn't possible to connect to message broker", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}
		}

		private void SubscribeToTopics()
		{
			_mosqClient.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
			byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE };
			_ = _mosqClient.Subscribe(_topic, qosLevels);
		}
		private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs args)
		{
			var messageString = Encoding.UTF8.GetString(args.Message);
			using (TextReader reader = new StringReader(messageString))
			{
				var notification = (Notification)new XmlSerializer(typeof(Notification)).Deserialize(reader);

				_illuminationState = notification.Data.Content == "on";

				if (_illuminationState)
				{
					_illuminationState = true;
					pictureBox1.Image = Resources.lampson;
					return;
				}

				_illuminationState = false;
				pictureBox1.Image = Resources.lampsoff;
			}
		}

		/////////////////API///////////////////////

		private void CreateApp(string appName)
		{
			var app = new Models.Application(appName);

			var request = new RestRequest("api/somiod", Method.Post);
			_ = request.AddObject(app);

			var response = _restClient.Execute(request);

			if (response.StatusCode == 0)
			{
				_ = MessageBox.Show("It wasn't possible to connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (response.StatusCode == HttpStatusCode.Conflict)
			{
				Debug.WriteLine("Application already exists");
				return;
			}

			if (response.StatusCode != HttpStatusCode.Created)
			{
				_ = MessageBox.Show("Error creating the application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void CreateContainer(string containerName, string appName)
		{
			var container = new Container(containerName);

			var request = new RestRequest($"api/somiod/{appName}", Method.Post);
			_ = request.AddObject(container);

			var response = _restClient.Execute(request);

			if (response.StatusCode == 0)
			{
				_ = MessageBox.Show("It wasn't possible to connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (response.StatusCode == HttpStatusCode.Conflict)
			{
				Debug.WriteLine("Container already exists");
				return;
			}

			if (response.StatusCode != HttpStatusCode.Created)
			{
				_ = MessageBox.Show("Error creating container", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void CreateSubscription(string subscriptionName, string containerName, string appName, string eventType, string endpoint)
		{
			var sub = new Subscription(subscriptionName, eventType, endpoint);

			var request = new RestRequest($"api/somiod/{appName}/{containerName}/subscriptions", Method.Post);
			_ = request.AddObject(sub);

			var response = _restClient.Execute(request);

			if (response.StatusCode == 0)
			{
				_ = MessageBox.Show("It wasn't possible to connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (response.StatusCode != HttpStatusCode.Created)
			{
				_ = MessageBox.Show("Error creating subscription", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
