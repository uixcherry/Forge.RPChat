using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace Forge.RPChat.Commands
{
    public class CommandWhisper : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "w";
        public string Help => string.Empty;
        public string Syntax => "<player> <message>";
        public List<string> Aliases => new List<string> { "whisp" };
        public List<string> Permissions => new List<string> { "forge.rpchat.whisp" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length < 2)
            {
                UnturnedChat.Say(player, Plugin.Instance.Translate("command_generic_invalid_parameter"), Color.yellow);
                return;
            }

            string targetName = command[0];
            string message = string.Join(" ", command, 1, command.Length - 1);
            UnturnedPlayer target = UnturnedPlayer.FromName(targetName);

            if (target == null)
            {
                UnturnedChat.Say(player, Plugin.Instance.Translate("command_generic_invalid_parameter"), Color.red);
                return;
            }

            float radius = Plugin.Instance.Configuration.Instance.WhisperCommandRadius;

            if (Vector3.Distance(player.Position, target.Position) > radius)
            {
                UnturnedChat.Say(player, Plugin.Instance.Translate("command_generic_invalid_parameter"), Color.red);
                return;
            }

            UnturnedChat.Say(target, Plugin.Instance.Translate("whisp_format", Plugin.Instance.Translate("whisp_prefix") + message, player.CharacterName), true);

            foreach (SteamPlayer client in Provider.clients)
            {
                UnturnedPlayer nearbyPlayer = UnturnedPlayer.FromSteamPlayer(client);
                if (nearbyPlayer != null && nearbyPlayer != target && nearbyPlayer != player && Vector3.Distance(player.Position, nearbyPlayer.Position) <= radius)
                {
                    UnturnedChat.Say(nearbyPlayer, Plugin.Instance.Translate("whisp_nearby", player.CharacterName), true);
                }
            }
        }
    }
}