// ReSharper disable InconsistentNaming - Harmony patches require multiple underscores to determine the right variable
namespace FasterActions
{
    [HarmonyPatch]
    public static class Patches
    {
        private static readonly MelonLogger.Instance Logger = Melon<FasterActions>.Logger;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CharcoalItem), nameof(CharcoalItem.StartDetailSurvey))]
        public static void FasterCharcoalMapping(CharcoalItem __instance)
        {
            // How long it takes to survey in real world seconds.  Default is 3 seconds
            __instance.m_SurveyRealSeconds = FasterActions.Settings.CharcoalMappingSeconds;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FireManager), nameof(FireManager.PlayerCalculateFireStartTime))]
        public static void FasterFireStarting(ref float __result)
        {
            // Speeds up the action wheel for starting fires by X amount
            __result = __result / FasterActions.Settings.FireStartSpeedup;
        }

        #region Food + Drink

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FoodItem), nameof(FoodItem.Awake))]
        public static void FasterEating(FoodItem __instance)
        {
            // Cuts time to eat in half
            __instance.m_TimeToEatSeconds = __instance.m_TimeToEatSeconds / FasterActions.Settings.EatingSpeedup;
            __instance.m_TimeToOpenAndEatSeconds = __instance.m_TimeToOpenAndEatSeconds / FasterActions.Settings.EatingSpeedup;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.OpenAndUseFoodInventoryItem))]
        public static void FasterCanOpening(GearItem gi, CanOpeningItem gearOpenedWith)
        {
            gearOpenedWith.m_CanOpeningLengthSeconds = FasterActions.Settings.CanOpeningSeconds;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.DrinkFromWaterSupply))]
        public static void FasterDrinking(WaterSupply ws, ItemLiquidVolume volumeAvailable)
        {
            ws.m_TimeToDrinkSeconds = FasterActions.Settings.DrinkingSpeedSeconds;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Panel_Cooking), nameof(Panel_Cooking.OnCook))]
        public static void FasterRecipePrepping(Panel_Cooking __instance)
        {
            __instance.m_RecipePreparationDisplayTimeSeconds = FasterActions.Settings.RecipePrepSpeed;
        }

        #endregion

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Panel_Crafting), nameof(Panel_Crafting.CraftingStart))]
        public static void FasterCrafting(Panel_Crafting __instance)
        {
            __instance.m_CraftingDisplayTimeSeconds = FasterActions.Settings.CraftingTimeSeconds;
        }

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.OnSelectCleanTool))]
        //public static void FasterCleaning(Panel_Inventory_Examine __instance)
        //{
        //    __instance.m_CleanTimeSeconds = 1.5f;
        //}

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.OnSelectSharpenTool))]
        public static void FasterSharpening(Panel_Inventory_Examine __instance)
        {
            Logger.Msg($"Default sharpen time : {__instance.m_SharpenTimeSeconds}");
            __instance.m_SharpenTimeSeconds = FasterActions.Settings.SharpeningTimeSeconds;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.OnRepair))]
        public static void FasterRepair(Panel_Inventory_Examine __instance)
        {
            Logger.Msg($"Default repair time : {__instance.m_RepairTimeSeconds}");
            __instance.m_RepairTimeSeconds = FasterActions.Settings.RepairTimeSeconds;
        }

        // This is for the harvesting/breakdown from inside the player's inventory.  Not for harvesting mushrooms
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.OnHarvest))]
        public static void FasterHarvest(Panel_Inventory_Examine __instance)
        {
            __instance.m_HarvestTimeSeconds = FasterActions.Settings.HarvestTimeSeconds;
        }

        // This is for breaking down world items, like furniture + junk.
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Panel_BreakDown), nameof(Panel_BreakDown.OnBreakDown))]
        public static void FasterBreakdown(Panel_BreakDown __instance)
        {
            Logger.Msg($"Default breakdown time : {__instance.m_SecondsToBreakDown}");
            __instance.m_SecondsToBreakDown = FasterActions.Settings.BreakdownTimeSeconds;
        }
    }
}