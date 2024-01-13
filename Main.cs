using AntiRaid.Helpers;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace AntiRaid
{
    public class Main : RocketPlugin<Configuration>
    {
        public string PluginName = "AntiRaid";
        public string PluginVersion = "v1.0.0";
        public string PluginSupportURL = "discord.gg/SdnkVzJAWF";

        public static Main Instance { get; set; }

        protected override void Load()
        {
            Instance = this;

            BarricadeManager.onDamageBarricadeRequested += OnDamageBarricade;

            Logger.Log($"{PluginName} {PluginVersion} loaded successfully!");
            Logger.Log($"Created by Akulation >> {PluginSupportURL}");
        }

        private void OnDamageBarricade(CSteamID instigatorSteamID, Transform barricadeTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            if (instigatorSteamID != null)
            {
                var _instigator = UnturnedPlayer.FromCSteamID(instigatorSteamID);

                if (_instigator.HasPermission(Configuration.Instance.BypassPermission))
                {
                    shouldAllow = true;
                    return;
                }
            }

            var barricade = BarricadeManager.FindBarricadeByRootTransform(barricadeTransform);

            if (Configuration.Instance.PreventAllRaids)
            {
                if (instigatorSteamID != null)
                {
                    MessageUtil.SendMessage("NotAllowedToRaid", UnturnedPlayer.FromCSteamID(instigatorSteamID));
                }

                shouldAllow = false;
                return;
            }

            if (barricade.interactable is InteractableFarm)
            {
                if (Configuration.Instance.AllowPlantRaids)
                {
                    shouldAllow = true;
                    return;
                }
            }

            if (instigatorSteamID == null) 
                return;

            var instigator = UnturnedPlayer.FromCSteamID(instigatorSteamID);

            List<EDamageOrigin> meleeDamage = new() { EDamageOrigin.Punch, EDamageOrigin.Useable_Melee };
            List<EDamageOrigin> gunDamage = new() { EDamageOrigin.Useable_Gun, EDamageOrigin.Bullet_Explosion };

            if (Configuration.Instance.PreventMeleeRaids && meleeDamage.Contains(damageOrigin))
            {
                if ((!instigator.Player.equipment.isBusy || 
                    instigator.Player.equipment.IsEquipAnimationFinished) && 
                    Configuration.Instance.MeleeWhitelist.Contains(instigator.Player.equipment.itemID))
                {
                    shouldAllow = true;
                    return;
                }
                MessageUtil.SendMessage("NotAllowedToRaid", instigator);
                shouldAllow = false;
                return;
            }

            if (gunDamage.Contains(damageOrigin) && Configuration.Instance.PreventGunRaids)
            {
                if ((!instigator.Player.equipment.isBusy || 
                    instigator.Player.equipment.IsEquipAnimationFinished) && 
                    Configuration.Instance.GunWhitelist.Contains(instigator.Player.equipment.itemID))
                {
                    shouldAllow = true;
                    return;
                }
                MessageUtil.SendMessage("NotAllowedToRaid", instigator);
                shouldAllow = false;
                return;
            }
        }

        protected override void Unload()
        {
            Instance = null;
            Logger.Log($"{PluginName} {PluginVersion} has been unloaded successfully!");
        }

        public override TranslationList DefaultTranslations => new()
        {
            { "NotAllowedToRaid", "<color=red>You are not allowed to raid this object!" },
            { "Error", "<color=red>An error has occured.</color>" }
        };
    }
}