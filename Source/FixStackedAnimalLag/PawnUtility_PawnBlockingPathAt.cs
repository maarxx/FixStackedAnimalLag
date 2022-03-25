using HarmonyLib;
using RimWorld;
using Verse;

namespace FixStackedAnimalLag;

[HarmonyPatch(typeof(PawnUtility))]
[HarmonyPatch("PawnBlockingPathAt")]
internal class PawnUtility_PawnBlockingPathAt
{
    private static bool Prefix(ref IntVec3 c, ref Pawn forPawn, ref Pawn __result)
    {
        if (!FixStackedAnimalLag_GlobalRuntimeSettings.shouldCollideEnemies || !forPawn.HostileTo(Faction.OfPlayer))
        {
            return true;
        }

        var thingList = c.GetThingList(forPawn.Map);
        foreach (var thing in thingList)
        {
            if (thing is not Pawn pawn || pawn == forPawn || pawn.Downed)
            {
                continue;
            }

            __result = pawn;
            return false;
        }

        return true;
    }
}