using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Harmony;
using UModFramework.API;

namespace SRCubeSlimeMod.Patches
{
    [HarmonyPatch(typeof(LookupDirector))]
    [HarmonyPatch("Awake")]
    class Patch_CubeSlimes
    {
        private static bool patched = false;

        //The Slimes which have Cube Icons
        private static readonly Identifiable.Id[] replaceIcons = {
            Identifiable.Id.PINK_SLIME,
            Identifiable.Id.ROCK_SLIME,
            Identifiable.Id.PHOSPHOR_SLIME,
            Identifiable.Id.TABBY_SLIME,
            Identifiable.Id.HONEY_SLIME,
            Identifiable.Id.BOOM_SLIME,
            Identifiable.Id.RAD_SLIME,
            Identifiable.Id.CRYSTAL_SLIME
        };

        public static void Prefix(LookupDirector __instance, ref List<GameObject> ___identifiablePrefabs, ref List<LookupDirector.VacEntry> ___vacEntries)
        {
            if (!Levels.isSpecial() || patched) return;
            patched = true;

            //Load the Cube Slime Mesh
            Mesh cubeMesh = UMFAsset.LoadMesh("cube_slime.obj");
            if (cubeMesh == null)
            {
                SRCubeSlimeMod.Log("Error: Failed to load the Cube Slime mesh.");
                return;
            }
            SRCubeSlimeMod.Log("Successfully loaded the Cube Slime mesh.");

            //Replace the mesh on all slimes with the cube slime mesh
            List<GameObject> prefabs = ___identifiablePrefabs;
            foreach (GameObject go in prefabs)
            {
                Identifiable ident = go.GetComponent<Identifiable>();
                //if (!ident || !Identifiable.IsSlime(ident.id)) continue;
                MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
                foreach (MeshFilter meshFilter in meshFilters)
                {
                    if (meshFilter.name == "slime_default_LOD1") meshFilter.mesh = cubeMesh;
                    if (meshFilter.name == "slime_default_LOD2") meshFilter.mesh = cubeMesh;
                    if (meshFilter.name == "slime_default_LOD3") meshFilter.mesh = cubeMesh;
                }
            }
            Traverse.Create(__instance).Field("identifiablePrefabs").SetValue(prefabs);
            SRCubeSlimeMod.Log("Successfully replaced all Slime Meshes with the Cube Slime mesh.");

            //Load and replace the icons
            List<LookupDirector.VacEntry> vacEntries = ___vacEntries;
            foreach (LookupDirector.VacEntry entry in vacEntries)
            {
                if (replaceIcons.Contains(entry.id))
                {
                    Texture2D icon = UMFAsset.LoadTexture2D("icon_" + entry.id.ToString().ToLower() + ".png");
                    Sprite sprite = Sprite.Create(icon, entry.icon.rect, entry.icon.pivot, entry.icon.pixelsPerUnit);
                    entry.icon = sprite;
                }
            }
            Traverse.Create(__instance).Field("vacEntries").SetValue(vacEntries);
            SRCubeSlimeMod.Log("Successfully replaced the Slime Icons with the Cube Slime icons.");
        }
    }
}