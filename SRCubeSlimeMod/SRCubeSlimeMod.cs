using UModFramework.API;

namespace SRCubeSlimeMod
{
    class SRCubeSlimeMod
    {
        [UMFHarmony(3)]
        public static void Start()
        {
            Log("Slime Rancher Cube Slime Mod v" + UMFMod.GetModVersion().ToString(), true);
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