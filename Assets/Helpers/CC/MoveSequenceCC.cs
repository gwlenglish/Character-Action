using GWLPXL.Movement.Character.com;


namespace GWLPXL.Movement.Character.CC.com
{
    /// <summary>
    /// scripted move sequence
    /// </summary>
    [System.Serializable]
    public class MoveSequenceCC
    {
        public string Name = string.Empty;
        [UnityEngine.Tooltip("Higher numbers have priority")]
        public int Priority = 0;

        public InputRequirements InputRequirements = new InputRequirements();
        public CharacterRequirements CharacterRequirements = new CharacterRequirements();
        public CharacterActionSequenceCC MovementBehavior = new CharacterActionSequenceCC();
    }
}

