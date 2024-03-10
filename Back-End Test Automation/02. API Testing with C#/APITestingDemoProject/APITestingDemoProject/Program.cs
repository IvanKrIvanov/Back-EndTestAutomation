using APITestingDemoProject;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

class Program
{
	static void Main(string[] args)
	{
		//WeatherForecast weatherForecast = new WeatherForecast()
		//{
		//	Date = DateTime.Now,
		//	TemperatureC = 32,
		//	Summary = "New Random Test Summary"
		//};
		//string weatherForecastJosn = JsonConvert.SerializeObject(weatherForecast, Formatting.Indented);

		//Console.WriteLine(weatherForecastJosn);

		//string jsonString = File.ReadAllText(Path.Combine(Environment.CurrentDirectory) + "/../../../People.json");

		//var person = new
		//{
		//	FirstName = string.Empty,
		//	LastName = string.Empty,
		//	JobTitle = string.Empty,
		//};

		//var weatherForcastObject = JsonConvert.DeserializeAnonymousType(jsonString, person);

		//string jsonString = File.ReadAllText(Path.Combine(Environment.CurrentDirectory) + "/../../../weatherForecast.json");

		//var weatherForcastObject = JsonConvert.DeserializeObject<List<WeatherForecast>>(jsonString);

		//WeatherForecast weatherForecast = new WeatherForecast()
		//{
		//	Date = DateTime.Now,
		//	TemperatureC = 32,
		//	Summary = "New Random Test Summary"
		//};

		//DefaultContractResolver contractResolver =
		//new DefaultContractResolver()
		//	{
		//		NamingStrategy = new SnakeCaseNamingStrategy()
		//	};
		//var serialized = JsonConvert.SerializeObject(weatherForecast,
		//	new JsonSerializerSettings()
		//	{
		//		ContractResolver = contractResolver,
		//		Formatting = Formatting.Indented
		//	});

		//string jsonString = File.ReadAllText(Path.Combine(Environment.CurrentDirectory) + "/../../../People.json");

		//var person = JObject.Parse(jsonString);
		//Console.WriteLine(person["firstName"]);
		//Console.WriteLine(person["lastName"]);

		var json = JObject.Parse(@"{'products': [
		{'name': 'Fruits', 'products': ['apple', 'banana']},
		{'name': 'Vegetables', 'products': ['cucumber']}]}");
		var products = json["products"].Select(t =>
		string.Format("{0} ({1})",
		t["name"],
		string.Join(", ", t["products"])
		));
	}
}

