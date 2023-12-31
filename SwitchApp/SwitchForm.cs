using SwitchApp.Properties;
using System;
using System.Net;
using RestSharp;
//using System.Reflection;
using System.Windows.Forms;
using SwitchApp.Models;
using System.Text.Json;

namespace SwitchApp
{
    public partial class SwitchForm : Form
    {
        private static readonly string _apiBaseUri = Settings.Default.ApiBaseUri;
        private static readonly string _appName = Settings.Default.AppName;
        private static readonly string _containerToSendData = Settings.Default.ContainerToSendData;
        private static readonly string _containerName = Settings.Default.ContainerName;
        private static readonly HttpStatusCode _apiErrorManual = (HttpStatusCode)Settings.Default.ApiErrorManual;


        private readonly RestClient _restClient = new RestClient(_apiBaseUri);
        public SwitchForm()
        {
            InitializeComponent();
        }




		private void ButtonOff_Click(object sender, System.EventArgs e)
		{
			CreateData(_appName, _containerToSendData, "OFF");
		}

		private void ButtonOn_Click(object sender, System.EventArgs e)
		{
			CreateData(_appName, _containerToSendData, "ON");
		}

		private void FormSwitch_Shown(object sender, EventArgs e)
		{
			CreateContainer(_containerName, _appName);
		}




		/////////////////API///////////////////////
		private void CreateData(string appName, string moduleToSendData, string content)
		{
			var mod = new Data(content);

			var request = new RestRequest($"api/somiod/{appName}/{moduleToSendData}/data", Method.Post);
			request.AddObject(mod);

			var response = _restClient.Execute(request);

			if (response.StatusCode == 0)
			{
				MessageBox.Show("It wasn't possible to connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (response.StatusCode != HttpStatusCode.OK)
				MessageBox.Show("Error creating data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void CreateContainer(string containerName, string appName)
		{
			var mod = new Container(containerName, appName);

			var request = new RestRequest($"api/somiod/{appName}", Method.Post);
			request.AddObject(mod);

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
        private bool EntityExistant(RestResponse response)
        {
            if (response.StatusCode == _apiErrorManual)
            {
                var error = JsonSerializer.Deserialize<Error>(response.Content ?? string.Empty);

                if (error.Message.Contains("already exists"))
                    return true;
            }
            return false;
        }



    }
}
