using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.com;
namespace GWLPXL.Movement.Character.CC.com
{

    /// <summary>
    /// a way not to the distance but hook up to the extra movements?
    /// </summary>
    public class PlayerInstancePreviewCC : MonoBehaviour
    {
        public PlayerInstanceCC Instance;
        public int PreviewIndex = 0;
        public KeyCode PreviewKey = KeyCode.F1;
        Dictionary<CharacterController, GameObject> previewDic = new Dictionary<CharacterController, GameObject>();

        private void OnEnable()
        {
            ActionManager.OnActionComplete += RemovePreview;
        }
        private void OnDisable()
        {
            ActionManager.OnActionComplete -= RemovePreview;
        }
        private void Update()
        {
            if (Input.GetKeyDown(PreviewKey))
            {
                CreatePreview();
            }
        }
        void CreatePreview()
        {
            if (PreviewIndex > Instance.Character.PlayerCC.Controls.Actions.Length - 1)
            {
                Debug.LogWarning("Illegal preview");
                return;
            }
            PlayerInstanceCC newPreview = Instantiate(Instance);
            CharacterControllerSO copy = Instantiate(newPreview.Character);
            newPreview.Character = copy;
            newPreview.MakeScriptedSequenceClone(copy);
            ActionManager.StartFlowSequence(newPreview.Controller, newPreview.Character.PlayerCC, PreviewIndex, newPreview.Animator);
            previewDic[newPreview.Controller] = newPreview.gameObject;

        }

        void RemovePreview(CharacterController controller)
        {
            if (previewDic.ContainsKey(controller))
            {
                Destroy(previewDic[controller]);
                previewDic.Remove(controller);
            }
        }
    }
}