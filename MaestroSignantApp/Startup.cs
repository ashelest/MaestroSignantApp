using System.Text.Json.Serialization;
using MaestroSignant.Api.Services;
using MaestroSignant.Api.SignalR;
using MaestroSignant.Application;
using MaestroSignant.Infrastructure;
using Newtonsoft.Json;

namespace MaestroSignant.Api;

public class Startup
{
    private readonly IConfiguration configuration;

    private const string ApiCorsPolicy = nameof(ApiCorsPolicy);

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddApplication(this.configuration);
        services.AddInfrastructure(this.configuration);

        services.AddHostedService<PostingsSyncBackgroundService>();

        services
            .AddMvc()
            .AddJsonOptions(options =>
            {
                var enumConverter = new JsonStringEnumConverter();
                options.JsonSerializerOptions.Converters.Add(enumConverter);
            });

        services.AddSingleton(_ => new JsonSerializer
        {
            NullValueHandling = NullValueHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ",
        });

        services.AddSwaggerGen();
        services.AddSwaggerDocument(config =>
        {
            config.PostProcess = document =>
            {
                document.Info.Version = "v1";
                document.Info.Title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            };
        });

        services.AddSignalR();

        ConfigureCors(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseOpenApi();
        app.UseSwaggerUi3();

        app.UseCors(ApiCorsPolicy);

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<SignantHub>("/SignantHub");
        });
    }

    private void ConfigureCors(IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy(ApiCorsPolicy, builder =>
        {
            builder
                .WithOrigins("https://localhost:5002")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("Content-Disposition");
        }));
    }
}