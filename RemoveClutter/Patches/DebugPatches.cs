namespace RemoveClutter.Patches
{
    [HarmonyPatch]
    public static class DebugPatches
    {
        /// <summary>
        /// Debugging util to show what object is currently being pointed at with the cursor
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
        [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.UpdateUniStormDayLength))]
        public static void GetNameOfObjectUnderCrosshair(TimeOfDay __instance)
        {
            if (!InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.F4))
            {
                return;
            }

            vp_FPSCamera cam = GameManager.GetVpFPSPlayer().FPSCamera;
            RaycastHit raycastHit = Utils.DoRayCast(cam.transform.position, cam.transform.forward);

            GameObject gameObject = raycastHit.collider.gameObject;

            HUDMessage.AddMessage($"{gameObject.name} - {gameObject.transform.parent.name}");
            Logger2.Msg($"Object under crosshair:>>>>> {gameObject.name} - {gameObject.transform.parent.name}");
        }
    }
}
