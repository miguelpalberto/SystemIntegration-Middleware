using RestSharp;
using SwitchApp.Models;
using SwitchApp.Properties;
using System;
using System.Linq;
using System.Net;
//using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

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

		private RestResponse GetData()
		{
			var request = new RestRequest($"api/somiod/{_appName}/{_containerToSendData}/data", Method.Get);

			var response = _restClient.Execute(request);

			if (response.StatusCode == 0)
			{
				_ = MessageBox.Show("It wasn't possible to connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null;
			}

			if (response.StatusCode != HttpStatusCode.OK)
			{
				_ = MessageBox.Show("Error getting data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null;
			}
			return response;

		}

		private RestResponse DeleteDataById(int idSelectedData)
		{
			var request = new RestRequest($"api/somiod/{_appName}/{_containerToSendData}/data/id/{idSelectedData}", Method.Delete);

			var response = _restClient.Execute(request);

			if (response.StatusCode == 0)
			{
				MessageBox.Show("It wasn't possible to connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null;
			}

			if (response.StatusCode != HttpStatusCode.OK)
			{
				MessageBox.Show("Error deleting data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null;
			}
			return response;
		}


		/////////////////FORM///////////////////////

		private void GetAllDataButton_Click(object sender, EventArgs e)
		{
			var response = GetData();
			if (response != null)
			{
				var xmlDoc = XDocument.Parse(response.Content);

				// Select Id and Content values
				var idContentPairs = xmlDoc.Descendants("Data")
					.Select(data => new
					{
						Id = data.Element("Id")?.Value,
						Content = data.Element("Content")?.Value,
						Name = data.Element("Name")?.Value
					})
					.Where(pair => pair.Content == "on" || pair.Content == "off")
					.ToList();

				// Display Id and Content pairs in the listBoxData
				foreach (var pair in idContentPairs)
				{
					listBoxData.Items.Add($"{pair.Id} - {pair.Content} --- ( {pair.Name} )");
				}
			}
		}

		private void GetByIdButton_Click(object sender, EventArgs e)
		{
			var id = idTextBox.Text;

			if (string.IsNullOrEmpty(id))
			{
				MessageBox.Show("Please enter an id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (!int.TryParse(id, out _))
			{
				MessageBox.Show("Please enter a valid numeric id", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			var request = new RestRequest($"api/somiod/{_appName}/{_containerToSendData}/data/id/{id}", Method.Get);

			var response = _restClient.Execute(request);

			if (response.StatusCode == 0)
			{
				MessageBox.Show("It wasn't possible to connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			try
			{
				var xmlDoc = XDocument.Parse(response.Content);

				var idContentPair = xmlDoc.Descendants("Data")
					.Select(data => new
					{
						Id = data.Element("Id")?.Value,
						Content = data.Element("Content")?.Value,
						Name = data.Element("Name")?.Value
					})
					.SingleOrDefault();

				if (idContentPair != null)
				{
					listBoxData.Items.Add($"{idContentPair.Id} - {idContentPair.Content} --- ( {idContentPair.Name} )");
				}
				else
				{
					MessageBox.Show($"No data found for ID {id}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error getting data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}	
			return;

		}

		private void ListBoxData_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void DeleteDataButton_Click(object sender, EventArgs e)
		{
			var selectedData = listBoxData.SelectedItem as string;

			if (string.IsNullOrEmpty(selectedData))
			{
				MessageBox.Show("Please select an item to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Split the selected item to extract the Id
			var idContentParts = selectedData.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

			if (idContentParts.Length < 2)
			{
				MessageBox.Show("Selected item format is invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			var selectedIdString = idContentParts[0];

			if (!int.TryParse(selectedIdString, out int selectedId))
			{
				MessageBox.Show("Invalid Id format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			var response = DeleteDataById(selectedId);
			if (response != null)
			{
				listBoxData.Items.Remove(selectedData);
			}
		}


		private void IdTextBox_TextChanged(object sender, EventArgs e)
		{

		}

	}
}
