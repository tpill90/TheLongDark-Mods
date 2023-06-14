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
        public static MelonLogger.Instance Logger = Melon<FasterActions>.Logger;
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FireManager), nameof(FireManager.PlayerCalculateFireStartTime))]
        public static void FasterFireStarting(ref float duration)
        {
            // Speeds up the action wheel for starting fires by 4X
            duration = duration / 4;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FoodItem), nameof(FoodItem.Awake))]
        public static void FasterEating(FoodItem __instance)
        {
            __instance.m_TimeToEatSeconds = __instance.m_TimeToEatSeconds / 2;
            __instance.m_TimeToOpenAndEatSeconds = __instance.m_TimeToOpenAndEatSeconds / 2;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.OpenAndUseFoodInventoryItem))]
        public static void FasterCanOpening(GearItem gi, CanOpeningItem gearOpenedWith)
        {
            gearOpenedWith.m_CanOpeningLengthSeconds = 1.4f;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.DrinkFromWaterSupply))]
        public static void FasterDrinking(WaterSupply ws, float volumeAvailable)
        {
            ws.m_TimeToDrinkSeconds = 1.4f;
        }

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.OnHarvest))]
        //public static void FasterHarvesting(Panel_Inventory_Examine __instance)
        //{
        //    Logger.Warning(nameof(FasterHarvesting) + " triggered");
        //    __instance.m_HarvestTimeSeconds = 1.5f;
        //}

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.OnSelectCleanTool))]
        //public static void FasterCleaning(Panel_Inventory_Examine __instance)
        //{
        //    __instance.m_CleanTimeSeconds = 1.5f;
        //}

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.OnSelectSharpenTool))]
        //public static void FasterSharpening(Panel_Inventory_Examine __instance)
        //{
        //    __instance.m_SharpenTimeSeconds = 1.5f;
        //}

        //TODO messes up stuff for some reason.  Creates stuttering
        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.OnRepair))]
        //public static void FasterRepair(Panel_Inventory_Examine __instance)
        //{
        //    Logger.Warning(nameof(FasterRepair) + " triggered");
        //    __instance.m_RepairTimeSeconds = 1.5f;
        //}

        //TODO messes up stuff for some reason.  Creates stuttering
        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(Panel_BreakDown), nameof(Panel_BreakDown.OnBreakDown))]
        //public static void FasterBreakdown(Panel_BreakDown __instance)
        //{
        //    Logger.Warning(nameof(FasterBreakdown) + " triggered");
        //    __instance.m_SecondsToBreakDown = 1.5f;
        //}
    }
}