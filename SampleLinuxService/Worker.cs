namespace SampleLinuxService
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private volatile bool _stop = false;
		private volatile bool _stopped = false;

		public Worker(ILogger<Worker> logger)
		{
			_logger = logger;			
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{			
			bool stop = false;
			while (!stop)
			{
				_logger.LogInformation("ThreadID: {0} Worker running at: {1}", Thread.CurrentThread.ManagedThreadId, DateTimeOffset.Now);
				await Task.Delay(1000, stoppingToken);
				_logger.LogInformation("I woke up");
				if (stoppingToken.IsCancellationRequested)
				{
					_logger.LogCritical("Cancellation is requested.");
					Console.WriteLine("Waiting for {0}ms before stopping.", StopIntervalMS);
					_logger.LogCritical("Waiting for {0}ms before stopping.", StopIntervalMS);
					Thread.Sleep(StopIntervalMS);
					stop = true;
				}
				
				if (_stop)
				{
					_logger.LogCritical("ThreadID: {0}, Stop signal has been set, cleaning up...", Thread.CurrentThread.ManagedThreadId);
					Thread.Sleep(5000);
					_logger.LogCritical("ThreadID: {0}, Finished cleaning up", Thread.CurrentThread.ManagedThreadId);
					stop = true;
				}
			}
			_stopped = true;
		}

		public override async Task StopAsync(CancellationToken cancellationToken)
		{			
			_logger.LogCritical("ThreadID: {0}, In StopAsync(), Setting stop signal", Thread.CurrentThread.ManagedThreadId);
			_stop = true;
			while(_stopped != true)
			{
				_logger.LogCritical("ThreadID: {0}, In StopAsync(), waiting for task to cleanup...", Thread.CurrentThread.ManagedThreadId);
				Thread.Sleep(1000);
				//await Task.Delay(1000, cancellationToken);
			}
			_logger.LogCritical("ThreadID: {0}, In StopAsync(), cleapup complete detected", Thread.CurrentThread.ManagedThreadId);
			await base.StopAsync(cancellationToken);
			return;
		}

		public static int StopIntervalMS = 1000;
	}
}