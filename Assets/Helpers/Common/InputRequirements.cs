

using GWLPXL.Movement.com;
using UnityEngine;
namespace GWLPXL.Movement.Character.com
{
    public enum FreeFormState
    {
        Ground = 0,
        Airborne = 1,

    }
    public enum AxisRequirementType//maybe dont need all
    {
        LocalInputZPositive = 0,
        LocalInputZNegative = 1,
        LocalInputZZero = 2,

        LocalInputXPositive = 10,
        LocalInputXNegative = 11,
        LocalInputXZero = 12,


        GlobalInputZPositive = 20,
        GlobalInputZNegative = 21,
        GlobalInputZZero = 22,

        GlobalInputXPositive = 30,
        GlobalInputXNegative = 31,
        GlobalInputXZero = 32,

    }
    public enum ButtonType
    {
        Click = 0,
        Held = 1,
        Released = 2
    }
    /// <summary>
    /// axis requirement for action
    /// </summary>
    [System.Serializable]
    public class InputAxisFreeForm
    {
        public AxisRequirementType RequirementType = AxisRequirementType.LocalInputZPositive;
        [Tooltip("0 means no movement, .01f is movement.")]
        [Range(0, 1f)]
        public float MinDirectionThreshold = .01f;

    }
    /// <summary>
    /// input button required for action
    /// </summary>
    [System.Serializable]
    public class InputButton
    {
        public string ButtonName = string.Empty;
        public ButtonType Type = ButtonType.Click;
    }
    /// <summary>
    /// input requirements for actions, buttons and axis
    /// </summary>
    [System.Serializable]
    public class InputRequirements
    {
        public InputButton[] InputButtons = new InputButton[0];
        public InputAxisFreeForm[] AxisRequirements = new InputAxisFreeForm[0];
    }
}

