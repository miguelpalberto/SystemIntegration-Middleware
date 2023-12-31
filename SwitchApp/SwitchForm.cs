using RestSharp;
using SwitchApp.Models;
using SwitchApp.Properties;
using System;
using System.Net;
//using System.Reflection;
using System.Windows.Forms;

namespace SwitchApp
{
	public partial class SwitchForm : Form
	{
		private static readonly string _ownAppName = Settings.Default.OwnAppName;
		private static readonly string _apiBaseUri = Settings.Default.ApiBaseUri;
		private static readonly string _appName = Settings.Default.AppName;
		private static readonly string _containerToSendData = Settings.Default.ContainerToSendData;

		private readonly RestClient _restClient = new RestClient(_apiBaseUri);
		public SwitchForm()
		{
			InitializeComponent();
		}

		private void ButtonOff_Click(object sender, System.EventArgs e)
		{
			CreateData(_appName, _containerToSendData, "off");
		}

		private void ButtonOn_Click(object sender, System.EventArgs e)
		{
			CreateData(_appName, _containerToSendData, "on");
		}

		private void FormSwitch_Shown(object sender, EventArgs e)
		{
			CreateApplication(_ownAppName);
		}

		/////////////////API///////////////////////
		private void CreateData(string appName, string container, string content)
		{
			var data = new Data("illumination_data", content);

			var request = new RestRequest($"api/somiod/{appName}/{container}/data", Method.Post);
			_ = request.AddObject(data);

			var response = _restClient.Execute(request);

			if (response.StatusCode == 0)
			{
				_ = MessageBox.Show("It wasn't possible to connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (response.StatusCode != HttpStatusCode.Created)
			{
				_ = MessageBox.Show("Error creating data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void CreateApplication(string applicationName)
		{
			var application = new Models.Application(applicationName);

			var request = new RestRequest($"api/somiod", Method.Post);
			_ = request.AddObject(application);

			var response = _restClient.Execute(request);

			if (response.StatusCode == 0)
			{
				_ = MessageBox.Show("It wasn't possible to connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (response.StatusCode != HttpStatusCode.Created)
			{
				_ = MessageBox.Show("Error creating appication", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
