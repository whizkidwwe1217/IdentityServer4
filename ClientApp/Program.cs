﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace ClientApp
{
	class Program
	{
		static async Task Main(string[] args)
		{
			// discover endpoints from metadata
			var client = new HttpClient();

			var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
			if (disco.IsError)
			{
				Console.WriteLine(disco.Error);
				return;
			}

			// request token
			var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
			{
				Address = disco.TokenEndpoint,
				ClientId = "client",
				ClientSecret = "secret",

				Scope = "api1"
			});

			if (tokenResponse.IsError)
			{
				Console.WriteLine(tokenResponse.Error);
				return;
			}

			Console.WriteLine(tokenResponse.Json);
			Console.WriteLine("\n\n");

			// call api
			var apiClient = new HttpClient();
			apiClient.SetBearerToken(tokenResponse.AccessToken);

			var response = await apiClient.GetAsync("https://localhost:6000/identity");
			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine($"Error: {response.StatusCode} {response.RequestMessage}");
			}
			else
			{
				var content = await response.Content.ReadAsStringAsync();
				Console.WriteLine(JArray.Parse(content));
			}
		}
	}
}