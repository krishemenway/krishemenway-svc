using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KrisHemenway.TeamspeakMonitor
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvcCore().AddJsonFormatters(FixJsonCamelCasing);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime)
		{
			app.UseMvc();
			new TeamspeakUserMonitor().StartMonitoring();
		}

		private void FixJsonCamelCasing(JsonSerializerSettings settings)
		{
			var resolver = settings.ContractResolver;
			if (resolver != null)
			{
				var res = resolver as DefaultContractResolver;
				res.NamingStrategy = null;  // <<!-- this removes the camelcasing
			}
		}
	}
}
