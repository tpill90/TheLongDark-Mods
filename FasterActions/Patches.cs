namespace FasterActions
{
    internal class FasterActions : MelonMod
    {
        public override void OnApplicationStart()
        {
            Debug.Log($"[{InfoAttribute.Name}] version {InfoAttribute.Version} loaded!");
        }
    }

    //TODO comment
    //TODO readme
    [HarmonyPatch]
    public static class Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(CharcoalItem), nameof(CharcoalItem.StartDetailSurvey))]
        public static void PatchCharcoalMappingTime(CharcoalItem __instance)
        {
            // How much time passes in game while surveying.  Default is 15 minutes
            __instance.m_SurveyGameMinutes = 10;

            // How long it takes to survey in real world seconds.  Default is 3 seconds 
            __instance.m_SurveyRealSeconds = 1.5f;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FireManager), nameof(FireManager.PlayerCalculateFireStartTime))]
        public static void FasterFireStarting(ref float __result)
        {                
            // Speeds up the action wheel for starting fires by 3X
            __result = __result / 3;
        }

        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(Harvestable), "DoHarvest")]
        //public static void FasterHarvesting(Harvestable __instance)
        //{
        //    //__instance.m_SecondsToHarvest = Settings.options.HarvestTimeModifier;
        //}


        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(BreakDown), nameof(BreakDown.Start))]
        //public static void QuickerWoodCutting(BreakDown __instance)
        //{
        //    // Branches take 5 mins instead of 10
        //    if (__instance.name == "Branch")
        //    {
        //        __instance.m_TimeCostHours /= 2f;
        //    }

        //    // Limbs take 30 mins base (15 mins with hatchet) instead of 90 mins base (45 mins with hatchet)
        //    if (__instance.name.EndsWith("Limb"))
        //    {
        //        __instance.m_TimeCostHours /= 3f;
        //    }
        //}
    }
}