using System;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using IlluminationApp.Properties;
using IlluminationApp.Models;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Net;
using RestSharp;
using System.Text.Json;
//using System.Reflection;


namespace IlluminationApp
{
    public partial class IlluminationForm : Form
    {

        private static readonly string _brokerIp = Settings.Default.BrokerIp;
        private static readonly string[] _topic = { Settings.Default.Topic };
        private static readonly string _appName = Settings.Default.AppName;
        private static readonly string _apiBaseUri = Settings.Default.ApiBaseUri;
        private static readonly HttpStatusCode _apiErrorManual = (HttpStatusCode)Settings.Default.ApiErrorManual;
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

        private void IlluminationForm_Load(object sender, System.EventArgs e)
        {
            ConnectToBroker();
            SubscribeToTopics();
            CreateApp(_appName);
            CreateContainer(_containerName, _appName);
            CreateSubscription(_subscriptionName, _containerName, _appName, _eventType, _endpoint);
			CreateSubscriptionHttp(_subscriptionName, _containerName, _appName, _eventType, _endpoint);
		}




        /////////////////BROKER///////////////////////
        private void ConnectToBroker()
        {
            _mosqClient = new MqttClient(_brokerIp);
            _mosqClient.Connect(Guid.NewGuid().ToString());

            if (!_mosqClient.IsConnected)
            {
                MessageBox.Show("It wasn't possible to connect to message broker", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void SubscribeToTopics()
        {
            _mosqClient.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE };
            _mosqClient.Subscribe(_topic, qosLevels);
        }
        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs args)
        {
            string messageString = Encoding.UTF8.GetString(args.Message);
            using (TextReader reader = new StringReader(messageString))
            {
                var notification = (Notification)new XmlSerializer(typeof(Notification)).Deserialize(reader);
                if (notification.EventType != "CREATE") return;

                _illuminationState = notification.Content == "ON";

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

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            //nada
        }



        /////////////////API///////////////////////

        private void CreateApp(string appName)
        {
            var app = new App(appName);

            var request = new RestRequest("api/somiodwebservice", Method.Post);
            request.AddObject(app);

            var response = _restClient.Execute(request);

            if (EntityExistant(response))
                return;

            if (response.StatusCode == 0)
            {
                MessageBox.Show("It wasn't possible to connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("Error creating the application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private bool EntityExistant(RestResponse response)
        {
            if (response.StatusCode == _apiErrorManual) {
                var error = JsonSerializer.Deserialize<Error>(response.Content ?? string.Empty);

                if (error.Message.Contains("already exists"))
                return true;
        }
            return false;
        }

        private void CreateContainer(string containerName, string appName)
        {
            var container = new Container(containerName, appName);

            var request = new RestRequest($"api/somiodwebservice/{appName}", Method.Post);
            request.AddObject(container);

            var response = _restClient.Execute(request);

            if (EntityExistant(response))
                return;

            if (response.StatusCode == 0)
            {
                MessageBox.Show("It wasn't possible to connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("Error creating container", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void CreateSubscription(string subscriptionName, string containerName, string appName, string eventType, string endpoint)
        {
            var sub = new Subscription(subscriptionName, containerName, eventType, endpoint);

            var request = new RestRequest($"api/somiodwebservice/{appName}/{containerName}/subscriptions", Method.Post);
            request.AddObject(sub);

            var response = _restClient.Execute(request);

            if (EntityExistant(response))
                return;

            if (response.StatusCode == 0)
            {
                MessageBox.Show("It wasn't possible to connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("Error creating subscription", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
		private void CreateSubscriptionHttp(string subscriptionName, string containerName, string appName, string eventType, string endpoint)
		{
			var sub = new Subscription(subscriptionName, containerName, eventType, endpoint);

			var request = new RestRequest($"api/somiodwebservice/{appName}/{containerName}/subscriptionshttp", Method.Post);
			request.AddObject(sub);

			var response = _restClient.Execute(request);

			if (EntityExistant(response))
				return;

			if (response.StatusCode == 0)
			{
				MessageBox.Show("It wasn't possible to connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (response.StatusCode != HttpStatusCode.OK)
				MessageBox.Show("Error creating subscription", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
