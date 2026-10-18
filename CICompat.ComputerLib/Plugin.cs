using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace CICompat.ComputerLib;

[BepInPlugin(Constants.Guid, Constants.Name, Constants.Version)]
[BepInDependency("tonimacaroni.computerinterface", "2.1.0"), BepInDependency("tech.lapiscreates.ComputerLib", "1.0.0")]
internal class Plugin : BaseUnityPlugin {
    public new static ManualLogSource Logger;

    private Plugin() {
        Logger = base.Logger;

        Harmony.UnpatchID("tech.lapiscreates.ComputerLib");

        Harmony.CreateAndPatchAll(GetType().Assembly, Constants.Guid);
    }
}