using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TVShowdown.Helpers;
using TVShowdown.Triggers;

namespace TVShowdown
{
    public partial class MainFrm : Form
    {
        private int currentSeconds = Config.TimeToStopShutdown;
        private TriggerManager triggerManager;
        public MainFrm()
        {
            InitializeComponent();
            triggerManager = new TriggerManager(new List<ITrigger> { new MouseTrigger(), new ProcessCPUTrigger("vlc") }, 3);
            triggerManager.OnTrigger += Tm_OnTrigger;
            Opacity = 0;
            ShowInTaskbar = false;
            Visible = false;
        }

        private void Tm_OnTrigger(object sender)
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                Opacity = 100;
                ShowInTaskbar = false;
                Visible = true;
                Reset();
                coutdownTimer.Start();
            }));
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            this.coutdownTimer.Interval = 1000;
            Reset();
        }

        private void Reset()
        {
            currentSeconds = Config.TimeToStopShutdown;
            lblCountdown.Text = currentSeconds.ToString();

        }

        private void coutdownTimer_Tick(object sender, EventArgs e)
        {
            if(currentSeconds-- <= 0)
            {
                ShutdownHelper.Shutdown();
            }
            else
            {
                lblCountdown.Text = currentSeconds.ToString();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Reset();
            Visible = false;
            coutdownTimer.Stop();
            triggerManager.Reset();
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            Reset();
            Visible = false;
            coutdownTimer.Stop();
            triggerManager.Reset();
        }
    }
}
