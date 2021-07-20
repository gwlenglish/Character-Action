using GWLPXL.Movement.Character.CC.com;
using UnityEngine;

namespace GWLPXL.Movement.Character.com
{

    public enum GetAxisType
    {
        GetAxisRaw = 0,
        GetAxis = 1
    }
    /// <summary>
    /// ticker class that gathers the player inputs
    /// </summary>
    public class GatherPlayerStateInputs : ITick
    {
        PlayerControllerCC player;
        public GatherPlayerStateInputs(PlayerControllerCC player)
        {
            this.player = player;
            AddTicker();
        }
        public void AddTicker()
        {
            TickManager.AddTicker(this);
        }

        public float GetTickDuration()
        {
            return Time.deltaTime;
        }

        public void RemoveTicker()
        {
            TickManager.RemoveTicker(this);
        }

        public void Tick()
        {
            AssignInputs();
        }
        protected virtual void AssignInputs()
        {
            float x = 0;
            float z = 0;

            UnityLegacyInput(ref x, ref z);

            player.Controls.Controller.Locomotion.Movement.X = x;
            player.Controls.Controller.Locomotion.Movement.Z = z;
            player.Controls.Controller.Rotate.Rotation.X = x;
            player.Controls.Controller.Rotate.Rotation.Z = z;
        }

        protected virtual void UnityLegacyInput(ref float x, ref float z)
        {
            switch (player.Controls.PlayerInputs.Type)
            {
                case GetAxisType.GetAxisRaw:
                    x = Input.GetAxisRaw(player.Controls.PlayerInputs.AxisX);
                    z = Input.GetAxisRaw(player.Controls.PlayerInputs.AxisZ);
                    break;
                case GetAxisType.GetAxis:
                    x = Input.GetAxis(player.Controls.PlayerInputs.AxisX);
                    z = Input.GetAxis(player.Controls.PlayerInputs.AxisZ);
                    break;
            }
        }
    }

}