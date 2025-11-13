using Il2CppAssets.Scripts.Inventory__Items__Pickups.Items;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoRestart
{
    public static class Config
    {
        public static bool _isValid { get; private set; } = false;
        public static List<string> _requiredItems { get; private set; } = new();
        public static bool _debugLogging { get; private set; } = false;

        public static void Setup()
        {
            var category = MelonPreferences.CreateCategory("AutoRestart");

            var requiredItemNamesEntry = category.CreateEntry("RequiredItemNames", "SoulHarvester,CreditCardGreen", "Required Items", "Comma-separated list of item names to search for.");
            var debugLoggingEntry = category.CreateEntry("DebugLogging", false, "Enable Debug Logging", "Set to true to see detailed logs in the console.");
            
            _debugLogging = debugLoggingEntry.Value;
            string rawItemNames = requiredItemNamesEntry.Value;

            if (string.IsNullOrWhiteSpace(rawItemNames))
            {
                ModLogger.Error("Config error: 'RequiredItemNames' is empty in AutoRestart.cfg.");
                _isValid = false;
                return;
            }
            
            RequiredItems = rawItemNames.Split(',')
                .Select(item => item.Trim())
                .Where(item => !string.IsNullOrEmpty(item))
                .ToList();
                
            if (RequiredItems.Count == 0)
            {
                ModLogger.Error("Config error: No valid items found after parsing 'RequiredItemNames'.");
                IsValid = false;
                return;
            }

            var allGameItemNames = new HashSet<string>(Enum.GetNames(typeof(EItem)), StringComparer.OrdinalIgnoreCase);
            var invalidItems = RequiredItems.Where(reqItem => !allGameItemNames.Contains(reqItem)).ToList();
            
            if (invalidItems.Count > 0)
            {
                ModLogger.Error($"Config error: Invalid item name(s) found: [{string.Join(", ", invalidItems)}]. Please check item spellings.");
                IsValid = false;
                return;
            }

            IsValid = true;
        }
    }
}