using GWLPXL.Movement.Character.CC.com;
using GWLPXL.Movement.com;
using UnityEngine;

namespace GWLPXL.Movement.Character.com
{
    /// <summary>
    /// ticker class that gathers the action inputs
    /// </summary>
    public class GatherSequenceInputs : ITick
    {
        System.Action<int> SequenceStarted;

        PlayerControllerCC player;

        public GatherSequenceInputs(PlayerControllerCC player, System.Action<int> startsequencecallback)
        {
            this.player = player;
            SequenceStarted += startsequencecallback;
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
            CheckSequenceInputs();
        }
        
        protected virtual void CheckSequenceInputs()
        {

            ActionManager.GetActionInputRequirements(player, SequenceStarted);
          
            

        }
    }

}