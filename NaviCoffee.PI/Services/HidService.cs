using HidSharp;
using HidSharp.Reports;
using HidSharp.Reports.Input;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace NaviCoffee.PI.Services
{
    /// <summary>
    /// HidService won't work on linux unless access has been granted to the correct device.
    /// In the case of the PI, this was an HID device.  The following was used to gather the info:
    /// https://puredata.info/docs/faq/how-can-i-set-permissions-so-hid-can-read-devices-in-gnu-linux
    /// Grab name of device: udevadm info -a -p $(udevadm info -q path -n /dev/*your device*)
    /// Add rule:
    /// sudo mkdir -p /etc/udev/rules.d
    /// sudo nano /etc/udev/rules.d/85-pure-data.rules
    /// Add the line:
    /// SUBSYSTEM=="HIDRAW", MODE="666"
    /// Group not required for the above but input can be captured by others.  Use:
    /// SUBSYSTEM=="HIDRAW", GROUP="input", MODE="0660"
    /// then after saving that:
    /// sudo groupadd -f input
    /// sudo gpasswd -a YOURUSERNAME input
    /// Then check your permissions:
    /// $ ls -al /dev/input/
    /// If the rule dosen't seem to work, you can run a udev test to see if it's being read and if there are any errors. Run the test using an input device:
    /// sudo udevadm test /dev/input/event0
    /// </summary>
    public class HidService
    {
        private readonly IConfiguration _config;
        static HidDevice _device;
        static HidDeviceInputReceiver _inputReceiver;
        HidStream _hidStream;
        static DeviceItemInputParser _inputParser;
        static byte[] _inputReportBuffer;

        public HidService()
        {
            _config = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .Build();
        }
        
        public void DisplayHidDevices()
        {
            var appSettings = _config.GetSection("AppSettings").Get<AppSettings>();

            if (appSettings.BarcodeScanner_DisplayHIDDevices)
            {
	        var devices = HidSharp.DeviceList.Local.GetHidDevices();

                foreach (HidDevice d in devices)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine(d);
                    System.Console.WriteLine(d.GetFileSystemName());
                    System.Console.WriteLine(d.DevicePath);
                    System.Console.WriteLine();
                }
            }
        }

        //static void PacketReceived(object sender, EventArgs e)
        static void PacketReceived(object sender, EventArgs e)
        {
            /*Console.WriteLine("PacketReceived");

            var buffer = new byte[_device.GetMaxInputReportLength()];

            while (_reciever.TryRead(buffer, 0, out var rpt))
            {
                Console.WriteLine(BitConverter.ToString(buffer));
                Console.WriteLine(e.GetType().ToString());
                Console.WriteLine(sender.ToString());

                Console.WriteLine(rpt.ReportType.ToString());
                //rpt.DeviceItem.OutputReports.First().DataItems.First().
            }
            string converted = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            Console.WriteLine("string: " + converted);
            */
            Report report;
            while (_inputReceiver.TryRead(_inputReportBuffer, 0, out report))
            {
                // Parse the report if possible.
                // This will return false if (for example) the report applies to a different DeviceItem.
                if (_inputParser.TryParseReport(_inputReportBuffer, 0, report))
                {
                    // If you are using Windows Forms, you could call BeginInvoke here to marshal the results
                    // to your main thread.
                    WriteDeviceItemInputParserResult(_inputParser);
                }
            }
        }

        static void WriteDeviceItemInputParserResult(HidSharp.Reports.Input.DeviceItemInputParser parser)
        {
            while (parser.HasChanged)
            {
                int changedIndex = parser.GetNextChangedIndex();
                var previousDataValue = parser.GetPreviousValue(changedIndex);
                var dataValue = parser.GetValue(changedIndex);

                Console.WriteLine(string.Format("  {0}: {1} -> {2}",
                                  (Usage)dataValue.Usages.FirstOrDefault(), previousDataValue.GetPhysicalValue(), dataValue.GetPhysicalValue()));
            }
            /*
            if (parser.HasChanged)
            {
                int valueCount = parser.ValueCount;

                for (int valueIndex = 0; valueIndex < valueCount; valueIndex++)
                {
                    var dataValue = parser.GetValue(valueIndex);
                    Console.Write(string.Format("  {0}: {1}",
                                      (Usage)dataValue.Usages.FirstOrDefault(), dataValue.GetPhysicalValue()));

                }

                Console.WriteLine();
            }
*/
        }

       

        public void Start()
        {
            var appSettings = _config.GetSection("AppSettings").Get<AppSettings>();

            var devices = HidSharp.DeviceList.Local.GetHidDevices();
            var tablets = devices.FirstOrDefault(d => d.VendorID == appSettings.BarcodeScanner_VID && d.ProductID == appSettings.BarcodeScanner_PID);            
            _device = tablets;
            //Console.WriteLine($"HID MaxInputReportLength: {_device.GetMaxInputReportLength()}");
            var descriptor = _device.GetReportDescriptor();
            //Console.WriteLine($"HID descriptor: { descriptor}");
            //_receiver = descriptor.CreateHidDeviceInputReceiver();
            //_receiver.Start(_device.Open());
            //_receiver.Received += PacketReceived;
            //

            if (_device.TryOpen(out _hidStream))
            {
                Console.WriteLine("Opened device.");
                _hidStream.ReadTimeout = Timeout.Infinite;
//                using (_hidStream)
  //              {
                    _inputReportBuffer = new byte[_device.GetMaxInputReportLength()];
                    _inputReceiver = descriptor.CreateHidDeviceInputReceiver();
                    _inputParser = descriptor.DeviceItems.First().CreateDeviceItemInputParser();

                    _inputReceiver.Received += (sender, e) =>
                    {
                        Report report;
                        while (_inputReceiver.TryRead(_inputReportBuffer, 0, out report))
                        {
                            // Parse the report if possible.
                            // This will return false if (for example) the report applies to a different DeviceItem.
                            if (_inputParser.TryParseReport(_inputReportBuffer, 0, report))
                            {
                                // If you are using Windows Forms, you could call BeginInvoke here to marshal the results
                                // to your main thread.
                                WriteDeviceItemInputParserResult(_inputParser);
                                
                            }
                        }
                    };
                    _inputReceiver.Start(_hidStream);
            //    }

            }
            else
            {
                Console.WriteLine("Unable to open HID stream");
            }
        }

        
    }
}
