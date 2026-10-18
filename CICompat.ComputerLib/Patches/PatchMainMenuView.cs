using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Bootstrap;
using CICompat.ComputerLib.Tools;
using CICompat.ComputerLib.Views;
using ComputerInterface.Interfaces;
using ComputerInterface.Views;
using ComputerLib.Types;
using HarmonyLib;

namespace CICompat.ComputerLib.Patches;

[HarmonyPatch(typeof(MainMenuView))]
internal static class PatchMainMenuView {
    [HarmonyPatch("ShowViewEntries"), HarmonyPrefix]
    private static void AddComputerLibScreens(List<IComputerViewEntry> viewEntries) {
        viewEntries.RemoveAll(entry => entry is ScreenViewEntry);

        foreach (Type screenType in FindScreenTypes()) {
            try {
                viewEntries.Add(new ScreenViewEntry(screenType));
            }
            catch (Exception exception) {
                Logging.Error($"Failed to register {screenType.Name}: {exception.Message}");
            }
        }
    }

    private static List<Type> FindScreenTypes() {
        IEnumerable<Assembly> assemblies = Chainloader.PluginInfos.Values.Select(pluginInfo => pluginInfo.Instance.GetType().Assembly).Distinct();

        return [
            .. assemblies.SelectMany(GetLoadableTypes).Where(screenType =>
                screenType != typeof(Screen) && typeof(Screen).IsAssignableFrom(screenType) && !screenType.IsAbstract &&
                !screenType.IsInterface && !screenType.ContainsGenericParameters &&
                screenType.GetConstructor(Type.EmptyTypes) != null).Distinct()
        ];
    }

    private static IEnumerable<Type> GetLoadableTypes(Assembly assembly) {
        try {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException exception) {
            return exception.Types.Where(type => type != null);
        }
    }
}