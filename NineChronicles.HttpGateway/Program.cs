namespace NineChronicles.HttpGateway
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseConfiguration(CreateConfiguration(args))
                        .UseStartup<Startup>();
                });

        private static IConfiguration CreateConfiguration(string[] args) =>
            new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
    }
}
