using Rocket.Core.Logging;
using Rocket.Core.Utils;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace AntiRaid.Helpers
{
    public class MessageUtil
    {
        public static void SendMessageSync(string translation, UnturnedPlayer player, bool rich = true, object[] arguments = null)
        {
            if (arguments is null)
            {
                TaskDispatcher.QueueOnMainThread(() => SendMessage(translation, player, rich));
            }
            else
            {
                TaskDispatcher.QueueOnMainThread(() => SendMessage(translation, player, rich, arguments));
            }
        }
        public static void SendMessage(string translation, UnturnedPlayer player, bool rich = true, object[] arguments = null)
        {
            if (arguments == null)
            {
                ChatManager.serverSendMessage(Main.Instance.Translate(translation), Color.white, null, player.Player.channel.owner, EChatMode.SAY, null, rich);
            }
            else
            {
                ChatManager.serverSendMessage(Main.Instance.Translate(translation, arguments), Color.white, null, player.Player.channel.owner, EChatMode.SAY, null, rich);
            }
        }

        public static void SendDebug(string message)
        {
            if (Main.Instance == null || Main.Instance.Configuration.Instance.Debug == false || string.IsNullOrEmpty(message)) return;
            Rocket.Core.Logging.Logger.Log($"DEBUG >> {message}");
        }
    }
}
