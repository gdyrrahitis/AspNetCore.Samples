namespace AspNetCoreMvc.Redis
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using StackExchange.Redis;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>((f) =>
            {
                return ConnectionMultiplexer.Connect("localhost");
            });
            services.AddScoped<IDatabase>((f) => {
                var connectionMultiplexerService = f.GetService<IConnectionMultiplexer>();
                return connectionMultiplexerService.GetDatabase(0);
            });

            // Adds MVC to the pipeline
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute("Default", "{controller}/{action}/{id:guid?}", new { controller = "Home", action = "Index" });
            });
        }
    }
}
