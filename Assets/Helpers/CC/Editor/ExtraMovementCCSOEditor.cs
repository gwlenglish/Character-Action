using com.GWLPXL.Helpers.JsonSaving;
using UnityEngine;

namespace GWLPXL.Movement.Character.CC.com
{
#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(CharacterActionsCCSO))]
    public class ExtraMovementCCSOEditor : UnityEditor.Editor
    {
        int index = 0;
        UnityEditor.SerializedProperty resourcesData;
        private void OnEnable()
        {
            resourcesData = serializedObject.FindProperty("Resources");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            CharacterActionsCCSO so = (CharacterActionsCCSO)target;
            UnityEditor.EditorGUILayout.PropertyField(resourcesData);
            
            if (so.Resources == null)
            {
                serializedObject.ApplyModifiedProperties();
                return;
            }

            base.OnInspectorGUI();//base implementation, maybe later re-write
            string guid;
            long file;
            UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(so, out guid, out file);
            IResourceContainer resources = so.Resources as IResourceContainer;
            string[] flows = ResourceManager.AssignChoices(resources);
            index = UnityEditor.EditorGUILayout.Popup(index, flows);
            if (GUILayout.Button("Save"))
            {
                string str = JsonUtility.ToJson(so, false);
                string key = so.ScriptedNames;
                ResourceManager.SaveToResources(str, key, resources, guid, file);

            }

            if (GUILayout.Button("Load"))
            {
                string json = ResourceManager.GetJsonRead(flows[index], resources);

                if (string.IsNullOrEmpty(json) == false)
                {
                    ActionsDataContainerSO container = so.Resources;
                    JsonUtility.FromJsonOverwrite(json, so);
                    so.Resources = container;
                }
                else
                {
                    Debug.LogWarning("Did not find load file in resource container");
                }

            }

            serializedObject.ApplyModifiedProperties();
        }

      
    }

#endif
}
