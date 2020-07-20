using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Rollout.Common;
using Rollout.BLL;
using log4net;
using System.Reflection;

namespace Rollout.UI.Winform
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
            tb_Output.Clear(); // Clear form upon loading
        }

        public void AddText(string TextToAdd)
        {
            tb_Output.Text += (TextToAdd+"\r\n");
            Application.DoEvents();
            return;
        }
    }
}
