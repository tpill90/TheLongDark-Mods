using Object = UnityEngine.Object;

namespace DeveloperConsole 
{
    [HarmonyPatch]
    internal static class InitializePatch 
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BootUpdate), nameof(BootUpdate.Start))]
        public static void SetupKeybind() 
        {
            MelonLogger.Msg("Setting up keybind");
            Object.Instantiate(Resources.Load("uConsole"));
            uConsole.m_Instance.m_Activate = KeyCode.F1;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SaveGameSystem), nameof(SaveGameSystem.LoadSceneData))]
        public static void LoadSceneData(ref string name)
        {
            MelonLogger.Msg("Setting up keybind 2");
            Object.Instantiate(Resources.Load("uConsole"));
            uConsole.m_Instance.m_Activate = KeyCode.F1;
        }
    }
}
