using com.GWLPXL.Helpers.JsonSaving;
using GWLPXL.Movement.Character.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// scriptable object container for the flow data
/// </summary>
[CreateAssetMenu(menuName ="TestFlowDataContainer")]
public class ActionFlowDataContainer : ScriptableObject, IResourceContainer
{

    public List<GUIDResource> Resources = new List<GUIDResource>();
    public ActionsDataContainerSO Abilities = null;

    public List<GUIDResource> GetResources() => Resources;

    public void SetResources(List<GUIDResource> value)
    {
        Resources = value;
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}


