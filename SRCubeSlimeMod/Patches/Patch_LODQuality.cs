using UnityEngine;
using MonomiPark.SlimeRancher;
using HarmonyLib;

namespace SRCubeSlimeMod.Patches
{
    [HarmonyPatch(typeof(SavedProfile))]
    [HarmonyPatch("PushQualitySettings")]
    static class Patch_LODQuality1
    {
        public static void Postfix()
        {
            //Sets the lod level when the game is started.
            if (SRCSMConfig.AutoSetLOD) QualitySettings.maximumLODLevel = 1;
        }
    }

    [HarmonyPatch(typeof(OptionsUI))]
    [HarmonyPatch("Close")]
    static class Patch_LODQuality2
    {
        public static void Postfix()
        {
            //Sets the lod level when the user changes options.
            if (SRCSMConfig.AutoSetLOD) QualitySettings.maximumLODLevel = 1;
        }
    }
}