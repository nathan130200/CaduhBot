using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduh
{
	public static class Log
	{
		static string AddInfo(string message)
		{
			return "[" + DateTime.Now.ToString("o") + "] " + message;
		}

		public static void Info(string message)
		{
			Trace.TraceInformation(AddInfo(message));
		}

		public static void Info(string format, params object[] args)
		{
			Info(string.Format(format, args));
		}

		public static void Warn(string message)
		{
			Trace.TraceWarning(AddInfo(message));
		}

		public static void Warn(string format, params object[] args)
		{
			Warn(string.Format(format, args));
		}

		public static void Error(string message)
		{
			Trace.TraceError(AddInfo(message));
		}

		public static void Error(string format, params object[] args)
		{
			Error(string.Format(format, args));
		}
	}
}
