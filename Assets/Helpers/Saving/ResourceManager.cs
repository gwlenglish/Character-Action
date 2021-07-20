using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.GWLPXL.Helpers.JsonSaving
{

    /// <summary>
    /// static class for loading and saving data in json using the interface
    /// </summary>
    public static class ResourceManager
    {
        const string jsonextension = ".json";
        public static string ReadFromJson(UnityEngine.Object ob)
        {
            string json = string.Empty;
#if UNITY_EDITOR
            string savepath = UnityEditor.AssetDatabase.GetAssetPath(ob);

            using (System.IO.FileStream fs = new System.IO.FileStream(savepath, System.IO.FileMode.Open))
            {
                using (System.IO.StreamReader writer = new System.IO.StreamReader(fs))
                {
                    json = writer.ReadToEnd();
                }
            }
#endif
            return json;
        }
        public static void SaveJsonFile(string json, string path, string name)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(path);
            sb.Append("/");
            sb.Append(name);
            sb.Append(jsonextension);

            string savepath = sb.ToString();
            Debug.Log(savepath);
            using (System.IO.FileStream fs = new System.IO.FileStream(savepath, System.IO.FileMode.Create))
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fs))
                {
                    writer.Write(json);
                }
            }

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        public static string[] AssignChoices(IResourceContainer container)
        {
            List<JsonData> data = new List<JsonData>();
            List<GUIDResource> resources = container.GetResources();

            for (int i = 0; i < resources.Count; i++)
            {
                for (int j = 0; j < resources[i].JsonData.Count; j++)
                {
                    data.Add(resources[i].JsonData[j]);
                }

            }

            string[] keys = new string[data.Count];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = data[i].Key;
            }
            string[] flows = keys;
            return flows;
        }

        public static string GetJsonRead(string key, IResourceContainer container, long file)
        {
            List<GUIDResource> resources = container.GetResources();
            for (int i = 0; i < resources.Count; i++)
            {
                if (file == resources[i].FileID)
                {
                    //found it
                    GUIDResource resource = resources[i];
                    for (int j = 0; j < resource.JsonData.Count; j++)
                    {
                        if (string.CompareOrdinal(resource.JsonData[j].Key, key) == 0)
                        {
                            return resource.JsonData[i].JsonRead;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public static Object GetAsset(string key, IResourceContainer container)
        {
            List<GUIDResource> resources = container.GetResources();
            for (int i = 0; i < resources.Count; i++)
            {
                List<JsonData> data = resources[i].JsonData;
                for (int j = 0; j < data.Count; j++)
                {
                    if (string.CompareOrdinal(key, data[j].Key) == 0)
                    {
                        //found it
#if UNITY_EDITOR
                        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(resources[i].UniqueGUID);
                        Object ob = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(Object));
                        return ob;
#endif
                    }
                }

            }
            return null;
        }

        public static string GetJsonRead(string key, IResourceContainer container)
        {
            List<GUIDResource> resources = container.GetResources();
            for (int i = 0; i < resources.Count; i++)
            {
                List<JsonData> data = resources[i].JsonData;
                for (int j = 0; j < data.Count; j++)
                {
                    if (string.CompareOrdinal(key, data[j].Key) == 0)
                    {
                        //found it
                        return data[j].JsonRead;
                    }
                }

            }
            return string.Empty;
        }


        public static void SaveToResources(string json, string key, IResourceContainer resources, string guid, long file)
        {
            //string str = JsonUtility.ToJson(so, false);
            //string key = so.FlowName;
            bool foundjson = false;
            bool foundguid = false;
            List<GUIDResource> re = resources.GetResources();
            for (int i = 0; i < re.Count; i++)
            {
                if (string.CompareOrdinal(guid, re[i].UniqueGUID) == 0)
                {
                    //found guid but not json
                    foundguid = true;
                    List<JsonData> _temp = re[i].JsonData;
                    for (int j = 0; j < _temp.Count; j++)
                    {
                        if (string.CompareOrdinal(key, _temp[j].Key) == 0)
                        {
                            _temp[j].JsonRead = json;
                            foundjson = true;
                        }
                    }
                    if (foundjson == false)
                    {
                        JsonData jsondata = new JsonData(key, json);
                        _temp.Add(jsondata);
                    }

                }

            }

            if (foundguid == false)
            {
                GUIDResource resource = new GUIDResource(guid, file, new List<JsonData>());
                re.Add(resource);
                JsonData jsondata = new JsonData(key, json);
                resource.JsonData.Add(jsondata);
            }

            resources.SetResources(re);
        }
        public static void SaveToResourcesByFile(string json, string key, IResourceContainer resources, string guid, long file)
        {
            //string str = JsonUtility.ToJson(so, false);
            //string key = so.FlowName;
            bool foundjson = false;
            bool foundguid = false;
            List<GUIDResource> re = resources.GetResources();
            for (int i = 0; i < re.Count; i++)
            {
                if (file == re[i].FileID)
                {
                    //found guid but not json
                    foundguid = true;
                    List<JsonData> _temp = re[i].JsonData;
                    for (int j = 0; j < _temp.Count; j++)
                    {
                        if (string.CompareOrdinal(key, _temp[j].Key) == 0)
                        {
                            _temp[i].JsonRead = json;
                            foundjson = true;
                        }
                    }
                    if (foundjson == false)
                    {
                        JsonData jsondata = new JsonData(key, json);
                        _temp.Add(jsondata);
                    }
                }


            }

            if (foundguid == false)
            {
                GUIDResource resource = new GUIDResource(guid, file, new List<JsonData>());
                re.Add(resource);
                JsonData jsondata = new JsonData(key, json);
                resource.JsonData.Add(jsondata);
            }


            resources.SetResources(re);


        }

       

    }
}
