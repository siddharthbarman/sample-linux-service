using SampleLinuxService;
using System.Runtime.Loader;

public class Program
{
	public static void Main(string[] args)
	{
		try
		{			
			IHost host = CreateHostBuilder(args).Build();
			host.Run();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
		}
	}

	private static IHostBuilder CreateHostBuilder(string[] args)
	{
		return Host.CreateDefaultBuilder(args)
			.UseSystemd()
			.ConfigureServices(services =>
			{
				services.AddHostedService<MyWorker>();
			});
	}
}


