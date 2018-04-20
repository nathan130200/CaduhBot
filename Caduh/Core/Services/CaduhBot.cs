using Caduh.Entities;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Caduh.Core.Services
{
	public class CaduhBot
	{
		public DiscordClient DiscordApi {
			get;
			private set;
		}

		public CaduhBot()
		{
			DiscordApi = new DiscordClient(new DiscordConfiguration
			{
				AutoReconnect = true,
				UseInternalLogHandler = false,
				Token = Config.Instance.Token,
				TokenType = Config.Instance.TokenType,
				LogLevel = Config.Instance.LogLevel
			});

			DiscordApi.SetWebSocketClient<DSharpPlus.Net.WebSocket.WebSocketSharpClient>();

			DiscordApi.Ready += this.DiscordApi_Ready;
			DiscordApi.DebugLogger.LogMessageReceived += this.DiscordApi_LogMessageReceived;
		}

		void DiscordApi_LogMessageReceived(object sender, DSharpPlus.EventArgs.DebugLogMessageEventArgs e)
		{
			switch (e.Level)
			{
				case LogLevel.Critical:
				case LogLevel.Error:
					Log.Error(e.Application + ": " + e.Message);
					break;

				case LogLevel.Info:
				case LogLevel.Debug:
					Log.Info(e.Application + ": " + e.Message);
					break;

				case LogLevel.Warning:
					Log.Warn(e.Application + ": " + e.Message);
					break;
			}
		}

		async Task DiscordApi_Ready(DSharpPlus.EventArgs.ReadyEventArgs e)
		{
			await DiscordApi.UpdateStatusAsync(new DiscordGame
			{
				Name = "SDK | " + DiscordApi.Ping + "ms",
				StreamType = GameStreamType.Twitch,
				Url = "https://twitch.tv/nathanferreiradsr_"
			});
		}

		public void SetupInteractivity()
		{
			DiscordApi.UseInteractivity(new InteractivityConfiguration
			{
				PaginationBehaviour = TimeoutBehaviour.Ignore,
				PaginationTimeout = TimeSpan.FromSeconds(120),
				Timeout = TimeSpan.FromSeconds(60)
			});
		}

		public void SetupCommandsNext()
		{
			var module = DiscordApi.UseCommandsNext(new CommandsNextConfiguration
			{
				StringPrefix = "!",
				CaseSensitive = false,
				EnableDefaultHelp = true,
				EnableDms = false,
				EnableMentionPrefix = true,
				IgnoreExtraArguments = true,
			});

			module.CommandExecuted += async (e) =>
			{
				var ctx = e.Context;
				Log.Info("Command Executed: #{0} {1}#{2}: {3}", ctx.Channel.Name, ctx.User.Username, ctx.User.Discriminator, ctx.Message.Content);
			};

			module.CommandErrored += async (e) =>
			{
				if (e.Exception is CommandNotFoundException)
					return;

			    var ctx = e.Context;
				Log.Error("Command Errored: #{0} {1}#{2}: {3}\r\n{4}\r\n", ctx.Channel.Name, ctx.User.Username, ctx.User.Discriminator, ctx.Message.Content, e.Exception.ToString() + "\r\n" + e.Exception.StackTrace);
			};

			module.RegisterCommands(Assembly.GetEntryAssembly());
		}

		public async Task ConnectAsync()
			=> await DiscordApi.ConnectAsync();

		public async Task DisconnectAsync()
			=> await DiscordApi.DisconnectAsync();

	}
}
