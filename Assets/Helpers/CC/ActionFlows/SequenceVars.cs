using System.Collections.Generic;

namespace GWLPXL.Movement.Character.com
{
    [System.Serializable]
    public class SequenceVars
    {
        public bool AnyCancel = false;
        public List<int> Connections = new List<int>();
        public int Count = 0;
        public int PreviousCount = 0;
    }
}

