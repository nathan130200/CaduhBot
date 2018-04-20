using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caduh.Entities
{
	public class Config
	{
		public string Token {
			get;
			set;
		}

		public DSharpPlus.TokenType TokenType {
			get;
			set;
		}

		public DSharpPlus.LogLevel LogLevel {
			get;
			set;
		}

		public static Config Instance {
			get;
			private set;
		}

		public static void Load()
		{
			var file = new FileInfo(Directory.GetCurrentDirectory() + @"\Config.json");

			if (!file.Exists)
			{
				Instance = new Config();
				Instance.Token = "";
				Instance.TokenType = DSharpPlus.TokenType.Bot;
				Instance.LogLevel = DSharpPlus.LogLevel.Debug;
				File.WriteAllText(file.FullName, JsonConvert.SerializeObject(Instance));
			}
			else
			{
				Instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText(file.FullName));
			}
		}
	}
}
