using App.Services;

namespace App
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add controllers
            services.AddControllers();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
            });

            // Add Swagger/OpenAPI
            //services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen();

            // Add PokemonService as a scoped service
            services.AddScoped<IPokemonService, PokemonService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure middleware for serving Angular files
            app.UseDefaultFiles(); // Serves default files like index.html
            app.UseStaticFiles(); // Allows serving Angular static files (CSS, JS, etc.)

            // Configure middleware for the HTTP request pipeline
            //if (env.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // Add fallback to serve index.html for Angular routes
                endpoints.MapFallbackToFile("/index.html");
            });
        }
    }
}
