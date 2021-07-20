using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWLPXL.Movement.Character.com;



namespace GWLPXL.Movement.Character.CC.com
{

    /// <summary>
    /// scriptable object container for the player controlled CC
    /// </summary>
    [CreateAssetMenu(menuName = "TestCCController")]
    public class CharacterControllerSO : ScriptableObject
    {
        public PlayerControllerCC PlayerCC;
    }


}

