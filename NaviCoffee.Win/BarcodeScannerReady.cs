using NaviCoffee.PI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NaviCoffee.Win
{
    public partial class BarcodeScannerReady : Form
    {
        OrderService _orderService;
        private GlobalKeyboardHook _globalKeyboardHook;
        private StringBuilder _barCode = new StringBuilder();
        public Timer _timer;

        public BarcodeScannerReady()
        {
            InitializeComponent();

            _orderService = new OrderService();

            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;

            _timer = new Timer
            {
                Interval = 1000,
                Enabled = false
            };
        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
            {
                //Debug.WriteLine(e.KeyboardData.VirtualCode);

                if (((Keys)e.KeyboardData.VirtualCode >= Keys.A &&
                (Keys)e.KeyboardData.VirtualCode <= Keys.Z) ||
                ((Keys)e.KeyboardData.VirtualCode >= Keys.D0 &&
                (Keys)e.KeyboardData.VirtualCode <= Keys.D9))
                {
                    _barCode.Append((char)e.KeyboardData.VirtualCode);
                }
                else if ((Keys)e.KeyboardData.VirtualCode == Keys.OemMinus)
                {
                    _barCode.Append("-");
                }
            }

            if (_barCode.ToString().Length == 1)
            {
                _timer.Enabled = true;
                _timer.Start();
                _timer.Tick += new EventHandler(timerDelay_Tick);                    
            }
        }

        private void timerDelay_Tick(object sender, EventArgs e)
        {
            try
            {
                _timer.Stop();
                string strCurrentString = _barCode.ToString();
                if (strCurrentString != "")
                {
                    barcodesListView.Items.Add(strCurrentString);
                    Debug.WriteLine(strCurrentString);

                    _orderService.AddBarcodeScan(strCurrentString);

                    _barCode = new StringBuilder();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
