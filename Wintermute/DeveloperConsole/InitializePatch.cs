﻿using Harmony;
using UnityEngine;

namespace DeveloperConsole {

    [HarmonyPatch(typeof(BootUpdate), "Start")]
    internal static class InitializePatch {

        private static void Prefix() {
            Object.Instantiate(Resources.Load("uConsole"));
            uConsole.m_Instance.m_Activate = KeyCode.F1;
        }
    }
}
