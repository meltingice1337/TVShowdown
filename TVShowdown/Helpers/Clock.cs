using System;
using System.Collections.Generic;
using System.Text;

namespace TVShowdown.Helpers
{
    class Clock
    {
        private int maxIdleTime;
        private DateTime initialTime;
        private bool enabled;

        public Clock(int maxIdleTime, DateTime initialTime = default(DateTime))
        {
            if(initialTime == default(DateTime))
                this.initialTime = DateTime.Now;
            this.maxIdleTime = maxIdleTime;
            Enable();
        }

        public bool TimeSurpassed
        {
            get
            {
                TimeSpan timeSpan = DateTime.Now - initialTime;
                return timeSpan.TotalMilliseconds > maxIdleTime && enabled;
            }
        }

        public void Reset()
        {
            initialTime = DateTime.Now;
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
        }
    }
}
