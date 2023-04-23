namespace EnableFeatProgressInCustomMode
{
    internal class EnableFeatProgressInCustomMode : MelonMod
    {
        public override void OnApplicationStart()
        {
            Debug.Log($"[{InfoAttribute.Name}] version {InfoAttribute.Version} loaded!");
        }
    }

    [HarmonyPatch]
    internal static class Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Feat), nameof(Feat.ShouldBlockIncrement))]
        public static void NeverBlockIncrement(ref bool __result)
        {
            __result = false;
        }
    }
}