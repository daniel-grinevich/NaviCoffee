using System;
using EversysApi.DataObject;
using EversysApi.Defines;
using EversysApi.Services;
using NaviCoffee.PI.Models;

namespace NaviCoffee.PI.Services
{
    public class CoffeeMakerService : ICoffeeMakerService
    {
        private readonly SerialService _serialService;
        private readonly ApiFunctions _apiFunctions;
        private readonly MyExchangeService _exchangeService;
        private readonly bool _useExchange;

        public CoffeeMakerService(bool useExchange)
        {
            _useExchange = useExchange;

            if (useExchange)
            {
                _exchangeService = new MyExchangeService();

                // create new serial service
                _serialService = new SerialService(_exchangeService);

                // create new api function service
                _apiFunctions = new ApiFunctions(_serialService, _exchangeService);
            }
            else
            {
                // create new serial service
                _serialService = new SerialService();

                // create new api function service
                _apiFunctions = new ApiFunctions(_serialService);
            }

            TestMakeCoffee();
                        
        }

        private void TestMakeCoffee()
        {
            Console.WriteLine("Try to auto connect");

            // try to auto connect to machine
            if (_serialService.AutoConnect())
            {
                Console.WriteLine("Successful connected on " + _serialService.PortName);

                Console.WriteLine("Try to read e'API version");

                // get API version
                if (_apiFunctions.GetVersion(out ApiVersion apiVersion))
                {
                    Console.WriteLine("Version -> " + apiVersion.ToString());
                }
                else
                    Console.WriteLine("Couldn't send GetVersion() command");

                Console.WriteLine("Making coffee");
                _apiFunctions.DoProduct(EversysApi.Defines.ProcSide_t.Left_e, new ApiProductParameter
                {
                    BeanHopper = EversysApi.Defines.BeanHopper_t.Front_e,
                    ProductProcess = EversysApi.Defines.ProductProcess_t.ProProcCoffee_e,
                    ProductType = EversysApi.Defines.ProductType_t.LatteMacchiato_e,
                });
                Console.WriteLine("Coffee made");

                // IMPORTANT!!! disconnect serial connection
                if (_useExchange)
                {
                    // you can do it directly
                    // serialService.Disconnect( );

                    // or with the ApplicationExit callback
                    _exchangeService.CallApplicationExit();
                }
                else
                {
                    Console.WriteLine("\r\nDisconnect serial service");
                    _serialService.Disconnect();
                }
            }
            else
            {
                Console.WriteLine("Auto connect FAILED");
            }
        }

        public bool MakeCoffee(Coffee coffee)
        {
            bool ret = false;

            try
            {
                if (AutoConnect())
                {

                    ret = _apiFunctions.DoProduct(
                        EversysApi.Defines.ProcSide_t.Left_e, //TODO: When we get cup sensors, match this up with the senor etc.
                        new ApiProductParameter
                        {
                            BeanHopper = EversysApi.Defines.BeanHopper_t.Front_e, //TODO: When we multiple blends, match this up with the selected blend.
                            ProductProcess = EversysApi.Defines.ProductProcess_t.ProProcCoffee_e,
                            ProductType = BuildCoffeeType(coffee)
                        });

                    Disconnect();
                }
            }
            catch (Exception ex)
            {
                LogService.LogMessage($"MakeCoffee {coffee.ToString()} {ex.Message}");
            }

            return (ret);
        }

        private ProductType_t BuildCoffeeType(Coffee coffee)
        {
            LogService.LogMessage($"BuildCoffeeType {coffee.CoffeeType}");

            EversysApi.Defines.ProductType_t productType;

            switch (coffee.CoffeeType)
            {
                case CoffeeType.Black:
                    productType = ProductType_t.Coffee_e;
                    break;
                case CoffeeType.Espresso:
                    productType = ProductType_t.Espresso_e;
                    break;
                case CoffeeType.Latte:
                    productType = ProductType_t.LatteMacchiato_e;
                    break;
                default:
                    LogService.LogMessage($"BuildCoffeeType unable to find coffee of type {coffee.CoffeeType}");
                    productType = ProductType_t.Coffee_e;
                    break;
            }

            return productType;
        }

        private bool AutoConnect()
        {
            bool success = false;

            // try to auto connect to machine
            if (_serialService.AutoConnect())
            {
                LogService.LogMessage("Successful connected on " + _serialService.PortName);

                LogService.LogMessage("Try to read e'API version");

                // get API version
                if (_apiFunctions.GetVersion(out ApiVersion apiVersion))
                {
                    LogService.LogMessage("Version -> " + apiVersion.ToString());
                }
                else
                {
                    LogService.LogMessage("Couldn't send GetVersion() command");
                }

                success = true;
            }
            else
            {
                LogService.LogMessage("Auto connect FAILED");
            }
            return success;
        }

        private void Disconnect()
        {

            LogService.LogMessage("\r\nDisconnect serial service");
            _serialService.Disconnect();
        }
    }
}
