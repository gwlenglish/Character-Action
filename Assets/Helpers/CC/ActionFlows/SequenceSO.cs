using GWLPXL.Movement.Character.com;
using System.Collections.Generic;

using UnityEngine;

namespace GWLPXL.Movement.Character.com
{
    
    /// <summary>
    /// scriptable object container for the combo flows data
    /// </summary>
    [CreateAssetMenu(menuName = "TestSequence")]
    [System.Serializable]
    public class SequenceSO : ScriptableObject
    {
        [HideInInspector]
        public string Current = string.Empty;
        public ActionFlowDataContainer FlowData;
        public ActionsDataContainerSO ActionData;
        public int loaded = 0;
        public string FlowName = string.Empty;
        public int flowCount = 0;
        public int previousFlowCount = 0;
        public List<Flow> Flows = new List<Flow>();


        public int prev = 0;

        public List<string> GetNexts(string key)
        {
            ComboPhase phase = FindComboPhase();
            List<string> temp = new List<string>();
            if (phase == null) return temp;

            for (int i = 0; i < phase.Next.Count; i++)
            {
                temp.Add(phase.Next[i].MoveAbility);
            }
            return temp;

        }


        private ComboPhase FindComboPhase()
        {
            Debug.Log("Current " + Current);
            ComboPhase phase = null;
            for (int i = 0; i < Flows.Count; i++)
            {
                List<ComboPhase> combos = Flows[i].ComboPhase;
                for (int j = 0; j < combos.Count; j++)
                {
                    if (string.CompareOrdinal(Current, combos[j].Start.MoveAbility) == 0)
                    {
                        phase = combos[j];
                        break;
                    }
                }
            }

            return phase;
        }

        public virtual bool CanTransition(string next)
        {
            if (string.IsNullOrEmpty(next)) return false;
            if (string.IsNullOrEmpty(Current)) return true;//first time

            ComboPhase phase = FindComboPhase();
            if (phase == null) return false;

            switch (phase.NextType)
            {
                case ValidSequenceType.Any:
                    return true;
                case ValidSequenceType.SelectedOnly:
                    for (int i = 0; i < phase.Next.Count; i++)
                    {
                        if (string.CompareOrdinal(next, phase.Next[i].MoveAbility) == 0)
                        {
                            return true;
                        }
                    }
                    break;
                case ValidSequenceType.None:
                    return false;
            }

            return false;

        }

    }


}

