using System.Collections.Generic;

namespace com.GWLPXL.Helpers.JsonSaving
{

    /// <summary>
    /// interface for saving data as json
    /// </summary>
    public interface IResourceContainer
    {

        List<GUIDResource> GetResources();
        void SetResources(List<GUIDResource> value);

    }



}