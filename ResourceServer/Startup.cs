using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ResourceServer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddAuthentication("JWT")
			.AddJwtBearer("JWT", options =>
			{
				options.Authority = "http://localhost:5000";
				options.RequireHttpsMetadata = false;
				options.Audience = "api1";

				// options.RequireHttpsMetadata = false;
				// options.Authority = Configuration.GetValue<string>("Auth:Authority");
				// options.Audience = Configuration.GetValue<string>("Auth:Audience");

				// var byteSecret = Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Auth:Secret"));
				// var securityKey = new SymmetricSecurityKey(byteSecret);
				// options.TokenValidationParameters = new TokenValidationParameters
				// {
				// 	ValidateIssuer = true,
				// 	ValidateAudience = true,
				// 	ClockSkew = TimeSpan.Zero,
				// 	IssuerSigningKey = securityKey,
				// 	ValidIssuer = Configuration.GetValue<string>("Auth:Issuer"),
				// 	ValidAudience = Configuration.GetValue<string>("Auth:Audience")
				// };

				// options.Events = new JwtBearerEvents
				// {
				// 	OnMessageReceived = context =>
				// 	{
				// 		if (context.Request.Query.ContainsKey("access_token"))
				// 		{
				// 			context.Token = context.Request.Query["access_token"];
				// 		}

				// 		return Task.CompletedTask;
				// 	},
				// 	OnAuthenticationFailed = context =>
				// 	{
				// 		if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
				// 		{
				// 			context.Response.Headers.Add("Token-Expired", "true");
				// 		}
				// 		return Task.CompletedTask;
				// 	}
				// };
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();

			app.UseHttpsRedirection();
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
