
using com.GWLPXL.Helpers.JsonSaving;
using System.Collections.Generic;
using UnityEngine;




/// <summary>
/// much like the flow one
/// </summary>
/// 
[CreateAssetMenu(menuName = "TestAbilitiesDataContainer")]
    public class ActionsDataContainerSO : ScriptableObject, IResourceContainer
    {
        [SerializeField]
        List<GUIDResource> resources = new List<GUIDResource>();



    public List<GUIDResource> GetResources() => resources;

    public void SetResources(List<GUIDResource> value)
    {
        resources = value;
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);

#endif
    }
}

#if UNITY_EDITOR


#endif
