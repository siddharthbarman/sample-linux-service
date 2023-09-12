namespace SampleLinuxService
{
	public class MyWorker : GracefulShutdownWorker
	{
		public MyWorker(ILogger<MyWorker> logger) : base(logger)
		{		
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{			
			bool stop = false;
			while (!stop)
			{
				_logger.LogInformation("ThreadID: {0} I am working at {1}", Thread.CurrentThread.ManagedThreadId, DateTimeOffset.Now);
				await Task.Delay(1000, stoppingToken);
				
				if (RequireStop)
				{
					_logger.LogCritical("ThreadID: {0}, Stop signal has been set, cleaning up...", Thread.CurrentThread.ManagedThreadId);
					Thread.Sleep(5000);
					_logger.LogCritical("ThreadID: {0}, Finished cleaning up", Thread.CurrentThread.ManagedThreadId);
					stop = true;
				}
			}
			SafeToShutdown = true;
		}

		public static int StopIntervalMS = 1000;
	}
}