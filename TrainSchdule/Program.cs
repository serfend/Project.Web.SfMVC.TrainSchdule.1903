using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
namespace TrainSchdule.WEB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
			
		}

		//Kestrel Web Server
		//UseIIS()
		//UseIISIntegration()内网Web应用

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
