#if UNITY_EDITOR


namespace GWLPXL.Movement.Character.CC.com
{

    [UnityEditor.CustomEditor(typeof(CharacterControllerSO))]
    public class CharacterControllerSOEditor : UnityEditor.Editor
    {
        int index = 0;
        string[] options = new string[0];
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //CharacterControllerSO so = (CharacterControllerSO)target;
            //if (so.PlayerCC.Controls.FlowControls != null)
            //{
            //    options = ResourceManager.AssignChoices(so.PlayerCC.Controls.FlowControls.FlowData);
            //    index = UnityEditor.EditorGUILayout.Popup(index, options);
            //}

        }
    }

}

#endif

