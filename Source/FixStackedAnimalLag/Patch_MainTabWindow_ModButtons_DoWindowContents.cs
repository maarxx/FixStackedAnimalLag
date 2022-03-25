using HarmonyLib;
using ModButtons;

namespace FixStackedAnimalLag;

[HarmonyPatch(typeof(MainTabWindow_ModButtons))]
[HarmonyPatch("DoWindowContents")]
internal class Patch_MainTabWindow_ModButtons_DoWindowContents
{
    private static void Prefix()
    {
        FixStackedAnimalLag_RegisterToMainTab.ensureMainTabRegistered();
    }
}