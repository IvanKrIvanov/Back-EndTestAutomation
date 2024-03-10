using RestSharp.Authenticators;
using RestSharp;
using System.Net;
using Newtonsoft.Json;

namespace RestSharpTest
{
	public class Tests
	{
		RestClient client;

		[SetUp]
		public void Setup()
		{
			var options = new RestClientOptions("https://api.github.com")
			{
				Authenticator = new HttpBasicAuthenticator("IvanKrIvanov", "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")
			};
			client = new RestClient(options);
		
		}

		[Test]
		public void Test_GitIssuesEndPoint()
		{
			// Arrange
			var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues", Method.Get);

			// Act
			var response = client.Execute(request);

			// Assert
			Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
		}

		[Test]
		public void Test_GitIssuesEndPoint_MoreValidation()
		{
			// Arrange
			var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues", Method.Get);

			// Act
			var response = client.Execute(request);
			var issuesObjects = JsonConvert.DeserializeObject<List<Issue>>(response.Content);

			// Assert
			Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
		}

		[Test]
		public void Test_GitPostMethod()
		{	// Arrange, Act 
			var createdIssue = CreateIssue("IssueTest123", "BodyTest123");

			// Assert
			Assert.That(createdIssue.title.Equals("IssueTest123"));
			Assert.That(createdIssue.body.Equals("BodyTest123"));
		}
		private Issue CreateIssue(string title, string body)
		{
			
			var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues", Method.Get);

			request.AddBody(new {body, title });
			var response = client.Execute(request);
			var issuesObject = JsonConvert.DeserializeObject<Issue>(response.Content);

			return issuesObject;
		}


	}
}