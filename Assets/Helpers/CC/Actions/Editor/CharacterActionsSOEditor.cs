#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GWLPXL.Movement.Character.com;
using UnityEditorInternal;

namespace GWLPXL.Movement.Character.CC.com
{
  
    [CustomEditor(typeof(CharacterActionsCCSO))]
    public class CharacterActionsSOEditor : Editor
    {
        string[] tabs = new string[10] { "Original", "Input", "Requirements", "Blocking", "Multipliers", "Movement","Animator", "InputBuffers", "Extend Options", "Early Exit Options" };
        int tab;


        SerializedProperty movements;
        SerializedProperty requirements;
        SerializedProperty input;
        SerializedProperty scripted;
        SerializedProperty movementsfield;
        SerializedProperty movementbehavior;
        SerializedProperty movementSequence;
        List<string> readonlynamelist = new List<string>();

        private void OnEnable()
        {
            
            movements = serializedObject.FindProperty("Movement");
            movementsfield = movements.FindPropertyRelative("Movements");
            scripted = movementsfield.FindPropertyRelative("ScriptedMovement");
            input = scripted.FindPropertyRelative("InputRequirements");
            requirements = scripted.FindPropertyRelative("CharacterRequirements");
            movementbehavior = scripted.FindPropertyRelative("MovementBehavior");
            movementSequence = movementbehavior.FindPropertyRelative("MovementSequence");

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            CharacterActionsCCSO so = (CharacterActionsCCSO)target;

   

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.LabelField(so.Movement.Movements.ScriptedMovement.Name, EditorStyles.boldLabel);

            GUILayout.Space(10);

            EditorGUILayout.EndHorizontal();

            tab = GUILayout.SelectionGrid(tab, tabs, 4);

            readonlynamelist.Clear();
            ActionCCVars[] varsarr = so.Movement.Movements.ScriptedMovement.MovementBehavior.MovementSequence;
            switch (tab)
            {
                case 0:
                    base.OnInspectorGUI();
                    break;
                case 1:
                    InputRequirements inputclass = so.Movement.Movements.ScriptedMovement.InputRequirements;
                    
                    for (int i = 0; i < inputclass.InputButtons.Length; i++)
                    {
                        readonlynamelist.Add(inputclass.InputButtons[i].ButtonName);
                    }
                    for (int i = 0; i < inputclass.AxisRequirements.Length; i++)
                    {
                        readonlynamelist.Add(inputclass.AxisRequirements[i].RequirementType.ToString());
                    }
                    ReadonlyHelpBox("Current Input, READONLY",readonlynamelist);

                    EditorGUILayout.PropertyField(input);

                    break;
                case 2:
                    //movement
                    CharacterRequirements chrequire = so.Movement.Movements.ScriptedMovement.CharacterRequirements;
                    for (int i = 0; i < chrequire.RequiredStates.Length; i++)
                    {
                        readonlynamelist.Add(chrequire.RequiredStates[i].ToString());
                    }

                    ReadonlyHelpBox("Current Requirements, READONLY", readonlynamelist);

                    EditorGUILayout.PropertyField(requirements);
                    break;

      
                case 3:

                    for (int i = 0; i < varsarr.Length; i++)
                    {

                        BlockingOptions blocking = varsarr[i].BlockingOptions;

                        EditorGUILayout.LabelField("Blocking Options for Sequence " + i);
                        EditorGUI.indentLevel++;

                        for (int j = 0; j < blocking.Blocking.Count; j++)
                        {
                            EditorGUI.BeginChangeCheck();
                            var layersSelection = EditorGUILayout.MaskField("Layers", LayerMaskToField(blocking.Blocking[j].BlockingLayers), InternalEditorUtility.layers);
                         
                            blocking.Blocking[j].Direction = EditorGUILayout.Vector3Field("Direction",blocking.Blocking[i].Direction);
                            blocking.Blocking[j].DistanceCheck = EditorGUILayout.FloatField(blocking.Blocking[i].DistanceCheck);
                            if (EditorGUI.EndChangeCheck())
                            {
                                Undo.RecordObject(so, "Layers changed");
                                blocking.Blocking[j].BlockingLayers = FieldToLayerMask(layersSelection);
    
                            }

                        }
                        EditorGUI.indentLevel--;

                    }
                   

                    
                    break;
                case 4:
                    //multies

                    for (int i = 0; i < varsarr.Length; i++)
                    {
                        EditorGUILayout.LabelField("Multiplier Options for Sequence " + i);
                        SerializedProperty prop = movementSequence.GetArrayElementAtIndex(i);
                        DrawProperty(so, prop, "Multipliers");

                    }

                    break;
                case 5:
                    //movement
                    for (int i = 0; i < varsarr.Length; i++)
                    {
                        EditorGUILayout.LabelField("Movement Options for Sequence " + i);
                        SerializedProperty prop = movementSequence.GetArrayElementAtIndex(i);
                        DrawProperty(so, prop, "TravelX");
                        DrawProperty(so, prop, "TravelY");
                        DrawProperty(so, prop, "TravelZ");

                    }
                  


                    break;
                case 6:
                    //anim
                    for (int i = 0; i < varsarr.Length; i++)
                    {
                        EditorGUILayout.LabelField("Animator Options for Sequence " + i);
                        SerializedProperty prop = movementSequence.GetArrayElementAtIndex(i);
                        DrawProperty(so, prop, "AnimatorVars");

                    }

                    break;
                case 7:
                    //input buffers
                    for (int i = 0; i < varsarr.Length; i++)
                    {
                        EditorGUILayout.LabelField("Input Buffers for Sequence " + i);
                        SerializedProperty prop = movementSequence.GetArrayElementAtIndex(i);
                        DrawProperty(so, prop, "BufferVars");
                    }


                    break;
                case 8:
                    //extend buffers
                    for (int i = 0; i < varsarr.Length; i++)
                    {
                        EditorGUILayout.LabelField("Extend Options for Sequence " + i);
                        SerializedProperty prop = movementSequence.GetArrayElementAtIndex(i);
                        DrawProperty(so, prop, "ExtendOptions");
                    }


                    break;
                case 9:
                    //early exit
                    for (int i = 0; i < varsarr.Length; i++)
                    {
                        EditorGUILayout.LabelField("Early Exit Options for Sequence " + i);
                        SerializedProperty prop = movementSequence.GetArrayElementAtIndex(i);
                        DrawProperty(so, prop, "EarlyExitOptions");
                    }


                    break;
            }

      


            serializedObject.ApplyModifiedProperties();
        }

        // Converts a LayerMask to a field value
        private int LayerMaskToField(LayerMask mask)
        {
            int field = 0;
            var layers = InternalEditorUtility.layers;
            for (int c = 0; c < layers.Length; c++)
            {
                if ((mask & (1 << LayerMask.NameToLayer(layers[c]))) != 0)
                {
                    field |= 1 << c;
                }
            }
            return field;
        }

        private LayerMask FieldToLayerMask(int field)
        {
            LayerMask mask = 0;
            var layers = InternalEditorUtility.layers;
            for (int c = 0; c < layers.Length; c++)
            {
                if ((field & (1 << c)) != 0)
                {
                    mask |= 1 << LayerMask.NameToLayer(layers[c]);
                }
            }
            return mask;
        }

        private static void DrawProperty(ScriptableObject target, SerializedProperty prop, string propertyname)
        {
            SerializedProperty multiinside = prop.FindPropertyRelative(propertyname);
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(multiinside);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, propertyname + "Changed");
            }
            EditorGUI.indentLevel--;
        }

        private void ReadonlyHelpBox(string helpLabel, List<string> names)
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10);
            EditorGUILayout.LabelField(helpLabel, EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            for (int i = 0; i < names.Count; i++)
            {
                EditorGUILayout.LabelField(names[i], EditorStyles.boldLabel);

            }
           
            EditorGUI.indentLevel--;
            GUILayout.Space(10);
            EditorGUILayout.EndVertical();
        }
    }
}
#endif