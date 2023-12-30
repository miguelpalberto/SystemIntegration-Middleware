using System;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using IlluminationApp.Properties;
using IlluminationApp.Models;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.IO;
using System.Text;
using System.Xml.Serialization;


namespace IlluminationApp
{
    public partial class IlluminationForm : Form
    {

        private static readonly string BrokerIp = Settings.Default.BrokerIp;
        private static readonly string[] Topic = { Settings.Default.Topic };

        private MqttClient _mosqClient;
        private bool _illuminationState;


        public IlluminationForm()
        {
            InitializeComponent();
        }

        private void IlluminationForm_Load(object sender, System.EventArgs e)
        {
            ConnectToBroker();
            SubscribeToTopics();
        }


        private void ConnectToBroker()
        {
            _mosqClient = new MqttClient(BrokerIp);
            _mosqClient.Connect(Guid.NewGuid().ToString());

            if (!_mosqClient.IsConnected)
            {
                MessageBox.Show("It wasn't possible to connect to message broker", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void SubscribeToTopics()
        {
            _mosqClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE };
            _mosqClient.Subscribe(Topic, qosLevels);
        }
        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs args)
        {
            string messageString = Encoding.UTF8.GetString(args.Message);
            using (TextReader reader = new StringReader(messageString))
            {
                var notification = (Notification)new XmlSerializer(typeof(Notification)).Deserialize(reader);
                if (notification.EventType != "CREATE") return;

                _illuminationState = notification.Content == "ON";
                UpdateIllumination();
            }
        }
        private void UpdateIllumination()
        {
            if (_illuminationState)
            {
                _illuminationState = true;
                pictureBox1.Image = Resources.IlluminationOn;
                return;
            }

            _illuminationState = false;
            pictureBox1.Image = Resources.IlluminationOff;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //nada
        }
    }
}
