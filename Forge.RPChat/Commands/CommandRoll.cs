using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace Forge.RPChat.Commands
{
    public class CommandRoll : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "roll";
        public string Help => "Roll a random number between 1 and 100.";
        public string Syntax => "[min] [max]";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "forge.rpchat.roll" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            int min = 1;
            int max = 100;

            if (command.Length > 0)
            {
                if (int.TryParse(command[0], out int parsedMin))
                {
                    min = parsedMin;
                }
                if (command.Length > 1 && int.TryParse(command[1], out int parsedMax))
                {
                    max = parsedMax;
                }
            }

            if (min > max)
            {
                UnturnedChat.Say(player, Plugin.Instance.Translate("roll_error_min_max"), Color.red);
                return;
            }

            int roll = new System.Random().Next(min, max + 1);

            string message = Plugin.Instance.Translate("roll_format", Plugin.Instance.Translate("roll_prefix") + roll.ToString(), $"{min}-{max}");

            float radius = Plugin.Instance.Configuration.Instance.RollCommandRadius;
            foreach (SteamPlayer client in Provider.clients)
            {
                UnturnedPlayer nearbyPlayer = UnturnedPlayer.FromSteamPlayer(client);
                if (nearbyPlayer != null && Vector3.Distance(player.Position, nearbyPlayer.Position) <= radius)
                {
                    UnturnedChat.Say(nearbyPlayer, message, true);
                }
            }
        }
    }
}