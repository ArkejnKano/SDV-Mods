using System.Collections;
using System.IO;
using CarolineMarriage.Framework;
using Shared;
using Shared.APIs;
using Shared.ConsoleCommands;
using SharedPatching;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace CarolineMarriage
{
    internal class ModEntry : Mod
    {
        public bool IsEnabled { get; set; }
        /// <summary>The minimum version the host must have for the mod to be enabled on a farmhand.</summary>
        private readonly string MinHostVersion = "1.0";

        /// <summary>The mod instance.</summary>
        public static ModEntry Instance { get; private set; }

        public ModConfig Config { get; private set; }

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            IModEvents events = helper.Events;
            Instance = this;
            Config = helper.ReadConfig<ModConfig>();

            events.GameLoop.GameLaunched += this.OnGameLaunched;
            events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            events.GameLoop.SaveLoaded += this.OnSaveLoaded;

            HarmonyPatcher.Apply(
                this
                //,
                //new TemporaryAnimatedSpritePatcher(),
                //new FarmerPatcher()
            );

            ConsoleCommandHelper.RegisterCommandsInAssembly(this);
        }

        /// <summary>The event called after the first game update, once all mods are loaded.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            // get Wear More Rings API if present
            //this.WearMoreRings = this.Helper.ModRegistry.GetApi<IWearMoreRingsApi>("bcmpinc.WearMoreRings");
        }

        /// <summary>The event called when the game updates (roughly sixty times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!IsEnabled)
                return;
        }

        /// <summary>The event called after a save slot is loaded.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            // check if mod should be enabled for the current player
            IsEnabled = Context.IsMainPlayer;
            this.Monitor.Log("Oh shit we in business.", LogLevel.Info);
            if (!IsEnabled)
            {
                ISemanticVersion hostVersion = this.Helper.Multiplayer.GetConnectedPlayer(Game1.MasterPlayer.UniqueMultiplayerID)?.GetMod(this.ModManifest.UniqueID)?.Version;
                if (hostVersion == null)
                {
                    IsEnabled = false;
                    this.Monitor.Log("This mod is disabled because the host player doesn't have it installed.", LogLevel.Warn);
                }
                else if (hostVersion.IsOlderThan(this.MinHostVersion))
                {
                    IsEnabled = false;
                    this.Monitor.Log($"This mod is disabled because the host player has {this.ModManifest.Name} {hostVersion}, but the minimum compatible version is {this.MinHostVersion}.", LogLevel.Warn);
                }
                else
                    IsEnabled = true;
            }
        }

        /// <summary>Get whether the player has any ring with the given ID equipped.</summary>
        /// <param name="id">The ring ID to match.</param>
        public bool HasRingEquipped(int id)
        {
            return this.CountRingsEquipped(id) > 0;
        }

        public Hashtable ItemsAlreadyUpdated = new Hashtable();

        /// <summary>Count the number of rings with the given ID equipped by the player.</summary>
        /// <param name="id">The ring ID to match.</param>
        public int CountRingsEquipped(int id)
        {
            int count =
                (Game1.player.leftRing.Value?.GetEffectsOfRingMultiplier(id) ?? 0)
                + (Game1.player.rightRing.Value?.GetEffectsOfRingMultiplier(id) ?? 0);

            //if (this.WearMoreRings != null)
            //    count = Math.Max(count, this.WearMoreRings.CountEquippedRings(Game1.player, id));

            return count;
        }
    }
}