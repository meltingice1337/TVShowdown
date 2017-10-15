using System;
using System.Collections.Generic;
using System.Text;

namespace TVShowdown.Triggers
{
    public enum TriggerType
    {
        ProcessCPU,
        Mouse
    }

    public class TriggerState
    {
        public TriggerType TriggerType;
        public bool Fired = true;

        public TriggerState(TriggerType triggerType, bool fired = false)
        {
            TriggerType = triggerType;
            Fired = fired;
        }
    }
}
