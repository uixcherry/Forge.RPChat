using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace Forge.RPChat.Commands
{
    public class CommandShout : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "s";
        public string Help => string.Empty;
        public string Syntax => "<message>";
        public List<string> Aliases => new List<string> { "shout" };
        public List<string> Permissions => new List<string> { "forge.rpchat.shout" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length == 0)
            {
                UnturnedChat.Say(player, Plugin.Instance.Translate("command_generic_invalid_parameter"), Color.yellow);
                return;
            }

            string message = string.Join(" ", command).Trim();

            if (string.IsNullOrEmpty(message))
            {
                UnturnedChat.Say(player, Plugin.Instance.Translate("command_generic_invalid_parameter"), Color.yellow);
                return;
            }

            string formattedMessage = Plugin.Instance.Translate("shout_format", Plugin.Instance.Translate("shout_prefix") + message, player.CharacterName);

            float radius = Plugin.Instance.Configuration.Instance.ShoutCommandRadius;
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