namespace SampleLinuxService
{
	public abstract class GracefulShutdownWorker : BackgroundService
	{
		protected readonly ILogger<GracefulShutdownWorker> _logger;
		protected volatile bool _stop = false;
		private volatile bool _stopped = false;

		public GracefulShutdownWorker(ILogger<GracefulShutdownWorker> logger)
		{
			_logger = logger;
		}

		public override async Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogCritical("Setting stop signal");
			_stop = true;
			while (_stopped != true)
			{
				_logger.LogCritical("Waiting for cleanup to complete");
				Thread.Sleep(_cleanupIntervalMS);				
			}
			_logger.LogCritical("Cleapup complete detected");
			await base.StopAsync(cancellationToken);
			return;
		}

		protected bool RequireStop
		{
			get { return _stop; }
		}

		protected bool SafeToShutdown
		{
			get { return _stopped; }
			set { _stopped = value; }
		}

		protected int _cleanupIntervalMS = 1000;
	}
}
