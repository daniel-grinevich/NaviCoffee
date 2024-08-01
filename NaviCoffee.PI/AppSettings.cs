using System;
using System.Collections.Generic;
using System.Text;

namespace NaviCoffee.PI
{
    public class AppSettings
    {
        public string WebApi { get; set; }

        public int BarcodeScanner_PID { get; set; }

        public int BarcodeScanner_VID { get; set; }

        public bool BarcodeScanner_DisplayHIDDevices { get; set; }

        public string Barcodes_Read_Path { get; set; }

        public bool RunSyrupModule { get; set; }

        public string Syrup_Read_Path { get; set; }

        public int Syrup_Timer_In_Milliseconds { get; set; }

    }
}
