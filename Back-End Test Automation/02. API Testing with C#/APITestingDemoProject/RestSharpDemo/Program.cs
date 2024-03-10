using RestSharp;
using RestSharp.Authenticators;
using RestSharpDemo;
using System.Text.Json.Serialization;

class Program
{
	private static readonly object resp;

	public static object JsonSerializer { get; private set; }

	static void Main(string[] args)
	{
		var client = new RestClient(new RestClientOptions("https://api.github.com")
		{
			Authenticator = new HttpBasicAuthenticator("IvanKrIvanov", "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")
		});

		var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues", Method.Post);

		request.AddHeader("Content-Type", "application/json");
		request.AddJsonBody(new { title = "TestingIssue3/6", body = "IvanKrIvanov" });
		var response = client.Execute(request);
		Console.WriteLine(response.StatusCode);
	}
}