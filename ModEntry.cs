using System;
using CarolineMarriage.Framework.CarolineMarriage.Framework;
using Shared.Shared;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace CarolineMarriage
{
    internal class ModEntry : Mod
    {/// <summary>The mod entry point, called after the mod is first loaded.</summary>
     /// <param name="helper">Provides simplified APIs for writing mods.</param>
     /// <summary>The mod instance.</summary>
     public static ModEntry Instance { get; private set; }
        public ModConfig Config { get; private set; }

        public override void Entry(IModHelper helper)
        {
            IModEvents events = helper.Events;
            Instance = this;
            Log.Monitor = this.Monitor;
            Config = helper.ReadConfig<ModConfig>();

            HarmonyPatcher.Apply(
                this,
                new TemporaryAnimatedSpritePatcher(),
                new FarmerPatcher()
            );

            ConsoleCommandHelper.RegisterCommandsInAssembly(this);
        }
        public override void Entry(IModHelper helper)
        {
            throw new NotImplementedException();
        }
    }
}
