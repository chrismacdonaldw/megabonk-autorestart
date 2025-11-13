using HarmonyLib;
using Il2Cpp;
using System;

namespace AutoRestart
{
    public static class Patches
    {
        public static event Action<InteractableShadyGuy> _onShadyGuyReady;

        [HarmonyPatch(typeof(InteractableShadyGuy), nameof(InteractableShadyGuy.FindItems))]
        public static class ShadyGuyFindItemsPatch
        {
            [HarmonyPostfix]
            public static void Postfix(InteractableShadyGuy __instance)
            {
                onShadyGuyReady?.Invoke(__instance);
            }
        }
    }
}