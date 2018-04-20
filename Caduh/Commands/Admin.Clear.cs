using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduh.Commands
{
	[Group("admin")]
	public partial class Admin
	{
		[Command("limpar")]
		[RequireUserPermissions(Permissions.Administrator)]
		[RequirePermissions(Permissions.ManageMessages)]
		public async Task ClearAsync(CommandContext ctx, int quantidade = 10)
		{
			var loading = await ctx.RespondAsync(embed: new DiscordEmbedBuilder()
				.WithAuthor("Limpando Chat...", icon_url: "https://i.imgur.com/3ufvo0l.gif")
				.WithColor(DiscordColor.Red)
			);

			if (quantidade > 100)
				quantidade = 100;

			foreach(var message in await ctx.Channel.GetMessagesAsync(quantidade))
			{
				if (!message.Pinned)
				{
					if (message.Id != loading.Id)
					{
						try { await message.DeleteAsync(); } catch { }
					}
				}
			}

			await loading.ModifyAsync("@everyone", embed: new DiscordEmbedBuilder()
				.WithAuthor($"Chat Limpo por {ctx.User.Username}#{ctx.User.Discriminator}", icon_url: "https://i.imgur.com/Tl3gkGH.png")
				.WithColor(DiscordColor.Green)
			);

			await Task.Delay(6500);
			try { await loading.DeleteAsync(); } catch { }
		}
	}
}
