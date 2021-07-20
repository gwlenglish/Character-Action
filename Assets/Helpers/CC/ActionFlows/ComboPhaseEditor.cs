using System.Collections.Generic;

namespace GWLPXL.Movement.Character.com
{
    [System.Serializable]
    public class ComboPhaseEditor
    {

        public List<int> Nexts = new List<int>();
        public int Nextcount = 0;
        public int Previousnextcount = 0;

        public List<int> Cancels = new List<int>();
        public int Cancelcount = 0;
        public int PreviousCancelcount = 0;
    }
}
