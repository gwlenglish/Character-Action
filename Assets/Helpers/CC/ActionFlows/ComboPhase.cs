using System.Collections.Generic;

namespace GWLPXL.Movement.Character.com
{
    [System.Serializable]
    public class ComboPhase
    {
        public bool Foldout;
        public int index = 0;

        public Sequence Start = new Sequence();
        public ValidSequenceType NextType = ValidSequenceType.SelectedOnly;
        public List<Sequence> Next = new List<Sequence>();
        public ComboPhaseEditor Vars = new ComboPhaseEditor();

    }
}

