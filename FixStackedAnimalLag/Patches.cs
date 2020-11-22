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
            //Log.Message("Hello from Harmony in scope: com.github.harmony.rimworld.maarx.fixstackedanimallag");
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
            if (forPawn.HostileTo(Faction.OfPlayer))
            {
                //actAsIfHadCollideWithPawnsJob = true;
                //collideOnlyWithStandingPawns = false;
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
            //Log.Message("Hello from Harmony PawnUtility_ShouldCollideWithPawns Prefix");
            if (p.RaceProps.Animal && p.Faction == Faction.OfPlayer)
            {
                __result = false;
                return false;
            }
            if (p.HostileTo(Faction.OfPlayer))
            {
                __result = true;
                return false;
            }
            return true;
        }
    }

    //[HarmonyPatch(typeof(GenHostility))]
    //[HarmonyPatch("IsActiveThreatTo")]
    //class GenHostility_IsActiveThreatTo
    //{
    //    static bool Prefix(ref bool __result)
    //    {
    //        Log.Message("Hello from Harmony GenHostility_IsActiveThreatTo Prefix");
    //        //return true;
    //        __result = false;
    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(JobGiver_AIFightEnemies))]
    //[HarmonyPatch("TryFindShootingPosition")]
    //class JobGiver_AIFightEnemies_TryFindShootingPosition
    //{
    //    static bool Prefix(ref bool __result)
    //    {
    //        Log.Message("Hello from Harmony JobGiver_AIFightEnemies_TryFindShootingPosition Prefix");
    //        __result = false;
    //        return true;
    //    }
    //}

    //[HarmonyPatch(typeof(JobGiver_AIGotoNearestHostile))]
    //[HarmonyPatch("TryGiveJob")]
    //class JobGiver_AIGotoNearestHostile_TryGiveJob
    //{
    //    static bool Prefix(ref Job __result)
    //    {
    //        Log.Message("Hello from Harmony JobGiver_AIGotoNearestHostile_TryGiveJob Prefix");
    //        __result = null;
    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(Lord))]
    //[HarmonyPatch("LordTick")]
    //class Lord_LordTick
    //{
    //    static bool Prefix()
    //    {
    //        Log.Message("Hello from Harmony Lord_LordTick Prefix");
    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(JobGiver_AIFightEnemy))]
    //[HarmonyPatch("TryGiveJob")]
    //class JobGiver_AIFightEnemy_TryGiveJob
    //{
    //    static bool Prefix(ref Job __result)
    //    {
    //        Log.Message("Hello from Harmony JobGiver_AIFightEnemy_TryGiveJob Prefix");
    //        __result = null;
    //        return false;
    //    }
    //}
}
