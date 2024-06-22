using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace Forge.RPChat.Commands
{
    public class CommandDo : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "do";
        public string Help => string.Empty;
        public string Syntax => "<description>";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "forge.rpchat.do" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length == 0)
            {
                UnturnedChat.Say(player, Plugin.Instance.Translate("command_generic_invalid_parameter"), Color.yellow);
                return;
            }

            string description = string.Join(" ", command).Trim();

            if (string.IsNullOrEmpty(description))
            {
                UnturnedChat.Say(player, Plugin.Instance.Translate("command_generic_invalid_parameter"), Color.yellow);
                return;
            }

            description = char.ToUpper(description[0]) + description.Substring(1);
            if (!description.EndsWith("."))
                description += ".";

            string formattedMessage = Plugin.Instance.Translate("do_format", Plugin.Instance.Translate("do_prefix") + description, player.CharacterName);

            float radius = Plugin.Instance.Configuration.Instance.DoCommandRadius;
            foreach (SteamPlayer client in Provider.clients)
            {
                UnturnedPlayer nearbyPlayer = UnturnedPlayer.FromSteamPlayer(client);
                if (nearbyPlayer != null && Vector3.Distance(player.Position, nearbyPlayer.Position) <= radius)
                {
                    UnturnedChat.Say(nearbyPlayer, formattedMessage, true);
                }
            }
        }
    }
}