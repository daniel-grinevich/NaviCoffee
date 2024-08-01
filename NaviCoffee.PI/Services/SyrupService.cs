using Bifrost.Devices.Gpio;
using Bifrost.Devices.Gpio.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NaviCoffee.PI.Services
{
    public class SyrupService
    {
        const int PIN_NUMBER = 21;

        public void AddSyrup(int waitInMilliseconds)
        {
            var controller = GpioController.Instance;
            var pin = controller.OpenPin(PIN_NUMBER);
            pin.SetDriveMode(GpioPinDriveMode.Output);
            pin.Write(GpioPinValue.High);

            Thread.Sleep(waitInMilliseconds);

            pin.Write(GpioPinValue.Low);

        }
    }
}
