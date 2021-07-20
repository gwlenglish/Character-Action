namespace GWLPXL.Movement.Character.com
{
    /// <summary>
    /// base class for Animator State Name
    /// </summary>
    [System.Serializable]
    public class CharacterAnimator
    {
        public string AnimatorStateName;

    }
    /// <summary>
    /// base class for rotate direction strings for the animator
    /// </summary>
    [System.Serializable]
    public class CharacterRotateAnimatorRotate : CharacterAnimator
    {
        public string DirX = "RotX";
        public string DirZ = "RotZ";

    }
    /// <summary>
    /// base class for locomotion strings for the animator
    /// </summary>
    [System.Serializable]
    public class CharacterLocomotionAnimator : CharacterAnimator
    {
        public string DirX = "MoveX";
        public string DirZ = "MoveZ";
        public string Speed = "GroundSpeed";
        public string AirborneState = "Airborne";

    }
    /// <summary>
    /// base class for falling animator strings
    /// </summary>
    [System.Serializable]
    public class CharacterFallingAnimator : CharacterAnimator
    {

    }
    //base class for jumping animator strings
    [System.Serializable]
    public class CharacterJumpingAnimator : CharacterAnimator
    {

    }
}

