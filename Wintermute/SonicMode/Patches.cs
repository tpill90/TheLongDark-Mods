namespace SonicMode
{
    internal class SonicMode : MelonMod
    {
        public override void OnApplicationStart()
        {
            Settings.Instance.AddToModSettings("Sonic Mode");
            MelonLogger.Msg($" Version {Info.Version} loaded!");
        }
    }

    [HarmonyPatch]
    public static class Patches
    {
        private static bool first_run = true;
        private static float initial_recovery = 0f;
        private static float initial_seconds_before_recovery = 0f;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(vp_FPSController), nameof(vp_FPSController.GetSlopeMultiplier))]
        private static void IncreaseSpeed(ref float __result)
        {
            if (GameManager.GetPlayerManagerComponent().PlayerIsSprinting())
            {
                __result *= Settings.Instance.sprintSpeedMultiplier;
            }
            else if (GameManager.GetPlayerManagerComponent().PlayerIsCrouched())
            {
                __result *= Settings.Instance.crouchSpeedMultiplier;
            }
            else
            {
                __result *= Settings.Instance.walkSpeedMultiplier;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerMovement), nameof(PlayerMovement.Update))]
        private static void IncreaseStamina(PlayerMovement __instance)
        {
            if (first_run)
            {
                initial_recovery = __instance.m_SprintStaminaRecoverPerHour;
                initial_seconds_before_recovery = __instance.m_SecondsNotSprintingBeforeRecovery;
                first_run = false;
            }
            __instance.m_SprintStaminaRecoverPerHour = initial_recovery * Settings.Instance.rechargeScalar;
            __instance.m_SecondsNotSprintingBeforeRecovery = initial_seconds_before_recovery * Settings.Instance.secondsScalar;
        }
    }
}
