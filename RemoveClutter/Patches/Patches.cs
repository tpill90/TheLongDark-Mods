namespace RemoveClutter.Patches
{
    [HarmonyPatch]
    public static class Patches2
    {
        /// <summary>
        /// Shows right click option when hovering over removable item
        /// </summary
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Panel_HUD), nameof(Panel_HUD.SetHoverText))]
        public static void ShowButtonPrompts(ref GameObject itemUnderCrosshairs)
        {
            if (!IsRemovable(itemUnderCrosshairs))
            {
                return;
            }

            var panelHud = InterfaceManager.GetPanel<Panel_HUD>();
            panelHud.m_EquipItemPopup.enabled = true;
            panelHud.m_EquipItemPopup.ShowGenericPopupWithDefaultActions("**Remove**", "Remove");
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(InputManager), nameof(InputManager.ExecuteAltFire))]
        public static void HandleRightClickOnRemovableObject()
        {
            if (!GameManager.GetPlayerManagerComponent())
            {
                return;
            }

            GameObject gameObject = GetGameObjectUnderCrosshair();
            if (!IsRemovable(gameObject))
            {
                return;
            }

            MelonCoroutines.Start(RemoveClutter.RemoveObject(gameObject));
        }

        public static GameObject GetGameObjectUnderCrosshair()
        {
            PlayerManager pm = GameManager.GetPlayerManagerComponent();

            float maxPickupRange = GameManager.GetGlobalParameters().m_MaxPickupRange;
            float maxRange = pm.ComputeModifiedPickupRange(maxPickupRange);
            if (pm.GetControlMode() == PlayerControlMode.InFPCinematic)
            {
                maxRange = 50f;
            }

            return GameManager.GetPlayerManagerComponent().GetInteractiveObjectUnderCrosshairs(maxRange);
        }

        public static bool IsRemovable(GameObject targetedObject)
        {
            if (targetedObject != null)
            {
                if (targetedObject.name.Contains("xpzclutter"))
                {
                    return true;
                }
                if (targetedObject.transform.parent.name.Contains("xpzclutter"))
                {
                    return true;
                }
            }
            if (targetedObject != null && targetedObject.transform.parent.name.Contains("xpzclutter"))
            {
                return true;
            }
            if (targetedObject != null && targetedObject.name.Contains("Plate"))
            {
                return true;
            }

            if (targetedObject != null && targetedObject.GetComponent<Container>() == null)
            {
                targetedObject = targetedObject.transform.GetParent()?.gameObject;
            }

            if (!targetedObject || targetedObject.GetComponent<Container>() == null || !targetedObject.GetComponent<Container>().m_IsCorpse)
            {
                return false;
            }
            return true;
        }
    }
}
