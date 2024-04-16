using IdeaAPItest.Models_DTO_;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;

namespace IdeaAPItest
{
	public class IdeaAPITest
	{
		private RestClient client;
		private const string BASEURL = "http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:84";
		private const string EMAIL = "ivanov.ivan.kr@gmail.com";
		private const string PASSWORD = "123456";
		private static string lastIdeaId;

		[OneTimeSetUp]
		public void Setup()
		{
			string jwtToken = GetJwtToken(EMAIL, PASSWORD);

			var options = new RestClientOptions(BASEURL)
			{
				Authenticator = new JwtAuthenticator(jwtToken)
			};
			client = new RestClient(options);
		}

		private string GetJwtToken(string email, string password)
		{
			RestClient authClient = new RestClient(BASEURL);
			var request = new RestRequest("api/User/Authentication");
			request.AddJsonBody(new { email, password });
			
			var response = authClient.Post(request);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				var content = JsonSerializer.Deserialize<JsonElement>(response.Content);
				var token = content.GetProperty("accessToken").GetString();
				if (string.IsNullOrWhiteSpace(token))
				{
					throw new InvalidOperationException("The JWT token is null or empty.");
				}
				return token;
			}
			else
			{
				throw new InvalidOperationException($"Authentication failed: {response.StatusCode}, {response.Content}");
			}

		}

		[Test, Order(1)]
		public void CreateNewIdea_WithCorrectData_ShouldSucceed()
		{
			var requestData = new IdeaDto()
			{
				Title = "TestTitel",
				Description = "TestDescription",
			};
			var request = new RestRequest("api/Idea/Create");
			request.AddJsonBody(requestData);
			var response = client.Execute(request, Method.Post);
			var responseData = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
			Assert.That(responseData.Msg, Is.EqualTo("Successfully created!")); 
		}

		[Test, Order(2)]
		public void GetAllIdeas_ShouldReturnNonEmptyArray()
		{

			var request = new RestRequest("api/Idea/All");

			var response = client.Execute(request, Method.Get);
			var responseData = JsonSerializer.Deserialize<ApiResponseDTO[]>(response.Content);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
			Assert.That(responseData.Length, Is.GreaterThan(0));

			lastIdeaId = responseData[responseData.Length - 1].IdeaId;
		}


		[Test, Order(3)]
		public void EditedIdea_WithCorrectData_ShouldSucce()
		{
			var requestData = new IdeaDto()
			{
				Title = "EditedTestTitel",
				Description = "TestDescription with edit",
			};
			var request = new RestRequest("api/Idea/Edit");
			request.AddQueryParameter("ideaId", lastIdeaId);

			request.AddJsonBody(requestData);
			var response = client.Execute(request, Method.Put);
			var responseData = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
			Assert.That(responseData.Msg, Is.EqualTo("Edited successfully"));
		}

		[Test, Order(4)]
		public void DeleteIdea_ShouldSucce()
		{

			var request = new RestRequest("api/Idea/Delete");
			request.AddQueryParameter("ideaId", lastIdeaId);

			var response = client.Execute(request, Method.Delete);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
			Assert.That(response.Content, Does.Contain("The idea is deleted!"));
		}


		[Test, Order(5)]
		public void CreateIdea_WithoutRequiredFields_ShouldReturnBadRequest()
		{
			var requestData = new IdeaDto()
			{
				Title = "TestTitel",
			};
			var request = new RestRequest("api/Idea/Create");
			request.AddJsonBody(requestData);
			var response = client.Execute(request, Method.Post);
			var responseData = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}

		[Test, Order(6)]
		public void EditNonExistingIdea_ShouldReturnNotFound()
		{
			var requestData = new IdeaDto()
			{
				Title = "EditedTestTitel",
				Description = "TestDescription with edit",
			};
			var request = new RestRequest("api/Idea/Edit");
			request.AddQueryParameter("ideaId", "1236546658");

			request.AddJsonBody(requestData);
			var response = client.Execute(request, Method.Put);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
			Assert.That(response.Content, Does.Contain("There is no such idea!"));
		}

		[Test, Order(7)]
		public void DeleteNonExistingIdea_ShouldReturnNotFound()
		{

			var request = new RestRequest("api/Idea/Delete");
			request.AddQueryParameter("ideaId", "249841548");

			var response = client.Execute(request, Method.Delete);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
			Assert.That(response.Content, Does.Contain("There is no such idea!"));
		}
	}
}