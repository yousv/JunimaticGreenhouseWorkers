using System;
using StardewModdingAPI;

namespace Yousv.JunimaticGreenhouseWorkers
{
    public interface IGenericModConfigMenuApi
    {
        void Register(IManifest mod, Action reset, Action save, bool titleScreenOnly);
        void AddSectionTitle(IManifest mod, Func<string> text, Func<string> tooltip = null);
        void AddBoolOption(IManifest mod, Func<bool> getValue, Action<bool> setValue, Func<string> name, Func<string> tooltip = null, string fieldId = null);
        void AddTextOption(IManifest mod, Func<string> getValue, Action<string> setValue, Func<string> name, Func<string> tooltip = null, string[] allowedValues = null, Func<string, string> formatAllowedValue = null, string fieldId = null);
        void AddNumberOption(IManifest mod, Func<int> getValue, Action<int> setValue, Func<string> name, Func<string> tooltip = null, int? min = null, int? max = null, int? interval = null, Func<int, string> formatValue = null, string fieldId = null);
    }

    internal class ModConfigMenu
    {
        public void Entry(ModEntry mod)
        {
            var api = mod.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (api is null)
                return;

            string T(string key) => mod.Helper.Translation.Get(key).ToString();

            api.Register(mod.ModManifest, () => Reset(mod), () => mod.Helper.WriteConfig(mod.Config), false);

            AddBool(api, mod,
                () => mod.Config.ModEnabled,
                v =>
                {
                    mod.Config.ModEnabled = v;
                    mod.InvalidateCutscenes();
                },
                T("config.mod-enabled.name"),
                T("config.mod-enabled.tooltip"));

            AddSection(api, mod, T("config.section.general"));
            AddBool(api, mod,
                () => mod.Config.Enabled,
                v =>
                {
                    mod.Config.Enabled = v;
                    mod.InvalidateCutscenes();
                },
                T("config.workers-enabled.name"),
                T("config.workers-enabled.tooltip"));
            api.AddTextOption(mod: mod.ModManifest,
                getValue: () => mod.Config.Unlock,
                setValue: v =>
                {
                    mod.Config.Unlock = v;
                    mod.InvalidateCutscenes();
                },
                name: () => T("config.unlock.name"),
                tooltip: () => T("config.unlock.tooltip"),
                allowedValues: ModConfig.UnlockOptions);
            api.AddNumberOption(mod: mod.ModManifest,
                getValue: () => mod.Config.JunimoSpeed,
                setValue: v => mod.Config.JunimoSpeed = v,
                name: () => T("config.junimo-speed.name"),
                tooltip: () => T("config.junimo-speed.tooltip"),
                min: 1, max: 10, interval: 1);
            AddBool(api, mod,
                () => mod.Config.AllowAllLocations,
                v => mod.Config.AllowAllLocations = v,
                T("config.allow-all-locations.name"),
                T("config.allow-all-locations.tooltip"));

            AddSection(api, mod, T("config.section.tasks"));
            AddBool(api, mod,
                () => mod.Config.PlantSeeds,
                v => mod.Config.PlantSeeds = v,
                T("config.plant-seeds.name"),
                T("config.plant-seeds.tooltip"));
            AddBool(api, mod,
                () => mod.Config.WaterCrops,
                v => mod.Config.WaterCrops = v,
                T("config.water-crops.name"),
                T("config.water-crops.tooltip"));
            AddBool(api, mod,
                () => mod.Config.FertilizeCrops,
                v => mod.Config.FertilizeCrops = v,
                T("config.fertilize-soil.name"),
                T("config.fertilize-soil.tooltip"));
            AddBool(api, mod,
                () => mod.Config.HarvestCrops,
                v => mod.Config.HarvestCrops = v,
                T("config.harvest-crops.name"),
                T("config.harvest-crops.tooltip"));
            AddBool(api, mod,
                () => mod.Config.WaterAfterPlanting,
                v => mod.Config.WaterAfterPlanting = v,
                T("config.water-after-planting.name"),
                T("config.water-after-planting.tooltip"));
            AddBool(api, mod,
                () => mod.Config.BulkCarry,
                v => mod.Config.BulkCarry = v,
                T("config.bulk-carry.name"),
                T("config.bulk-carry.tooltip"));
            AddBool(api, mod,
                () => mod.Config.AllowShipping,
                v => mod.Config.AllowShipping = v,
                T("config.allow-shipping.name"),
                T("config.allow-shipping.tooltip"));

            AddSection(api, mod, T("config.section.debug"));
            AddBool(api, mod,
                () => mod.Config.VerboseLogging,
                v => mod.Config.VerboseLogging = v,
                T("config.verbose-logging.name"),
                T("config.verbose-logging.tooltip"));
            AddBool(api, mod,
                () => mod.Config.ShowPathDebug,
                v => mod.Config.ShowPathDebug = v,
                T("config.show-path-debug.name"),
                T("config.show-path-debug.tooltip"));
            AddBool(api, mod,
                () => mod.Config.ShowTaskBubbles,
                v => mod.Config.ShowTaskBubbles = v,
                T("config.show-task-bubbles.name"),
                T("config.show-task-bubbles.tooltip"));
            AddBool(api, mod,
                () => mod.Config.NeverDropItems,
                v => mod.Config.NeverDropItems = v,
                T("config.never-drop-items.name"),
                T("config.never-drop-items.tooltip"));
        }

        private static void Reset(ModEntry mod)
        {
            var defaults = new ModConfig();
            mod.Config.ModEnabled = defaults.ModEnabled;
            mod.Config.Enabled = defaults.Enabled;
            mod.Config.ShowTaskBubbles = defaults.ShowTaskBubbles;
            mod.Config.Unlock = defaults.Unlock;
            mod.Config.PlantSeeds = defaults.PlantSeeds;
            mod.Config.WaterCrops = defaults.WaterCrops;
            mod.Config.FertilizeCrops = defaults.FertilizeCrops;
            mod.Config.HarvestCrops = defaults.HarvestCrops;
            mod.Config.WaterAfterPlanting = defaults.WaterAfterPlanting;
            mod.Config.JunimoSpeed = defaults.JunimoSpeed;
            mod.Config.BulkCarry = defaults.BulkCarry;
            mod.Config.AllowAllLocations = defaults.AllowAllLocations;
            mod.Config.WorkAroundVillagers = defaults.WorkAroundVillagers;
            mod.Config.AllowShipping = defaults.AllowShipping;
            mod.Config.VerboseLogging = defaults.VerboseLogging;
            mod.Config.ShowPathDebug = defaults.ShowPathDebug;
            mod.Config.NeverDropItems = defaults.NeverDropItems;
            mod.InvalidateCutscenes();
        }

        private static void AddSection(IGenericModConfigMenuApi api, ModEntry mod, string title)
            => api.AddSectionTitle(mod.ModManifest, () => title);

        private static void AddBool(IGenericModConfigMenuApi api, ModEntry mod, Func<bool> getValue, Action<bool> setValue, string name, string tooltip)
            => api.AddBoolOption(mod.ModManifest, getValue, setValue, () => name, () => tooltip);
    }
}
