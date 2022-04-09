using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;

namespace SampleLog
{
	public class NativeLibrary
	{
		private static Logging? _log;

		[UnmanagedCallersOnly(EntryPoint = "init")]
		public static void Init()
		{
			// Create logging
			_log = new Logging();
		}

		[UnmanagedCallersOnly(EntryPoint = "init_config")]
		public static void InitConfig(IntPtr pathPtr)
		{
			string path = Marshal.PtrToStringAnsi(pathPtr) ?? string.Empty;

			// Initiate the configuration
			IConfigurationRoot config = new ConfigurationBuilder()
				.SetBasePath(path)
				.AddJsonFile("appSettings.json", false)
				.Build();

			// Create logging
			_log = new Logging(config);
		}

		[UnmanagedCallersOnly(EntryPoint = "log_message")]
		public static void LogMessage(int level, IntPtr message)
		{
			// Parse strings from the passed pointers 
			string logMessage = Marshal.PtrToStringAnsi(message) ?? string.Empty;

			if (_log == null)
			{
				Console.WriteLine($"Unable to load Serilog. Message received was: {logMessage}");
				return;
			}
			_log.LogMessage(level, logMessage);
		}
	}
}
