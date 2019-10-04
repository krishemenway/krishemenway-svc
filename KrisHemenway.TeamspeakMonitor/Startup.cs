using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace KrisHemenway.TeamspeakMonitor
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvcCore().AddJsonOptions(FixJsonCamelCasing);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app)
		{
			app.UseRouting();
			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

			new TeamspeakUserMonitor().StartMonitoring();
		}

		private void FixJsonCamelCasing(JsonOptions options)
		{
			// this unsets the default behavior (camelCase); "what you see is what you get" is now default
			options.JsonSerializerOptions.PropertyNamingPolicy = null;
		}
	}
}
