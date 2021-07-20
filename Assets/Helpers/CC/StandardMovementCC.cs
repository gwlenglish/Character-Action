using GWLPXL.Movement.com;

namespace GWLPXL.Movement.Character.CC.com
{
    /// <summary>
    /// base class for the standard movement on the character cc
    /// </summary>
    [System.Serializable]
    public class StandardMovementCC
    {
        public CharacterRotate Rotate;
        public CharacterLocomotionCC Locomotion;
        public CharacterFallingCC Fall;

    }


}

