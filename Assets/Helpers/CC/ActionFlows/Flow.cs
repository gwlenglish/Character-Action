using System.Collections.Generic;

namespace GWLPXL.Movement.Character.com
{
    /// <summary>
    /// base class that defines a combo flow
    /// </summary>
    [System.Serializable]
    public class Flow
    {
        public List<ComboPhase> ComboPhase = new List<ComboPhase>();//save to disk
        public int combophasecount = 0;
        public int previousphasecount = 0;



    }
}
