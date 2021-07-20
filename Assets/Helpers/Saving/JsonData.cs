
namespace com.GWLPXL.Helpers.JsonSaving
{

    /// <summary>
    /// json data class
    /// </summary>
    [System.Serializable]
    public class JsonData
    {
        public string Key;
        public string JsonRead;
        public JsonData(string key, string json)
        {
            Key = key;
            JsonRead = json;
        }
    }
}


