using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using System.Collections;
using System.Reflection;
using UnityEngine;
using static R2API.SoundAPI;

namespace NowYouFuckedUp
{

    //This attribute specifies that we have a dependency on R2API, as we're using it to add our item to the game.
    //You don't need this if you're not using R2API in your plugin, it's just to tell BepInEx to initialize R2API before this plugin so it's safe to use R2API.
    [BepInDependency(R2API.R2API.PluginGUID)]
	
	//This attribute is required, and lists metadata for your plugin.
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	
	//We will be using 2 modules from R2API: ItemAPI to add our item and LanguageAPI to add our language tokens.
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI), nameof(SoundAPI))]
	
	//This is the main declaration of our plugin class. BepInEx searches for all classes inheriting from BaseUnityPlugin to initialize on startup.
    //BaseUnityPlugin itself inherits from MonoBehaviour, so you can use this as a reference for what you can declare and use in your plugin class: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
    public class NowYouFuckedUpClass : BaseUnityPlugin
	{
        //The Plugin GUID should be a unique ID for this plugin, which is human readable (as it is used in places like the config).
        //If we see this PluginGUID as it is on thunderstore, we will deprecate this mod. Change the PluginAuthor and the PluginName !
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Zylvian";
        public const string PluginName = "FuckedUp";
        public const string PluginVersion = "1.0.0";
        public static PluginInfo PInfo { get; private set; }

        //We need our item definition to persist through our functions, and therefore make it a class field.
        //private static ItemDef myItemDef;
        private static uint soundId = 1351821836;

        //private uint currEvent;

        //The Awake() method is run at the very start when the game is initialized.
        public void Awake()
        {
            PInfo = Info;

            Log.Init(Logger);

            using (var bankStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NowYouFuckedUp.FuckedUpBank.bnk"))
            {
                var bytes = new byte[bankStream.Length];
                bankStream.Read(bytes, 0, bytes.Length);
                SoundBanks.Add(bytes);
            }


            // On Lemurian Spawn

            //On.EntityStates.Wisp1Monster.SpawnState.OnEnter += (orig, self) =>
            On.EntityStates.LemurianBruiserMonster.SpawnState.OnEnter += (orig, self) =>
            {

                // Monster game object
                GameObject go = self.gameObject;

                // Play this looping soundbank until death
                uint eventID = AkSoundEngine.PostEvent(soundId, go);
                Log.LogInfo("Play sound x when LemurianBruiserMonster spawns. ID: " + eventID.ToString());


                GlobalEventManager.onCharacterDeathGlobal += (report) =>{
                    if (report.victim.gameObject == go)
                    {
                        Log.LogInfo("Stop playing sound when monster dies.");
                        AkSoundEngine.StopPlayingID(eventID);
                    }
                };

                RoR2.Run.onRunDestroyGlobal += (run) =>
                {
                    Log.LogInfo("Stop playing sound when game unloads.");
                    AkSoundEngine.StopPlayingID(eventID);
                };

                orig(self);

            };

        }

    }
}
