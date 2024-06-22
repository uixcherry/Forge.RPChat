using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace Forge.RPChat.Commands
{
    public class CommandTry : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "try";
        public string Help => string.Empty;
        public string Syntax => "<action>";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "forge.rpchat.try" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length == 0)
            {
                UnturnedChat.Say(player, Plugin.Instance.Translate("command_generic_invalid_parameter"), Color.yellow);
                return;
            }

            string action = string.Join(" ", command).Trim().ToLower();

            if (string.IsNullOrEmpty(action))
            {
                UnturnedChat.Say(player, Plugin.Instance.Translate("command_generic_invalid_parameter"), Color.yellow);
                return;
            }

            string result = new System.Random().Next(2) == 0 ? Plugin.Instance.Translate("try_success") : Plugin.Instance.Translate("try_fail");

            string formattedMessage = Plugin.Instance.Translate("try_format", player.CharacterName, Plugin.Instance.Translate("try_prefix") + action, result);

            float radius = Plugin.Instance.Configuration.Instance.TryCommandRadius;
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