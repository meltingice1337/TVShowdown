using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TVShowdown.Triggers
{
    class TriggerManager
    {
        public delegate void TriggerEventHandler(object sender);
        public event TriggerEventHandler OnTrigger;

        private List<TriggerState> triggeredStates;
        private List<ITrigger> triggers;

        public TriggerManager(List<ITrigger> triggers, int idleMaxSeconds)
        {
            triggeredStates = new List<TriggerState>();
            this.triggers = triggers;
            triggers.ForEach((trigger) =>
            {
                triggeredStates.Add(new TriggerState(trigger.GetTriggerType()));
                trigger.Watch(idleMaxSeconds * 1000);
                trigger.OnTrigger += Trigger_OnTrigger;
            });
        }

        private void Trigger_OnTrigger(object sender, TriggerState e)
        {
            var shouldTriggerFurther = true;
            foreach (TriggerState entry in triggeredStates)
            {
                if(entry.TriggerType == e.TriggerType)
                {
                    entry.Fired = e.Fired;
                }

                if(!entry.Fired)
                {
                    shouldTriggerFurther = false;
                }
            }

            if(shouldTriggerFurther)
            {
                foreach (ITrigger trigger in triggers)
                {
                    trigger.Stop();
                }
                OnTrigger.Invoke(this);
            }
        }

        public void Reset()
        {
            foreach (TriggerState entry in triggeredStates)
            {
                entry.Fired = false;
            }

            foreach (ITrigger trigger in triggers)
            {
                trigger.Reset();
            }
        }
    }
}
