using System.Reflection;
using HarmonyLib;
using Verse;

namespace FixStackedAnimalLag;

[StaticConstructorOnStartup]
internal class Main
{
    static Main()
    {
        var harmony = new Harmony("com.github.harmony.rimworld.maarx.fixstackedanimallag");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}

/*
[HarmonyPatch(typeof(WanderUtility))]
[HarmonyPatch("GetColonyWanderRoot")]
class WanderUtility_GetColonyWanderRoot
{
    private static Dictionary<Area, IntVec3> cachedRoots = new Dictionary<Area, IntVec3>();
    private static Dictionary<Area, int> cachedTimes = new Dictionary<Area, int>();
    private static int cacheTTL = 1000;
    static void Postfix(ref Pawn pawn, ref IntVec3 __result, ref bool __state)
    {
        if (!__state)
        {
            //Log.Message("Hello from GetColonyWanderRoot Postfix");
            Area pawnArea = pawn.playerSettings.AreaRestriction;
            cachedRoots.SetOrAdd(pawnArea, __result);
            cachedTimes.SetOrAdd(pawnArea, Find.TickManager.TicksGame);
        }
    }

    static bool Prefix(ref Pawn pawn, ref IntVec3 __result, ref bool __state)
    {
        //Log.Message("Hello from GetColonyWanderRoot Prefix");
        Area pawnArea = pawn.playerSettings.AreaRestriction;
        __state = false;
        if (cachedTimes.TryGetValue(pawnArea, int.MaxValue) < Find.TickManager.TicksGame + cacheTTL)
        {
            IntVec3 cachedValue = cachedRoots.TryGetValue(pawnArea, IntVec3.Invalid);
            if (cachedValue != IntVec3.Invalid)
            {
                __result = cachedValue;
                __state = true;
                return false;
            }
        }
        Log.Message("Hello from GetColonyWanderRoot Original");
        return true;
    }
}

[HarmonyPatch(typeof(RCellFinder))]
[HarmonyPatch("RandomWanderDestFor")]
class RCellFinder_RandomWanderDestFor
{
    private static Dictionary<IntVec3, IntVec3> cachedDests = new Dictionary<IntVec3, IntVec3>();
    private static Dictionary<IntVec3, int> cachedTimes = new Dictionary<IntVec3, int>();
    private static int cacheTTL = 1000;

    static void Postfix(ref Pawn pawn, ref IntVec3 __result, ref bool __state)
    {
        if (!__state)
        {
            //Log.Message("Hello from RandomWanderDestFor Postfix");
            IntVec3 startPos = pawn.Position;
            cachedDests.SetOrAdd(startPos, __result);
            cachedTimes.SetOrAdd(startPos, Find.TickManager.TicksGame);
        }
    }

    static bool Prefix(ref Pawn pawn, ref IntVec3 root, ref float radius, ref Func<Pawn, IntVec3, IntVec3, bool> validator, ref Danger maxDanger, ref IntVec3 __result, ref bool __state)
    {
        //Log.Message("Hello from RandomWanderDestFor Prefix");
        if (pawn.training == null)
        {
            //Log.Message("Hello from RandomWanderDestFor Original - no Training");
            return true;
        }
        IntVec3 startPos = pawn.Position;
        __state = false;
        if (cachedTimes.TryGetValue(startPos, int.MaxValue) < Find.TickManager.TicksGame + cacheTTL)
        {
            IntVec3 cachedValue = cachedDests.TryGetValue(startPos, IntVec3.Invalid);
            if (cachedValue != IntVec3.Invalid)
            {
                __result = cachedValue;
                __state = true;
                return false;
            }
        }
        Log.Message("Hello from RandomWanderDestFor Original");
        return true;
    }
}
*/