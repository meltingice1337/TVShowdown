using System;
using System.Collections.Generic;
using System.Text;

namespace TVShowdown.Triggers
{
    public delegate void TriggerEventHandler(object sender, TriggerState e);
    interface ITrigger
    {
        event TriggerEventHandler OnTrigger;
        void Watch(int idleMaxTime);
        void StatusCheck(Object stateInfo);
        void Reset();
        void Stop();
        TriggerType GetTriggerType();
    }
}
