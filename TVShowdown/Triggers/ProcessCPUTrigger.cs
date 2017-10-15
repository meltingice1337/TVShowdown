using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using TVShowdown.Helpers;

namespace TVShowdown.Triggers
{
    class ProcessCPUTrigger : ITrigger
    {
        private PerformanceCounter perfCounter;
        private float idlePercentageThreshold;
        private Timer _timer;
        private TriggerType triggerType = TriggerType.ProcessCPU;
        private Clock clock;

        public event TriggerEventHandler OnTrigger;

        public ProcessCPUTrigger(string processName, float idlePercentageThreshold = 3)
        {
            perfCounter = new PerformanceCounter("Process", "% Processor Time", processName);
            this.idlePercentageThreshold = idlePercentageThreshold;
            try
            {
                perfCounter.NextValue();
            }
            catch { }
        }

        public void Watch(int maxIdleTime)
        {
            var autoEvent = new AutoResetEvent(false);
            clock = new Clock(maxIdleTime);
            _timer = new Timer(StatusCheck, autoEvent, 0, 3000);
        }

        public void StatusCheck(Object stateInfo)
        {
            OnTrigger(this, new TriggerState(triggerType, clock.TimeSurpassed && IsProcessIdle()));

            if (!IsProcessIdle())
            {
                clock.Reset();
            }

        }

        public TriggerType GetTriggerType()
        {
            return triggerType;
        }

        private bool IsProcessIdle()
        {
            Thread.Sleep(1000);
            try
            {
                float percentage = (float)perfCounter.NextValue() / Environment.ProcessorCount;
                return percentage < idlePercentageThreshold;
            }
            catch
            {
                return true;
            }
        }

        public void Reset()
        {
            clock.Enable();
            clock.Reset();
        }

        public void Stop()
        {
            clock.Disable();
        }
    }
}
