using Rocket.API;
using System.Collections.Generic;

namespace Forge.RPChat
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool EnableGlobalChat { get; set; }
        public bool EnableLocalChat { get; set; }
        
        public bool EnableGroupChat { get; set; }
        public List<string> GlobalChatPermissions { get; set; }
        public List<string> LocalChatPermissions { get; set; }
        public List<string> GroupChatPermissions { get; set; }
        
        public float MeCommandRadius { get; set; }
        public float DoCommandRadius { get; set; }
        public float TryCommandRadius { get; set; }
        public float RollCommandRadius { get; set; }
        public float ShoutCommandRadius { get; set; }
        public float WhisperCommandRadius { get;  set; }

        public void LoadDefaults()
        {
            EnableGlobalChat = true;
            EnableLocalChat = true;
            EnableGroupChat = true;
            
            GlobalChatPermissions = new List<string> { "default.permission", "vip.permission" };
            LocalChatPermissions = new List<string> { "default.permission" };
            GroupChatPermissions = new List<string> { "default.permission" };
            
            MeCommandRadius = 10f;
            DoCommandRadius = 10f;
            TryCommandRadius = 10f;
            RollCommandRadius = 10f;
            ShoutCommandRadius = 4f;
            WhisperCommandRadius = 25f;
        }
    }
}