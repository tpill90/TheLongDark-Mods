using Harmony;

namespace DeveloperConsole {

    [HarmonyLib.HarmonyPatch(typeof(uConsoleGUI), "InputFieldClearText")]
    internal static class ScrollDownOnCommandPatch {

        private static void Postfix() {
            uConsole.m_GUI.ScrollLogDownMax();
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(uConsoleAutoComplete), "DisplayPossibleMatches")]
    internal static class ScrollDownOnAutoCompletePatch {

        private static void Postfix() {
            uConsole.m_GUI.ScrollLogDownMax();
        }
    }
}
