using UnityEngine;
using UModFramework.API;

namespace SRCubeSlimeMod
{
    class SRCubeSlimeMod
    {
        internal static Mesh replaceMesh = null;
        internal static bool updateActiveSlimes = false;

        private static readonly string meshFile = "cube_slime.obj";

        //The Meshes to be replaced
        internal static readonly string[] replaceMeshes = {
            "slime_default_LOD1",
            "slime_default_LOD2",
            "slime_default_LOD3",
            "mosaic_LOD1",
            "mosaic_LOD2",
            "mosaic_LOD3"
        };

        //The Slimes which have replacement icons
        internal static readonly Identifiable.Id[] replaceIcons = {
            Identifiable.Id.PINK_SLIME,
            Identifiable.Id.ROCK_SLIME,
            Identifiable.Id.PHOSPHOR_SLIME,
            Identifiable.Id.TABBY_SLIME,
            Identifiable.Id.HONEY_SLIME,
            Identifiable.Id.BOOM_SLIME,
            Identifiable.Id.RAD_SLIME,
            Identifiable.Id.CRYSTAL_SLIME
        };

        [UMFHarmony(4)]
        public static void Start()
        {
            Log("Slime Rancher Cube Slime Mod v" + UMFMod.GetModVersion().ToString(), true);

            //Load the Mesh
            replaceMesh = UMFAsset.LoadMesh(meshFile);
            if (replaceMesh == null) Log("Error: Failed to load the mesh.");
            else Log("Successfully loaded the mesh.");
        }

        [UMFConfig]
        public static void LoadConfig()
        {
            SRCSMConfig.Instance.Load();
        }

        internal static void Log(string text, bool clean = false)
        {
            using (UMFLog log = new UMFLog()) log.Log(text, clean);
        }
    }
}