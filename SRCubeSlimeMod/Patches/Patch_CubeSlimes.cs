using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using UModFramework.API;

namespace SRCubeSlimeMod.Patches
{
    [HarmonyPatch(typeof(LookupDirector))]
    [HarmonyPatch("Awake")]
    class Patch_CubeSlimes1
    {
        private static bool patched = false;

        public static void Prefix(LookupDirector __instance, ref List<GameObject> ___identifiablePrefabs, ref List<LookupDirector.VacEntry> ___vacEntries)
        {
            if (!Levels.isSpecial() || patched) return;
            patched = true;

            //Replace the mesh on all slimes with the cube slime mesh
            List<GameObject> prefabs = ___identifiablePrefabs;
            foreach (GameObject go in prefabs)
            {
                Identifiable identifiable = go.GetComponent<Identifiable>();
                if (!identifiable || !Identifiable.IsSlime(identifiable.id)) continue;
                MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
                foreach (MeshFilter meshFilter in meshFilters)
                {
                    if (SRCubeSlimeMod.replaceMeshes.Contains(meshFilter.name)) meshFilter.mesh = SRCubeSlimeMod.replaceMesh;
                }
            }
            Traverse.Create(__instance).Field("identifiablePrefabs").SetValue(prefabs);
            SRCubeSlimeMod.Log("Successfully replaced the mesh in prefab Slimes.");

            //Load and replace the icons
            List<LookupDirector.VacEntry> vacEntries = ___vacEntries;
            foreach (LookupDirector.VacEntry entry in vacEntries)
            {
                if (SRCubeSlimeMod.replaceIcons.Contains(entry.id))
                {
                    Texture2D icon = UMFAsset.LoadTexture2D("icon_" + entry.id.ToString().ToLower() + ".png");
                    Sprite sprite = Sprite.Create(icon, entry.icon.rect, entry.icon.pivot, entry.icon.pixelsPerUnit);
                    entry.icon = sprite;
                }
            }
            Traverse.Create(__instance).Field("vacEntries").SetValue(vacEntries);
            SRCubeSlimeMod.Log("Successfully replaced the Slime Icons.");

            SRCubeSlimeMod.updateActiveSlimes = true;
        }
    }

    //Replaces meshes in active already loaded slimes on level/map change
    [HarmonyPatch(typeof(LookupDirector))]
    [HarmonyPatch("LateUpdate")]
    class Patch_CubeSlimes2
    {
        private static bool wasMainMenu = true;

        public static void Postfix(LookupDirector __instance)
        {
            if (wasMainMenu != Levels.isSpecial())
            {
                wasMainMenu = Levels.isSpecial();
                SRCubeSlimeMod.updateActiveSlimes = true;
            }

            if (!SRCubeSlimeMod.updateActiveSlimes) return;
            SRCubeSlimeMod.updateActiveSlimes = false;

            int num = 0;
            foreach (Identifiable identifiable in Object.FindObjectsOfType<Identifiable>())
            {
                if (!Identifiable.IsSlime(identifiable.id)) continue;
                MeshFilter[] meshFilters = identifiable.gameObject.GetComponentsInChildren<MeshFilter>();
                foreach (MeshFilter meshFilter in meshFilters)
                {
                    if (SRCubeSlimeMod.replaceMeshes.Contains(meshFilter.name)) meshFilter.mesh = SRCubeSlimeMod.replaceMesh;
                }
                num++;
            }
            SRCubeSlimeMod.Log("Successfully replaced the mesh in " + num.ToString() + " active Slimes.");
        }
    }
}