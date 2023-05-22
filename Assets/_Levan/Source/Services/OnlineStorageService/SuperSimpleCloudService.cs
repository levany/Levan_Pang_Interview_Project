using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevanPangInterview.Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace LevanPangInterview.Services
{
    public class SuperSimpleCloudService : Service
    {   
        //////// Documentation


        /// Using a Free Json torage for small prototypes.
        /// Website URL : https://jsonbin.io/
        /// API Doc URL : https://jsonbin.io/api-reference/bins/read

        //////// Consts
        
        /// JSONBIN.IO
        const string BIN_ID       = "6468b0119d312622a361ba45";
        const string BIN_URL      = "https://api.jsonbin.io/v3/b/6468b0119d312622a361ba45";
        const string API_KEY      = "$2b$10$e6hFjXjAgJvCsbJiUaedH.dLUNrM4CVUVwxx5v6/Nc14xCGZxyBTK ";
        const string API_URL_BASE = "https://api.jsonbin.io/v3";
        const string READ_URL     = API_URL_BASE + "/b/"+BIN_ID; // Get Request
        const string Write_URL    = API_URL_BASE + "/b/"+BIN_ID; // Put Request
        const string TEST_KEY     = "Name";
        const string TEST_VALUE   = "Levan";

        //// Members
        
        [SerializeField] private SuperSimpleCloudService.Data data    = new Data();
        [SerializeField] private BinData                      binData = new BinData();
        
        //// Properties

        public bool IsServiceEnabledd
        {
            get => Link.AppSettings.Use_Simple_Cloud_Service;
            set => Link.AppSettings.Use_Simple_Cloud_Service = value;
        }

        //// Public Properties

        public string LastError = null;

        //// Lifecycle

        public override async Task OnSystemInit()
        {
            if (!IsServiceEnabledd)
                return;

            this.data.Items = new();

            await ReadData();
        }


        //// API
        
        public T GetItem<T>() where T : Model
        {
            try
            {
                if (!IsServiceEnabledd)
                    return default;

                if (!IsServiceEnabledd) return null;
                if (LastError == null)  return null;

                var item = (T)this.data.Items.First(x => x is T);

                return item;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return default;
            }
        }

        public void SetItem(Model item)
        {
            try
            {
                if (!IsServiceEnabledd)
                    return;

                if (this.data.Items.Contains(item))
                    this.data.Items.Remove(item);

                this.data.Items.Add(item);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return;
            }
        }

        public async Task<bool> Sync()
        {
            try
            {
                if (!IsServiceEnabledd)
                    return true;

                bool isSuccess = await this.WriteData();

                return isSuccess;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }


        //// I/O Methods

        public async Task<bool> WriteData()
        {
            if (!IsServiceEnabledd)
                return true;

            try
            {
                Logger.Log($"Write Data Start");

                // Definitions
                bool isSuccess;

                // Serialize item
                string JsonBody = JsonConvert.SerializeObject(this.data);

                Logger.Log($"Write Data Json : {JsonBody}");

                // Create request
                using UnityWebRequest writeRequest = UnityWebRequest.Put(Write_URL ,JsonBody);
                writeRequest.timeout         = 10;
                writeRequest.SetRequestHeader("Content-Type",  "application/json");
                writeRequest.SetRequestHeader("X-Master-Key",  API_KEY);

                var writeOP = writeRequest.SendWebRequest();

                Logger.Log($"Write Request Send");

                while (!writeOP.isDone)
                {
                    //Logger.Log("Writing..." + Time.timeSinceLevelLoad);
                    await Task.Yield();
                }

                Logger.Log($"Write Request Done");

                this.LastError = writeRequest.error;
                isSuccess      = (LastError is null);
            
                Logger.Log($"Write LastError == null : {LastError == null}");

                Logger.Log($"Write Result : " + writeRequest.downloadHandler.text);
                Logger.Log($"Write Error  : " + LastError ?? "NO ERROR");

                return isSuccess;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> ReadData()
        {
            if (!IsServiceEnabledd)
                return true;

            try
            {
                Logger.Log($"Read Data Start");

                bool isSuccess;

                // Create web reqeuest
                using UnityWebRequest readRequest = UnityWebRequest.Get(READ_URL);
                readRequest.timeout = 3;
                readRequest.SetRequestHeader("Content-Type", "application/json");
                readRequest.SetRequestHeader("X-Master-Key", API_KEY);

                // Send request
                var readOP = readRequest.SendWebRequest();

                Logger.Log($"Read Data Sent");

                // await request
                while (!readOP.isDone)
                {
                    //Logger.Log("Reading..." + Time.timeSinceLevelLoad);
                    await Task.Yield();
                }

                Logger.Log($"Read Data FinishedSent");

                // Check for errors
                this.LastError = readOP.webRequest.error;
                isSuccess = (LastError is null);

                Logger.Log($"Last error null : {LastError == null}");

                Logger.Log($"read result Result : " + readRequest.downloadHandler.text);

                if (!isSuccess)
                    return false;

                // deserialize
                var json    = readRequest.downloadHandler.text;
                var binData = JsonConvert.DeserializeObject<BinData>(json);

                Logger.Log($"Read JSON : {json}");

                this.data    = binData.data;
                this.binData = binData;

                return true;

            }
            catch (Exception ex)
            {
                Logger.LogError($"CloudService.Read() : {ex.Message}");
                return false;
            }
        }

        ////////////////////////////////////////////// Helper types

        [Serializable]
        public class BinData
        {
            public Data     data     = new Data();
            public Metadata metadata = new Metadata();

            public override string ToString()
            =>
                $"Root:" +
                $"\n{data}" +
                $"\n{metadata}";
        }
        [Serializable]
        public class Metadata
        {
            public string   id        = "0";
            public bool     @private  = true;
            public DateTime createdAt = DateTime.UnixEpoch;
            public string   name      = "Name";

            public override string ToString()
            =>
                $"MetaData:" +
                $"\nid={id}" +
                $"\nprivate={@private}" +
                $"\ncreatedAt={createdAt}" +
                $"\nname={name}";
        }
        [Serializable]
        public class Data
        {
            public List<Model> Items = new();

            public override string ToString()
            =>
                $"Record:" +
                $"\nitem=\n{JsonConvert.SerializeObject(Items)}";
        }

        ////////////////////////////////////////
        

        #if UNITY_EDITOR

        [ContextMenu("WriteDataToCloud")]
        protected void WriteDataToCloud()
        {
            this.WriteData();
            Debug.Log("Done");
        }

        #endif

    }
}

