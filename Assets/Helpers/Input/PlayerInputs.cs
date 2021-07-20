namespace GWLPXL.Movement.Character.com
{
    /// <summary>
    /// locomotion input vars
    /// </summary>
    [System.Serializable]
    public class PlayerInputs
    {
        public GetAxisType Type = GetAxisType.GetAxisRaw;
        public string AxisX = "Horizontal";
        public string AxisZ = "Vertical";
    }
}

