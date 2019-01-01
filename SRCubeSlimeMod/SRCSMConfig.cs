using System;
using UModFramework.API;

namespace SRCubeSlimeMod
{
    internal class SRCSMConfig
    {
        private static readonly string configVersion = "1.0";

        public static bool AutoSetLOD;

        internal void Load()
        {
            SRCubeSlimeMod.Log("Loading settings.");
            try
            {
                using (UMFConfig cfg = new UMFConfig())
                {
                    string cfgVer = cfg.Read("ConfigVersion", new UMFConfigString());
                    if (cfgVer != string.Empty && cfgVer != configVersion)
                    {
                        cfg.DeleteConfig();
                        SRCubeSlimeMod.Log("The config file was outdated and has been deleted. A new config will be generated.");
                    }

                    cfg.Write("SupportsHotLoading", new UMFConfigBool(false));//Due to where the mod overwrites Slime meshes/icons it does not support hot loading.
                    cfg.Read("LoadPriority", new UMFConfigString("Normal"));
                    cfg.Write("MinVersion", new UMFConfigString("0.50"));
                    //cfg.Write("MaxVersion", new UMFConfigString("0.54.99999.99999"));
                    cfg.Write("UpdateURL", new UMFConfigString(@"https://raw.githubusercontent.com/EmeraldPlay27/SRCubeSlimeMod/master/version.txt"));
                    cfg.Write("ConfigVersion", new UMFConfigString(configVersion));

                    SRCubeSlimeMod.Log("Finished UMF Settings.");

                    AutoSetLOD = cfg.Read("AutoSetLOD", new UMFConfigBool(true, null, true), "Automatically sets the internal LOD setting so that Cube Slimes are always visible at all Model Quality settings.");

                    SRCubeSlimeMod.Log("Finished loading settings.");
                }
            }
            catch (Exception e)
            {
                SRCubeSlimeMod.Log("Error loading mod settings: " + e.Message + " (" + e.InnerException.Message + ")");
            }
        }

        public static SRCSMConfig Instance { get; } = new SRCSMConfig();
    }
}