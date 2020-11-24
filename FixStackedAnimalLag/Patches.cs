using HarmonyLib;
using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace FixStackedAnimalLag
{
    [StaticConstructorOnStartup]
    class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.github.harmony.rimworld.maarx.fixstackedanimallag");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(PawnCollisionTweenerUtility))]
    [HarmonyPatch("PawnCollisionPosOffsetFor")]
    class PawnCollisionTweenerUtility_PawnCollisionPosOffsetFor
    {
        static bool Prefix(ref Pawn pawn, ref Vector3 __result)
        {
            if (pawn != null && pawn.AnimalOrWildMan())
            {
                __result = Vector3.zero;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(PawnUtility))]
    [HarmonyPatch("PawnBlockingPathAt")]
    class PawnUtility_PawnBlockingPathAt
    {
        static bool Prefix(ref IntVec3 c, ref Pawn forPawn, ref bool actAsIfHadCollideWithPawnsJob, ref bool collideOnlyWithStandingPawns, ref bool forPathFinder, ref Pawn __result)
        {
            if (FixStackedAnimalLag_GlobalRuntimeSettings.shouldCollideEnemies && forPawn.HostileTo(Faction.OfPlayer))
            {
                List<Thing> thingList = c.GetThingList(forPawn.Map);
                for (int i = 0; i < thingList.Count; i++)
                {
                    Pawn pawn = thingList[i] as Pawn;
                    if (pawn != null && pawn != forPawn && !pawn.Downed)
                    {
                        __result = pawn;
                        return false;
                    }
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(PawnUtility))]
    [HarmonyPatch("ShouldCollideWithPawns")]
    class PawnUtility_ShouldCollideWithPawns
    {
        static bool Prefix(ref Pawn p, ref bool __result)
        {
            if (p.RaceProps.Animal && p.Faction == Faction.OfPlayer)
            {
                __result = false;
                return false;
            }
            if (FixStackedAnimalLag_GlobalRuntimeSettings.shouldCollideEnemies && p.HostileTo(Faction.OfPlayer))
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
}
