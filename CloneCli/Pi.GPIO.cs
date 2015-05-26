 
using System;
using System.Collections.Generic; //required for List<>
using System.IO;

//Originally written by Britguy, extended by EdwinJ85 on the Raspberry Pi Forums. 
namespace Pi.GPIO
{
    //GPIO connector on the Pi (P1) (as found next to the yellow RCA video socket on the Rpi circuit board)
    //P1-01 = top left,    P1-02 = top right
    //P1-25 = bottom left, P1-26 = bottom right
    //pi connector P1 pin     = GPIOnum = slice of pi v1.0 board label
    //                  P1-07 = GPIO4   = GP7
    //                  P1-11 = GPIO17  = GP0
    //                  P1-12 = GPIO18  = GP1
    //                  P1-13 = GPIO21  = GP2
    //                  P1-15 = GPIO22  = GP3
    //                  P1-16 = GPIO23  = GP4
    //                  P1-18 = GPIO24  = GP5
    //                  P1-22 = GPIO25  = GP6
    //So to turn on Pin7 on the GPIO connector, pass in enumGPIOPIN.gpio4 as the pin parameter

    /// <summary>
    /// A enumeration to designate a GPIO pin for input or output. Pin 16 is the OK light on the rPi board itself.
    /// </summary>
    public enum PIN
    {
        gpio0 = 0, gpio1 = 1, gpio4 = 4, gpio7 = 7, gpio8 = 8, gpio9 = 9, gpio10 = 10, gpio11 = 11,
        gpio14 = 14, gpio15 = 15, gpio16 = 16, gpio17 = 17, gpio18 = 18, gpio21 = 21, gpio22 = 22, gpio23 = 23, gpio24 = 24, gpio25 = 25
    };

    public static class GPIOHandler
    {

        private enum Direction { IN, OUT };

        private const string GPIO_PATH = "/sys/class/gpio/";

        //contains list of pins exported with an OUT direction
        static List<PIN> _OutExported = new List<PIN>();

        //contains list of pins exported with an IN direction
        static List<PIN> _InExported = new List<PIN>();

        //set to true to write whats happening to the screen
        private const bool DEBUG = true;

        //this gets called automatically when we try and output to, or input from, a pin
        private static void SetupPin(PIN pin, Direction direction)
        {
            //unexport if it we're using it already
            if (_OutExported.Contains(pin) || _InExported.Contains(pin)) UnexportPin(pin);

            //export
            File.WriteAllText(GPIO_PATH + "export", GetPinNumber(pin));

            if (DEBUG) Console.WriteLine("exporting pin " + pin + " as " + direction);

            // set i/o direction
            File.WriteAllText(GPIO_PATH + pin.ToString() + "/direction", direction.ToString().ToLower());

            //record the fact that we've setup that pin
            if (direction == Direction.OUT)
                _OutExported.Add(pin);
            else
                _InExported.Add(pin);
        }

        //no need to setup pin this is done for you
        /// <summary>
        /// This function allows us to control a GPIO pin as output.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="value"></param>
        public static void OutputPin(PIN pin, bool value)
        {
            //if we havent used the pin before,  or if we used it as an input before, set it up
            if (!_OutExported.Contains(pin) || _InExported.Contains(pin)) SetupPin(pin, Direction.OUT);

            string writeValue = "0";
            if (value) writeValue = "1";
            File.WriteAllText(GPIO_PATH + pin.ToString() + "/value", writeValue);

            if (DEBUG) Console.WriteLine("output to pin " + pin + ", value was " + value);
        }

        //no need to setup pin this is done for you
        /// <summary>
        /// This function allows us to control a GPIO pin as input.
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        public static bool InputPin(PIN pin)
        {
            bool returnValue = false;

            //if we havent used the pin before, or if we used it as an output before, set it up
            if (!_InExported.Contains(pin) || _OutExported.Contains(pin)) SetupPin(pin, Direction.IN);

            string filename = GPIO_PATH + pin.ToString() + "/value";
            if (File.Exists(filename))
            {
                string readValue = File.ReadAllText(filename);
                if (readValue != null && readValue.Length > 0 && readValue[0] == '1') returnValue = true;
            }
            else
                throw new Exception(string.Format("Cannot read from {0}. File does not exist", pin));

            if (DEBUG) Console.WriteLine("input from pin " + pin + ", value was " + returnValue);

            return returnValue;
        }

        /// <summary>
        /// if for any reason you want to unexport a particular pin use this, otherwise just call CleanUpAllPins when you're done
        /// </summary>
        /// <param name="pin"></param>
        public static void UnexportPin(PIN pin)
        {
            bool found = false;
            if (_OutExported.Contains(pin))
            {
                found = true;
                _OutExported.Remove(pin);
            }
            if (_InExported.Contains(pin))
            {
                found = true;
                _InExported.Remove(pin);
            }

            if (found)
            {
                File.WriteAllText(GPIO_PATH + "unexport", GetPinNumber(pin));
                if (DEBUG) Console.WriteLine("unexporting  pin " + pin);
            }
        }

        /// <summary>
        /// This function closes the handles to the GPIO pins after use and should ALWAYS be called as the last part of any program using this GPIO library.
        /// </summary>
        public static void CleanUpAllPins()
        {
            for (int p = _OutExported.Count - 1; p >= 0; p--) UnexportPin(_OutExported[p]); //unexport in reverse order
            for (int p = _InExported.Count - 1; p >= 0; p--) UnexportPin(_InExported[p]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pin"></param>
        /// <returns>returns n for enum value of gpio n</returns>
        private static string GetPinNumber(PIN pin)
        {
            return ((int)pin).ToString(); //e.g. returns 17 for enum value of gpio17
        }

    }

}