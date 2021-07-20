#if UNITY_EDITOR
using com.GWLPXL.Helpers.JsonSaving;
using UnityEngine;
using UnityEditor;
[UnityEditor.CustomEditor(typeof(ActionsDataContainerSO))]
public class AbilitiesDataContainerEditor : UnityEditor.Editor
{

        public override void OnInspectorGUI()
        {
        serializedObject.Update();
        base.OnInspectorGUI();
       
            ActionsDataContainerSO so = (ActionsDataContainerSO)target;


            if (GUILayout.Button("Save to file"))
            {
                string json = JsonUtility.ToJson(so);
                ResourceManager.SaveJsonFile(json, "Assets", so.name);

            }

        serializedObject.ApplyModifiedProperties();
        }


    }


#endif
