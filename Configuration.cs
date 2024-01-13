using Rocket.API;
using System.Collections.Generic;

namespace AntiRaid
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool Debug { get; set; }
        public bool AllowPlantRaids { get; set; }
        public bool PreventMeleeRaids { get; set; }
        public List<ushort> MeleeWhitelist { get; set; }
        public bool PreventGunRaids { get; set; }
        public List<ushort> GunWhitelist { get; set; }
        public bool PreventAllRaids { get; set; }
        public string BypassPermission { get; set; }
        public void LoadDefaults()
        {
            AllowPlantRaids = true;
            PreventMeleeRaids = true;
            MeleeWhitelist = new List<ushort>() { 15090 };
            PreventGunRaids = true;
            GunWhitelist = new List<ushort>() { 132 };
            PreventAllRaids = false;
            BypassPermission = "AntiRaid.AllowRaiding";
        }
    }
}
