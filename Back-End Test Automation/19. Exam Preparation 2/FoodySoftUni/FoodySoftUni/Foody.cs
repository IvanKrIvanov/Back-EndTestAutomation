using FoodySoftUni.Models;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;

namespace FoodySoftUni
{
	public class Foody
	{

		private RestClient client; 
		private static string foodId; 

		[OneTimeSetUp]
		public void Setup()
		{
			string accessToken = GetAccessToken("ivanov.ivan.kr", "123456");
			var restOptions = new RestClientOptions("http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:86")
			{
				Authenticator = new JwtAuthenticator(accessToken),
			};

			client = new RestClient(restOptions);
		}
		private string GetAccessToken(string username, string password)
		{
			var authClient = new RestClient("http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:86");

			var authRequest = new RestRequest("/api/User/Authentication", Method.Post);

			authRequest.AddJsonBody(new AuthenticationRequest
			{
				UserName = username,
				Password = password
			});

			var response = authClient.Execute(authRequest);

			if (response.IsSuccessStatusCode) 
			{ 
				var content = JsonSerializer.Deserialize<Authentication_response>(response.Content);
				var accessToken = content.AccessToken;
				return accessToken;
			}
			else
			{
				throw new InvalidOperationException("Authentication failed");
			}
		}
			
		[Order(1)]
		[Test]
		public void CreateFood_WithRequiredFields_ShouldSucceed()
		{
			var newFood = new FoodDTO
			{
				Name = "New test food",
				Description = "Description",
				URL = "",
			};
			var request = new RestRequest("/api/Food/Create", Method.Post);
			request.AddJsonBody(newFood);

			var response = client.Execute(request);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

			var data = JsonSerializer.Deserialize<APIResponceDTO>(response.Content);

			foodId = data.FoodId;
		}

		[Order(2)]
		[Test]
		public void EditFood_WithNewTitle_ShouldSucceed()
		{
			var request = new RestRequest($"/api/Food/Edit/{foodId}", Method.Patch);

			request.AddJsonBody(new[]
			{
				new
				{
					path = "/name",
					op = "replace",
					value = "New Food Title",
				},
			});

			var response = this.client.Execute(request);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

			var content = JsonSerializer.Deserialize<APIResponceDTO>(response.Content);

			Assert.That(content.Message, Is.EqualTo("Successfully edited"));
		}

		[Order(3)]
		[Test]
		public void GetAllFood_ShouldReturnAnArrayOfItems()
		{
			var request = new RestRequest("/api/Food/All", Method.Get);

			var response = this.client.Execute(request);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

			var content = JsonSerializer.Deserialize<List<APIResponceDTO>>(response.Content);

			Assert.IsNotEmpty(content);
		}

		[Order(4)]
		[Test]
		public void DeleteFood_WithCorrectId_ShouldBeSuccesful()
		{
			var request = new RestRequest($"/api/Food/Delete/{foodId}", Method.Delete);

			var response = this.client.Execute(request);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

			var content = JsonSerializer.Deserialize<APIResponceDTO>(response.Content);

			Assert.That(content.Message, Is.EqualTo("Deleted successfully!"));
		}

		[Order(5)]
		[Test]
		public void CreateFood_WithIncorrectData_ShouldFail()
		{
			var request = new RestRequest("/api/Food/Create", Method.Post);
			request.AddJsonBody(new { });

			var response = this.client.Execute(request);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}

		[Order(6)]
		[Test]
		public void EditFood_WithNonExisitingId_ShouldFail()
		{
			var request = new RestRequest($"/api/Food/Edit/XXXXXXXXXX", Method.Patch);

			request.AddJsonBody(new[]
			{
				new
				{
					path = "/name",
					op = "replace",
					value = "New Food Title",
				},
			});

			var response = this.client.Execute(request);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

			var content = JsonSerializer.Deserialize<APIResponceDTO>(response.Content);

			Assert.That(content.Message, Is.EqualTo("No food revues..."));
		}

		[Order(7)]
		[Test]
		public void DeleteFood_WithNonExistingId_ShouldFail()
		{
			var request = new RestRequest("/api/Food/Delete/XASDAXAS", Method.Delete);

			var response = this.client.Execute(request);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

			var content = JsonSerializer.Deserialize<APIResponceDTO>(response.Content);

			Assert.That(content.Message, Is.EqualTo("Unable to delete this food revue!"));
		}
	}
}