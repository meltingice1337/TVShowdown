using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using TVShowdown.Helpers;
using static TVShowdown.Win32;

namespace TVShowdown.Triggers
{
    class MouseTrigger : ITrigger
    {
        private TriggerType triggerType = TriggerType.Mouse;
        private LowLevelMouseProc _proc;
        private IntPtr _hookID = IntPtr.Zero;
        private Timer timer;
        private Clock clock;

        public event TriggerEventHandler OnTrigger;

        public void Watch(int maxIdleTime)
        {
            var autoEvent = new AutoResetEvent(false);
            this._proc = MouseEvent;
            _hookID = SetHook(_proc);
            clock = new Clock(maxIdleTime);
            timer = new Timer(StatusCheck, autoEvent, 0, 3000);
        }


        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr MouseEvent(int nCode, IntPtr wParam, IntPtr lParam)
        {
            clock.Reset();
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        public TriggerType GetTriggerType()
        {
            return triggerType;
        }

        public void Reset()
        {
            clock.Enable();
            _hookID = SetHook(_proc);
            clock.Reset();
        }

        public void StatusCheck(Object stateInfo)
        {
            OnTrigger(this, new TriggerState(triggerType, clock.TimeSurpassed));
        }

        public void Stop()
        {
            UnhookWindowsHookEx(_hookID);
            clock.Disable();
        }
    }
}
