// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationServer
{
	public class Startup
	{
		public IWebHostEnvironment Environment { get; }

		public Startup(IWebHostEnvironment environment)
		{
			Environment = environment;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			// uncomment, if you want to add an MVC-based UI
			//services.AddControllersWithViews();

			var builder = services.AddIdentityServer()
				.AddInMemoryIdentityResources(Config.Ids)
				.AddInMemoryApiResources(Config.Apis)
				.AddInMemoryClients(Config.Clients);

			// not recommended for production - you need to store your key material somewhere secure
			builder.AddDeveloperSigningCredential();
			//builder.AddSigningCredential(CreateSigningCredentials("This is my secret golden egg."));
		}

		private SigningCredentials CreateSigningCredentials(string clientSecret)
		{
			var byteSecret = Encoding.UTF8.GetBytes(clientSecret);
			//2048 is the minimum size for RsaSecurityKey    
			RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048);
			//Save the public key information to an RSAParameters structure.
			var key = new RsaSecurityKey(RSA.ExportParameters(true));//true  = hasPrivateKey
			var algorithm = SecurityAlgorithms.RsaSha256;
			var signingCredentials = new SigningCredentials(key: key, algorithm: algorithm);
			return signingCredentials;
		}

		public void Configure(IApplicationBuilder app)
		{
			if (Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// uncomment if you want to add MVC
			//app.UseStaticFiles();
			//app.UseRouting();

			app.UseIdentityServer();

			// uncomment, if you want to add MVC
			//app.UseAuthorization();
			//app.UseEndpoints(endpoints =>
			//{
			//    endpoints.MapDefaultControllerRoute();
			//});
		}
	}
}
