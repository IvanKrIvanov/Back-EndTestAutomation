using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using StorySpoiler.DTO_Models;
using System.Diagnostics.Tracing;
using System.Net;
using System.Text.Json;

namespace StorySpoiler
{
	public class StoryAPITest	
	{
		private const string BASEURL = "https://d5wfqm7y6yb3q.cloudfront.net/api";
		private const string USERNAME = "ivan";
		private const string PASSWORD = "123456";
		private RestClient client;
		private static string storyId;

		[OneTimeSetUp]
		public void Setup()
		{
			string jwtToken = GetJwtToken(USERNAME, PASSWORD);

			var options = new RestClientOptions(BASEURL)
			{
				Authenticator = new JwtAuthenticator(jwtToken),
			};

			this.client = new RestClient(options);

			string GetJwtToken(string username, string password)
			{
				RestClient authClient = new RestClient(BASEURL);

				var authRequest = new RestRequest("/User/Authentication");
				authRequest.AddJsonBody(new { username, password });

				var response = authClient.Post(authRequest);
				if (response.StatusCode == HttpStatusCode.OK)
				{
					var content = JsonSerializer.Deserialize<JsonElement>(response.Content);
					var accessToken = content.GetProperty("accessToken").GetString();
					if (string.IsNullOrWhiteSpace(accessToken))
					{
						throw new InvalidOperationException("The JWT token is null or empty.");
					}
					return accessToken;
				}
				else
				{
					throw new InvalidOperationException($"Authentication failed: {response.StatusCode}, {response.Content}");
				}
			}

		}

		[Order(1)]
		[Test] 
		public void CreatedNewStorySpoiler_WithValidData_ShouldSucceed()
		{
			var requestBody = new StoryDTO
			{
				Title = "Spoiler From VS",
				Description = "This spoiler is created from VS",
			};
			var request = new RestRequest("/Story/Create");
			request.AddJsonBody(requestBody);

			var response = this.client.Execute(request, Method.Post);
			var responseData = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
			Assert.That(responseData.Msg, Is.EqualTo("Successfully created!"));

			

			storyId = responseData.StoryId;
		}

		[Order(2)]
		[Test]
		public void EditAnExistStory_ShouldSucceed()
		{
			var updateData = new StoryDTO()
			{
				Title = "Updated From VS",
				Description = "This spoiler is Updated from VS",
			};
			var request = new RestRequest($"/Story/Edit/{storyId}");

			request.AddJsonBody(updateData);

			var response = this.client.Execute(request, Method.Put);
			var content = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
			Assert.That(content.Msg, Is.EqualTo("Successfully edited"));

		}

		[Order(3)]
		[Test]
		public void DeletedStory_ShouldSucced()
		{
			var request = new RestRequest($"/Story/Delete/{storyId}");

			var response = client.Execute(request, Method.Delete);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
			Assert.That(response.Content, Does.Contain("Deleted successfully!"));

		}


		[Order(4)]
		[Test]
		public void CreatedNewStorySpoiler_WithoutRequiredFields_ShouldReturnBadRequest()
		{
			var requestBody = new StoryDTO
			{
				Title = "Spoiler From VS",
			};
			var request = new RestRequest("/Story/Create");
			request.AddJsonBody(requestBody);

			var response = this.client.Execute(request, Method.Post);
			var responseData = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

		}

		[Order(5)]
		[Test]
		public void EditNonExistingStory_ShouldReturnNotFound()
		{
			var updateData = new StoryDTO()
			{
				Title = "Updated From VS",
				Description = "This spoiler is Updated from VS",
			};
			var request = new RestRequest("/Story/Edit/125336515255");

			request.AddJsonBody(updateData);

			var response = this.client.Execute(request, Method.Put);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
			Assert.That(response.Content, Does.Contain("No spoilers..."));

		}

		[Order(6)]
		[Test]
		public void DeletedStory__WithInvalidId_ShouldFail()
		{
			var request = new RestRequest("/Story/Delete/52516556165165");

			var response = client.Execute(request, Method.Delete);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
			Assert.That(response.Content, Does.Contain("Unable to delete this story spoiler!"));

		}
	}
}