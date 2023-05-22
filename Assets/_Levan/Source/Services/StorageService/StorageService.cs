using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LevanPangInterview.Services
{
    public class StorageService : Service
    {
        //////// Consts

        public const string KEY_PREFIX   = "Pang_";
        public const string MANIFEST_KEY = "Storage_Manifest";

        //////// Members

        public StorageManifest manifest;

        //////// Lifecycle

        public override async Task OnSystemInit()
        {
            if (PlayerPrefs.HasKey(MANIFEST_KEY))
            {
                LoadManifest();
            }
            else
            {
                manifest = new StorageManifest();
                manifest.SavedKeys.Add(MANIFEST_KEY);
                SaveManifest();
            }
        }

        //////// API

        public bool StoreItem(string Key, object item)
        {
            try
            {
                Logger.Log($"StorageService : Storing Item : key = {Key}, Item = {item}");

                bool KeyAlreadyExisted = PlayerPrefs.HasKey(KEY_PREFIX + Key);
    
                // Store Item
                var json = Serialize(item);
                PlayerPrefs.SetString(KEY_PREFIX + Key, json);

                // Update manifest
                if (!KeyAlreadyExisted )
                {
                    manifest.SavedKeys.Add( KEY_PREFIX + Key );
                    SaveManifest();
                }

                // Done
                Logger.Log($"StorageService : Done Storing. \nItem = {json}");

                return true; // success
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return false; // error
            }
        }

        public bool HasKey(string Key)
        {
            return PlayerPrefs.HasKey(KEY_PREFIX + Key);
        }

        public T LoadItem<T>(string Key)
        {
            try
            {
                Logger.Log($"StorageService : Loading Item : key = {Key}");

                if (!PlayerPrefs.HasKey(KEY_PREFIX + Key))
                    // throw new Exception("Storage.LoadItem() : No such key saved in PlayerPrefs");
                    return default;

                // Load Item
                var json = PlayerPrefs.GetString(KEY_PREFIX + Key);
                var item = Deserialize<T>(json);

                // Done
                Logger.Log($"StorageService : Done Loading. \nItem = {json}");

                return item;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return default;
            }
        }

        public void DeleteItem(string Key)
        {
            try
            {
                Logger.Log($"StorageService : Deleting Item : key = {Key}");

                if (!PlayerPrefs.HasKey(KEY_PREFIX + Key))
                    throw new Exception("Storage.DeleteItem() : No such key saved in PlayerPrefs");

                // Delete Item
                PlayerPrefs.DeleteKey(KEY_PREFIX + Key);

                // Update Manifest
                manifest.SavedKeys.Remove(KEY_PREFIX + Key);
                SaveManifest();

                // Done
                Logger.Log($"StorageService : Done Deleting");

            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        //////// Utility Methods

        public string Serialize(object Source)
        {
            string result = "";

            try
            {
                result = JsonConvert.SerializeObject(Source);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }

            return result;
        }

        public T Deserialize<T>(string Source)
        {
            try
            {
                T value = JsonConvert.DeserializeObject<T>(Source);
                return value;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        //////// Helper Methods
        
        protected void SaveManifest()
        {
            try
            {
                Debug.Log($"StorageService : Updating Manifest");

                // Save Manifest
                var json = Serialize(manifest);
                PlayerPrefs.SetString(MANIFEST_KEY, json);

                // Done
                Debug.Log($"StorageService : Done Updating Manifest");
            }
            catch (Exception ex) 
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        public void LoadManifest()
        {
            try
            {
                var json      = PlayerPrefs.GetString(MANIFEST_KEY);
                this.manifest = Deserialize<StorageManifest>(json);
            }
            catch (Exception ex) 
            {
                Logger.LogError("StorageService : Error Loading Manifest :");
                Logger.LogError(ex.Message);
                throw;
            }
        }

        //////// Quick Test Methods
        
        #if UNITY_EDITOR


        [ContextMenu("SaveRandomString_Test")]
        protected void SaveRandomString()
        {
            this.StoreItem("random_string_test_item", "RandomString");
            Debug.Log("Saved");
        }

        [ContextMenu("LoadRandomString_Test")]
        protected void LoadRandomString()
        {
            var str = LoadItem<string>("random_string_test_item");
            Debug.Log("Loaded : " + str);
        }

        [ContextMenu("DeleteRandomString_Test")]
        protected void DeleteRandomString()
        {
            DeleteItem("random_string_test_item");
            Debug.Log("deleted. Has Key = " + PlayerPrefs.HasKey(KEY_PREFIX + "random_string_test_item"));
        }

        [ContextMenu("Clear_Player_Prefs")]
        protected void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Deleted All");
        }

        #endif
    }

    //////// Helper Class

    [Serializable]
    public class StorageManifest
    {
        public List<string> SavedKeys = new List<string>();
    }
}