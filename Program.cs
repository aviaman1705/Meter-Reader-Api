using MeterReaderAPI;
using MeterReaderAPI.Logging;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging((hostBuilderContext, logging) =>
            {
                logging.AddDbLogger(options =>
                {
                    hostBuilderContext.Configuration.GetSection("Logging").GetSection("Database").GetSection("Options").Bind(options);
                });
            });
}