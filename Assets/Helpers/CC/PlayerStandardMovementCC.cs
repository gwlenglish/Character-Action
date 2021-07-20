
using GWLPXL.Movement.Character.com;
using UnityEngine;
namespace GWLPXL.Movement.Character.CC.com
{
    /// <summary>
    /// standard movement controls for a player CC character
    /// </summary>
    [System.Serializable]
    public class PlayerStandardMovementCC
    {
        public PlayerInputs PlayerInputs = null;
        public StandardMovementCC Controller = null;
        public CharacterActionsCCSO[] Actions = new CharacterActionsCCSO[0];
        public SequenceSO FlowControls = null;

    }
}

