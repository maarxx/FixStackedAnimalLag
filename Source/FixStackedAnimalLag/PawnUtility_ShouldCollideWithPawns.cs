using HarmonyLib;
using RimWorld;
using Verse;

namespace FixStackedAnimalLag;

[HarmonyPatch(typeof(PawnUtility))]
[HarmonyPatch("ShouldCollideWithPawns")]
internal class PawnUtility_ShouldCollideWithPawns
{
    private static bool Prefix(ref Pawn p, ref bool __result)
    {
        if (p.RaceProps.Animal && p.Faction == Faction.OfPlayer)
        {
            __result = false;
            return false;
        }

        if (!FixStackedAnimalLag_GlobalRuntimeSettings.shouldCollideEnemies || !p.HostileTo(Faction.OfPlayer) ||
            p.Downed)
        {
            return true;
        }

        __result = true;
        return false;
    }
}