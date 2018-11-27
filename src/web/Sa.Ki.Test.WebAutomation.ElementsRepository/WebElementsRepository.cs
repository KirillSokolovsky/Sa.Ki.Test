namespace Sa.Ki.Test.WebAutomation.ElementsRepository
{
    using Sa.Ki.Test.SakiTree;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Linq;

    public class WebElementsRepository
    {
        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            TypeNameHandling = TypeNameHandling.All,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        };

        public List<CombinedWebElementInfo> WebElements { get; set; }
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
                WebElements = new List<CombinedWebElementInfo>();
                return;
            }

            try
            {
                var json = File.ReadAllText(_storageFilePath);
                WebElements = JsonConvert.DeserializeObject<List<CombinedWebElementInfo>>(json, JsonSerializerSettings);
                PreapareAfterLoad(WebElements);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        public void Save()
        {
            if (!_storageFileInfo.Exists)
            {
                _storageFileInfo.Directory.Create();
            }

            try
            {
                PrepareBeforeSave(WebElements);
                var json = JsonConvert.SerializeObject(WebElements, JsonSerializerSettings);
                File.WriteAllText(_storageFilePath, json);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        private void PreapareAfterLoad(List<CombinedWebElementInfo> elements)
        {
            if (elements == null) return;
            foreach (var el in elements)
            {
                if (el is FrameWebElementInfo frame)
                {
                    var toCopy = (WebElementInfo)WebElements.FindNodeByTreePath(frame.TreePathToInnerElement);
                    var copy = toCopy.GetCopyWithoutParent();
                    copy.Parent = frame;
                    frame.InnerElement = copy;
                    frame.Elements = new List<WebElementInfo>();
                    frame.Elements.Add(copy);
                }
                else if (el is WebElementReference refer)
                {
                    var toCopy = (WebElementInfo)WebElements.FindNodeByTreePath(refer.TreePathToReferencedElement);
                    var copy = toCopy.GetCopyWithoutParent();

                    copy.Parent = refer;
                    refer.ReferencedElement = copy;
                    refer.Elements = new List<WebElementInfo>();
                    
                    if(copy is CombinedWebElementInfo cmb)
                    {
                        if(cmb.Elements != null)
                        {
                            foreach (var c in cmb.Elements)
                            {
                                c.Parent = refer;
                                refer.Elements.Add(c);
                            }
                        }
                    }
                }
                else if (el is CombinedWebElementInfo combined)
                {
                    var children = combined.Elements?.OfType<CombinedWebElementInfo>().ToList();
                    if (children != null)
                        PreapareAfterLoad(children);
                }
            }
        }
        private void PrepareBeforeSave(List<CombinedWebElementInfo> elements)
        {
            if (elements == null) return;
            foreach (var el in elements)
            {
                if (el is FrameWebElementInfo frame)
                {
                    frame.Elements?.Clear();
                    frame.InnerElement.Parent = null;
                    frame.InnerElement = null;
                }
                else if (el is WebElementReference refer)
                {
                    refer.Elements?.Clear();
                    refer.ReferencedElement.Parent = null;
                    refer.ReferencedElement = null;
                }
                else if (el is CombinedWebElementInfo combined)
                {
                    var children = combined.Elements?.OfType<CombinedWebElementInfo>().ToList();
                    if (children != null)
                        PrepareBeforeSave(children);
                }
            }
        }
    }
}
