using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace Forge.RPChat.Commands
{
    public class CommandNrp : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "b";
        public string Help => string.Empty;
        public string Syntax => "<message>";
        public List<string> Aliases => new List<string> { "nrp" };
        public List<string> Permissions => new List<string> { "forge.rpchat.b" };

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

            string formattedMessage = Plugin.Instance.Translate("nrp_format", Plugin.Instance.Translate("nrp_prefix") + message, player.CharacterName);

            foreach (SteamPlayer client in Provider.clients)
            {
                UnturnedPlayer nearbyPlayer = UnturnedPlayer.FromSteamPlayer(client);
                if (nearbyPlayer != null)
                {
                    UnturnedChat.Say(nearbyPlayer, formattedMessage, true);
                }
            }
        }
    }
}