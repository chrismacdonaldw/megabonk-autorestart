using MelonLoader;
using Il2Cpp;
using Il2CppAssets.Scripts.Managers;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[assembly: MelonInfo(typeof(AutoRestart.AutoRestart), "AutoRestart", "1.0.0", "cmac")]
[assembly: MelonGame("Ved", "Megabonk")]

namespace AutoRestart
{
    public class AutoRestart : MelonMod
    {
        private const string TARGET_SCENE_NAME = "GeneratedMap";
        private const float TIMEOUT_SECONDS = 5.0f;
        private const float SCENE_SETUP_DELAY = 0.5f;

        private bool _isRunCheckActive = false;
        private bool _hasFoundRequiredItems = false;
        private int _totalShadyGuysInScene = 0;
        private readonly HashSet<InteractableShadyGuy> _readyShadyGuys = new();
        private Coroutine _timeoutCoroutine;

        public override void OnInitializeMelon()
        {
            Config.Setup();
            if (!Config.IsValid)
            {
                ModLogger.Error("Configuration is invalid. The AutoRestart mod will be disabled.");
                return;
            }
            
            Patches.OnShadyGuyReady += OnShadyGuyReady;
            HarmonyInstance.PatchAll();
            
            ModLogger.Info($"AutoRestart Initialized. Searching for: [{string.Join(", ", Config.RequiredItems)}]");
        }
        
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            int currentStage = MapController.GetStageIndex();
            bool isTargetStage = sceneName.Equals(TARGET_SCENE_NAME, StringComparison.OrdinalIgnoreCase) && (currentStage == 0);
            
            if (!isTargetStage)
            {
                _isRunCheckActive = false;
                if (_timeoutCoroutine != null) MelonCoroutines.Stop(_timeoutCoroutine);
                return;
            }

            ModLogger.Debug("Entered target stage. Resetting...");
            
            _readyShadyGuys.Clear();
            _hasFoundRequiredItems = false;
            _totalShadyGuysInScene = 0;
            
            if (_timeoutCoroutine != null) MelonCoroutines.Stop(_timeoutCoroutine);
            MelonCoroutines.Start(SceneSetupCoroutine());
        }

        private System.Collections.IEnumerator SceneSetupCoroutine()
        {
            // Wait to let stuff spawn.
            yield return new WaitForSeconds(SCENE_SETUP_DELAY);

            var allShadyGuys = UnityEngine.Object.FindObjectsByType<InteractableShadyGuy>(FindObjectsSortMode.None);
            _totalShadyGuysInScene = allShadyGuys?.Length ?? 0;
            _isRunCheckActive = true;

            if (_totalShadyGuysInScene == 0)
            {
                RestartRun("No Shady Guys found after setup delay.");
                yield break;
            }

            ModLogger.Debug($"Found {_totalShadyGuysInScene} Shady Guy(s). Waiting for them to become ready...");
            _timeoutCoroutine = (Coroutine)MelonCoroutines.Start(RestartAfterTimeout());
        }

        private System.Collections.IEnumerator RestartAfterTimeout()
        {
            yield return new WaitForSeconds(TIMEOUT_SECONDS);
            if (_isRunCheckActive && !_hasFoundRequiredItems)
            {
                RestartRun($"Timeout of {TIMEOUT_SECONDS}s reached. Required items not found.");
            }
        }

        private void OnShadyGuyReady(InteractableShadyGuy shadyGuy)
        {
            if (!_isRunCheckActive || _hasFoundRequiredItems) return;
            if (!_readyShadyGuys.Add(shadyGuy)) return;
            
            List<string> itemNames = new List<string>();
            foreach (var item in shadyGuy.items)
            {
                itemNames.Add(item.name);
            }
            ModLogger.Debug($"A Shady Guy is ready. ({_readyShadyGuys.Count}/{_totalShadyGuysInScene} ready). Items: [{string.Join(", ", itemNames)}]");

            CheckShadyGuyInventories();
        }

        private void CheckShadyGuyInventories()
        {
            if (HasAllRequiredItems())
            {
                ModLogger.Info("Success! All required items found. This seed is valid.");
                _hasFoundRequiredItems = true; 
                _isRunCheckActive = false; 
                if (_timeoutCoroutine != null) MelonCoroutines.Stop(_timeoutCoroutine);
            }
            else if (_readyShadyGuys.Count >= _totalShadyGuysInScene)
            {
                if (_timeoutCoroutine != null) MelonCoroutines.Stop(_timeoutCoroutine);
                RestartRun($"All {_totalShadyGuysInScene} shadyGuys are ready, but required items not found.");
            }
        }

        private bool HasAllRequiredItems()
        {
            var itemToShadyGuysMap = new Dictionary<string, List<InteractableShadyGuy>>(StringComparer.OrdinalIgnoreCase);
            foreach (var shadyGuy in _readyShadyGuys)
            {
                foreach (var item in shadyGuy.items)
                {
                    if (!itemToShadyGuysMap.TryGetValue(item.name, out var shadyGuyList))
                    {
                        shadyGuyList = new List<InteractableShadyGuy>();
                        itemToShadyGuysMap[item.name] = shadyGuyList;
                    }
                    shadyGuyList.Add(shadyGuy);
                }
            }

            foreach (var requiredItemName in Config.RequiredItems)
            {
                if (!itemToShadyGuysMap.ContainsKey(requiredItemName)) return false;
            }

            return CanFindUniqueShadyGuyAssignment(Config.RequiredItems.ToList(), new HashSet<InteractableShadyGuy>(), itemToShadyGuysMap);
        }

        private bool CanFindUniqueShadyGuyAssignment(
            List<string> remainingItems,
            HashSet<InteractableShadyGuy> usedShadyGuys,
            Dictionary<string, List<InteractableShadyGuy>> itemToShadyGuysMap)
        {
            if (!remainingItems.Any()) return true;

            var currentItem = remainingItems[0];
            var nextItems = remainingItems.Skip(1).ToList();
            
            foreach (var shadyGuy in itemToShadyGuysMap[currentItem])
            {
                if (usedShadyGuys.Contains(shadyGuy)) continue;

                usedShadyGuys.Add(shadyGuy);
                if (CanFindUniqueShadyGuyAssignment(nextItems, usedShadyGuys, itemToShadyGuysMap))
                {
                    return true;
                }

                usedShadyGuys.Remove(shadyGuy);
            }

            return false;
        }

        private void RestartRun(string reason)
        {
            ModLogger.Debug($"{reason} Restarting run...");
            _isRunCheckActive = false;
            MapController.RestartRun();
        }
    }
}
