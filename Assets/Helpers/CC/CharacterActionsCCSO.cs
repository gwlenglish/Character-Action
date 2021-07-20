using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.Movement.Character.CC.com
{
    /// <summary>
    /// Scriptable Object Container for the character actions CC
    /// </summary>
    [CreateAssetMenu(menuName ="TestMovement")]
    public class CharacterActionsCCSO : ScriptableObject
    {
        public ActionsDataContainerSO Resources;
        public string ScriptedNames = string.Empty;
        public CharacterActions Movement;
    }
    /// <summary>
    /// class for character actions CC
    /// </summary>
    [System.Serializable]
    public class CharacterActions
    {
        public ExtraMovementCC Movements = new ExtraMovementCC();
    }





}
