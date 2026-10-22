using UnityEngine;

namespace YourNewFriend
{
    public static class SaveSystem
    {
        // Keys that only exist once the player has actually made progress.
        private const string _checkpointKey = "POS_ID";
        private const string _inventoryKey = "InventorySave";

        /// <summary>True if there is a previous game the player can continue.</summary>
        public static bool HasSave()
        {
            return PlayerPrefs.HasKey(_checkpointKey) || PlayerPrefs.HasKey(_inventoryKey);
        }

        /// <summary>Wipes all saved progress and runtime state so the next scene load starts from scratch.</summary>
        public static void StartNewGame()
        {
            // Audio settings are player preferences, not game progress: keep them across a new game.
            float musicVolume = PlayerPrefs.GetFloat(AudioManager.MusicVolumeKey, 1f);
            float sfxVolume = PlayerPrefs.GetFloat(AudioManager.SFXVolumeKey, 1f);

            PlayerPrefs.DeleteAll();

            PlayerPrefs.SetFloat(AudioManager.MusicVolumeKey, musicVolume);
            PlayerPrefs.SetFloat(AudioManager.SFXVolumeKey, sfxVolume);
            PlayerPrefs.Save();
            ResetRuntimeState();
            Debug.Log("SaveSystem: New game started, save data wiped.");
        }

        /// <summary>Prepares to continue from the last save (scene objects read PlayerPrefs on Awake/Start).</summary>
        public static void PrepareLoadGame()
        {
            ResetRuntimeState();
            Debug.Log("SaveSystem: Loading last save.");
        }

        /// <summary>
        /// Static fields survive scene loads, so they must be reset by hand.
        /// Otherwise values (and event listeners) from the previous session leak into the new one.
        /// </summary>
        private static void ResetRuntimeState()
        {
            DB_Building._buildCount = 0;
            DB_Building.condition = false;
            WireSwitch.ResetPuzzle();
            Cover.onOpenCover = null;
            Cover.Player = null;
            Chase.isPlayerSeen = false;
            MusicTriggerZone.mus_overlap = false;
        }
    }
}
