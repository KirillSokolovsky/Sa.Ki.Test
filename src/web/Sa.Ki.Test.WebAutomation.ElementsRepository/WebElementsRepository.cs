namespace Sa.Ki.Test.WebAutomation.ElementsRepository
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    public class WebElementsRepository
    {
        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            TypeNameHandling = TypeNameHandling.All,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        };

        public List<WebContextInfo> WebContexts { get; set; }
        private string _storageFilePath;
        private FileInfo _storageFileInfo;


        public WebElementsRepository(string storageFilePath)
        {
            _storageFilePath = storageFilePath;
            _storageFileInfo = new FileInfo(_storageFilePath);
        }

        public void Load()
        {
            if (!_storageFileInfo.Exists)
            {
                WebContexts = new List<WebContextInfo>();
                return;
            }

            try
            {
                var json = File.ReadAllText(_storageFilePath);
                WebContexts = JsonConvert.DeserializeObject<List<WebContextInfo>>(json, JsonSerializerSettings);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        public void Save()
        {
            if(!_storageFileInfo.Exists)
            {
                _storageFileInfo.Directory.Create();
            }

            try
            {
                var json = JsonConvert.SerializeObject(WebContexts, JsonSerializerSettings);
                File.WriteAllText(_storageFilePath, json);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }
    }
}
