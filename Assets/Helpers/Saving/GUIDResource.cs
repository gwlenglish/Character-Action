using System.Collections.Generic;

namespace com.GWLPXL.Helpers.JsonSaving
{
    /// <summary>
    /// used to save data in json format
    /// </summary>
    [System.Serializable]
    public class GUIDResource
    {
        public string UniqueGUID = string.Empty;
        public long FileID = 0;
        public List<JsonData> JsonData = new List<JsonData>();
        public GUIDResource(string guid, long file, List<JsonData> data)
        {
            UniqueGUID = guid;
            FileID = file;
            JsonData = data;
        }
    }


}
