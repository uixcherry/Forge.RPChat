using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace Forge.RPChat
{
    public class Plugin : RocketPlugin<Configuration>
    {
        public static Plugin Instance { get; private set; }

        private Dictionary<EChatMode, List<string>> chatPermissions;

        protected override void Load()
        {
            Instance = this;
            UnturnedPlayerEvents.OnPlayerChatted += OnPlayerChatted;

            InitializeChatPermissions();

            Logger.Log("Plugin Loaded");
            Logger.Log("Contact: discord.gg/HB9G962FRY");
        }

        protected override void Unload()
        {
            Instance = null;
            UnturnedPlayerEvents.OnPlayerChatted -= OnPlayerChatted;

            Logger.Log("Plugin Unloaded");
        }

        private void InitializeChatPermissions()
        {
            chatPermissions = new Dictionary<EChatMode, List<string>>
            {
                { EChatMode.GLOBAL, Configuration.Instance?.GlobalChatPermissions ?? new List<string>() },
                { EChatMode.LOCAL, Configuration.Instance?.LocalChatPermissions ?? new List<string>() },
                { EChatMode.GROUP, Configuration.Instance?.GroupChatPermissions ?? new List<string>() }
            };
        }

        private void OnPlayerChatted(UnturnedPlayer player, ref Color color, string message, EChatMode chatMode, ref bool cancel)
        {
            if (!IsChatEnabled(chatMode, player))
            {
                cancel = true;
                UnturnedChat.Say(player, Translate("chat_disabled", chatMode.ToString()), Color.red);
            }
        }

        private bool IsChatEnabled(EChatMode chatMode, UnturnedPlayer player)
        {
            if (player.IsAdmin || HasPermission(player, chatMode))
                return true;

            switch (chatMode)
            {
                case EChatMode.GLOBAL:
                    return Configuration.Instance?.EnableGlobalChat ?? false;
                case EChatMode.LOCAL:
                    return Configuration.Instance?.EnableLocalChat ?? false;
                case EChatMode.GROUP:
                    return Configuration.Instance?.EnableGroupChat ?? false;
                default:
                    return true;
            }
        }

        private bool HasPermission(UnturnedPlayer player, EChatMode chatMode)
        {
            List<string> permissions;
            if (chatPermissions.TryGetValue(chatMode, out permissions))
            {
                return permissions.Any(player.HasPermission);
            }
            return false;
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            { "me_format", "<color=#FFA500>{0}</color> {1}" },
            { "me_prefix", "ME: " },

            { "do_format", "{0} ({1})" },
            { "do_prefix", "DO: " },

            { "try_format", "{0} {1} ({2})" },
            { "try_prefix", "TRY: " },
            { "try_success", "successfully" },
            { "try_fail", "unsuccessfully" },

            { "roll_format", "{0} ({1})" },
            { "roll_prefix", "ROLL: " },
            { "roll_error_min_max", "Minimum value cannot be greater than maximum value." },

            { "nrp_format", "{0} ({1})" },
            { "nrp_prefix", "OOC: " },

            { "shout_format", "{0} ({1})" },
            { "shout_prefix", "SHOUT: " },

            { "whisp_format", "{0} ({1})" },
            { "whisp_prefix", "WHISPER: " },
            { "whisp_nearby", "{0} is whispering something." },

            { "command_generic_invalid_parameter", "Invalid parameter." },
            { "chat_disabled", "{0} chat is disabled for you." }
        };
    }
}