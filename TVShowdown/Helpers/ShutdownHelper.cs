using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TVShowdown.Helpers
{
    class ShutdownHelper
    {
        public static void Shutdown()
        {
            var psi = new ProcessStartInfo("shutdown", "/s /f /t 0");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
        }
    }
}
