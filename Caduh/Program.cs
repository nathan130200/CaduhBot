using Autofac;
using Caduh.Core.Services;
using Caduh.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Caduh
{
	public class Program
	{
		public static IContainer Services {
			get;
			private set;
		}

		public static CancellationTokenSource Cts {
			get;
		} = new CancellationTokenSource();

		static void Main(string[] args)
			=> MainAsync().GetAwaiter().GetResult();

		static async Task MainAsync()
		{
			try
			{
				Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

				Config.Load();

				CaduhBot bot = new CaduhBot();
				bot.SetupInteractivity();
				bot.SetupCommandsNext();
				await bot.ConnectAsync();

				Console.CancelKeyPress += (sender, e) =>
				{
					e.Cancel = true;
					Cts.Cancel();
				};

				while (!Cts.IsCancellationRequested)
					await Task.Delay(100);

				await bot.DisconnectAsync();
			}
			catch(Exception ex)
			{
				Log.Error(ex.ToString());
				Console.ReadKey();
				Environment.Exit(-1);
			}
		}
	}
}
