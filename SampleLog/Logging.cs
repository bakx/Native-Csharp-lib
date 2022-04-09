using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace SampleLog
{
	public class Logging
	{
		private readonly Logger logger;

		public enum LogLevels
		{
			Verbose = 0,
			Debug = 1,
			Informational = 2,
			Warning = 3,
			Error = 4,
			Fatal = 5
		}

		public Logging()
		{
			logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.Console(outputTemplate:
					"[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}")
				.WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
				.CreateLogger();
		}

		public Logging(IConfiguration config)
		{
			logger = new LoggerConfiguration()
				.ReadFrom.Configuration(config)
				.CreateLogger();
		}

		public void LogMessage(int logLevel = 0, string logMessage = "")
		{
			switch ((LogLevels)logLevel)
			{
				case LogLevels.Verbose:
					logger.Verbose(logMessage);
					break;
				case LogLevels.Debug:
					logger.Debug(logMessage);
					break;
				case LogLevels.Informational:
					logger.Information(logMessage);
					break;
				case LogLevels.Warning:
					logger.Warning(logMessage);
					break;
				case LogLevels.Error:
					logger.Error(logMessage);
					break;
				case LogLevels.Fatal:
					logger.Fatal(logMessage);
					break;
				default:
					logger.Verbose(logMessage);
					break;
			}
		}
	}
}
