#if UNITY_EDITOR

using com.GWLPXL.Helpers.JsonSaving;
using UnityEngine;

[UnityEditor.CustomEditor(typeof(ActionFlowDataContainer))]
public class ActionFlowDataContainerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        ActionFlowDataContainer so = (ActionFlowDataContainer)target;
       
       
        if (GUILayout.Button("Save to file"))
        {
            string json = JsonUtility.ToJson(so);
            ResourceManager.SaveJsonFile(json, "Assets", so.name);
            UnityEditor.EditorUtility.SetDirty(so);

        }

        serializedObject.ApplyModifiedProperties();
    }
}


#endif
