
#if UNITY_EDITOR
using com.GWLPXL.Helpers.JsonSaving;
using UnityEditor;
using UnityEngine;

namespace GWLPXL.Movement.Character.com
{

    [UnityEditor.CustomEditor(typeof(SequenceSO))]
    public class SequenceEditor : UnityEditor.Editor
    {
        UnityEditor.SerializedProperty flowdata;
        UnityEditor.SerializedProperty actionData;
        UnityEditor.SerializedProperty flows;
        private void OnEnable()
        {
            flowdata = serializedObject.FindProperty("FlowData");
            actionData = serializedObject.FindProperty("ActionData");
            flows = serializedObject.FindProperty("Flows");
        }
      
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SequenceSO so = (SequenceSO)target;
            EditorGUILayout.PropertyField(flowdata);
            EditorGUILayout.PropertyField(actionData);
            EditorGUILayout.PropertyField(flows);
            if (so.FlowData == null || so.ActionData == null)
            {
                serializedObject.ApplyModifiedProperties();
                return;
            }
            string guid;
            long file;
            UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(so, out guid, out file);
            IResourceContainer resources = so.FlowData as IResourceContainer;
            string[] flowsnames = ResourceManager.AssignChoices(resources);

            so.FlowName = UnityEditor.EditorGUILayout.TextField(so.FlowName);
            so.flowCount = UnityEditor.EditorGUILayout.IntField(so.flowCount);
            so.loaded = UnityEditor.EditorGUILayout.Popup(so.loaded, flowsnames);

            if (GUILayout.Button("Save"))
            {
                string str = JsonUtility.ToJson(so, false);
                string key = so.FlowName;

                ResourceManager.SaveToResources(str, key, resources, guid, file);

            }

            if (GUILayout.Button("Load"))
            {
                string json = ResourceManager.GetJsonRead(flowsnames[so.loaded], resources);
                if (string.IsNullOrEmpty(json) == false)
                {
                    Debug.Log("Loaded");
                    ActionFlowDataContainer flow = so.FlowData;
                    ActionsDataContainerSO actions = so.ActionData;
                    JsonUtility.FromJsonOverwrite(json, so);
                    so.FlowData = flow;
                    so.ActionData = actions;
                }
                else
                {
                    Debug.LogWarning("Did not find load file");
                }
            }

            if (so.flowCount != so.previousFlowCount)
            {
                while (so.Flows.Count < so.flowCount)
                {
                    so.Flows.Add(new Flow());
                }

                while (so.Flows.Count > so.flowCount)
                {
                    so.Flows.RemoveAt(so.Flows.Count - 1);
                }
                so.previousFlowCount = so.flowCount;
            }

            IResourceContainer actionr = so.ActionData as IResourceContainer;
            for (int i = 0; i < so.Flows.Count; i++)
            {
                Flow flow = so.Flows[i];

                string[] moves = ResourceManager.AssignChoices(actionr);

                UnityEditor.EditorGUILayout.LabelField("Flow Phases");

                flow.combophasecount = UnityEditor.EditorGUILayout.IntField(flow.combophasecount);
                if (flow.combophasecount == 0)
                {
                    flow.ComboPhase.Clear();
                    flow.previousphasecount = 0;
                    return;
                }
                if (flow.combophasecount != flow.previousphasecount)
                {
                    while (flow.ComboPhase.Count < flow.combophasecount)
                    {
                        flow.ComboPhase.Add(new ComboPhase());
                    }
                    while (flow.ComboPhase.Count > flow.combophasecount)
                    {
                        flow.ComboPhase.RemoveAt(flow.ComboPhase.Count - 1);
                    }
                    flow.previousphasecount = flow.combophasecount;
                }

                for (int j = 0; j < flow.combophasecount; j++)
                {

                    ComboPhase phase = flow.ComboPhase[j];
                    string label = phase.Start.MoveAbility;
                    if (string.IsNullOrEmpty(label))
                    {
                        label = "Combo Phase";
                    }

                    phase.Foldout = UnityEditor.EditorGUILayout.Foldout(phase.Foldout, label);
                    if (phase.Foldout)
                    {
                        UnityEditor.EditorGUILayout.LabelField("Initial Move");
                        phase.index = UnityEditor.EditorGUILayout.Popup(phase.index, moves);
                        ComboPhaseEditor editor = phase.Vars;
                        Debug.Log(editor);
                        phase.Start.MoveAbility = moves[phase.index];
                        UpdateNextField(phase, editor, moves);
                        //UpdateCancelField(phase, editor, moves);
                    }

                }


               
            }


         


            serializedObject.ApplyModifiedProperties();



        }


        private void UpdateCancelField(ComboPhase so, ComboPhaseEditor editor, string[] moves)
        {
            UnityEditor.EditorGUILayout.LabelField("Cancels");
            UnityEditor.EditorGUILayout.Space(5);
            UnityEditor.EditorGUILayout.LabelField("Type?");


            editor.Cancelcount = UnityEditor.EditorGUILayout.IntField(editor.Cancelcount);

            if (editor.Cancelcount <= 0)
            {
                editor.Cancels.Clear();

                editor.Cancelcount = 0;
                editor.PreviousCancelcount = 0;
                return;
            }





        }

        private void UpdateNextField(ComboPhase so, ComboPhaseEditor editor, string[] moves)
        {
            UnityEditor.EditorGUILayout.LabelField("Next");
            UnityEditor.EditorGUILayout.Space(5);
            UnityEditor.EditorGUILayout.LabelField("Any?");
            so.NextType = (ValidSequenceType)UnityEditor.EditorGUILayout.EnumPopup(so.NextType);
            editor.Nextcount = UnityEditor.EditorGUILayout.IntField(editor.Nextcount);
            if (editor.Nextcount <= 0)
            {
                editor.Nexts.Clear();
                so.Next.Clear();
                editor.Nextcount = 0;
                return;
            }

            if (editor.Nextcount != editor.Previousnextcount)
            {
                while (editor.Nexts.Count < editor.Nextcount)
                {
                    editor.Nexts.Add(0);
                    so.Next.Add(new Sequence());
                }

                while (editor.Nexts.Count > editor.Nextcount)
                {
                    editor.Nexts.RemoveAt(editor.Nexts.Count - 1);
                    so.Next.RemoveAt(so.Next.Count - 1);
                }


                editor.Previousnextcount = editor.Nextcount;
            }

            UnityEditor.EditorGUILayout.BeginVertical();

            switch (so.NextType)
            {
                case ValidSequenceType.SelectedOnly:

                    for (int i = 0; i < editor.Nextcount; i++)
                    {
                        editor.Nexts[i] = UnityEditor.EditorGUILayout.Popup(editor.Nexts[i], moves);
                        so.Next[i].MoveAbility = moves[editor.Nexts[i]];
                    }
                    break;

            }
            UnityEditor.EditorGUILayout.EndVertical();


        }
    }


}
#endif
