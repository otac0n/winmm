using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Collections.ObjectModel;

namespace WinMM
{
    public class Joystick
    {
        /// <summary>
        /// Holds a list of manufactureres, read lazily from the assembly's resources.
        /// </summary>
        private static XmlDocument manufacturers;

        /// <summary>
        /// Holds the device's capabilities.
        /// </summary>
        private JoystickDeviceCaps capabilities;

        /// <summary>
        /// Holds this device's DeviceID.
        /// </summary>
        private int deviceId;

        public Joystick(int deviceId)
        {
            if (deviceId >= DeviceCount || deviceId < 0)
            {
                throw new ArgumentOutOfRangeException("deviceId", "The Device ID specified was not within the valid range.");
            }

            this.deviceId = deviceId;
        }

        /// <summary>
        /// Gets the number of devices available on the system.
        /// </summary>
        private static int DeviceCount
        {
            get
            {
                return (int)NativeMethods.joyGetNumDevs();
            }
        }

        /// <summary>
        /// Gets the devices offered by the system.
        /// </summary>
        public static ReadOnlyCollection<JoystickDeviceCaps> Devices
        {
            get
            {
                return GetAllDeviceCaps().AsReadOnly();
            }
        }

        /// <summary>
        /// Gets a document containing the names of all of the device manufactureres.
        /// </summary>
        private static XmlDocument Manufacturers
        {
            get
            {
                if (manufacturers == null)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(WinMM.Properties.Resources.Devices);
                    manufacturers = doc;
                }

                return manufacturers;
            }
        }

        /// <summary>
        /// Gets this device's capabilities.
        /// </summary>
        public JoystickDeviceCaps Capabilities
        {
            get
            {
                if (this.capabilities == null)
                {
                    this.capabilities = GetDeviceCaps(this.deviceId);
                }

                return this.capabilities;
            }
        }

        /// <summary>
        /// Retreives a manufacturer's name from the manufacturer registry resource.
        /// </summary>
        /// <param name="manufacturerId">The ManufacturerID for which to search.</param>
        /// <returns>The specified manufacturer's name.</returns>
        private static string GetManufacturer(int manufacturerId)
        {
            XmlDocument manufacturers = Manufacturers;
            XmlElement man = null;

            if (manufacturers != null)
            {
                man = (XmlElement)manufacturers.SelectSingleNode("/devices/manufacturer[@id='" + manufacturerId.ToString(CultureInfo.InvariantCulture) + "']");
            }

            if (man == null)
            {
                return "Unknown [" + manufacturerId + "]";
            }

            return man.GetAttribute("name");
        }

        /// <summary>
        /// Retrieves the capabilities of a device.
        /// </summary>
        /// <param name="deviceId">The DeviceID for which to retrieve the capabilities.</param>
        /// <returns>The capabilities of the device.</returns>
        private static JoystickDeviceCaps GetDeviceCaps(int deviceId)
        {
            JOYCAPS joycaps = new JOYCAPS();
            NativeMethods.joyGetDevCaps(deviceId, ref joycaps, Marshal.SizeOf(joycaps.GetType()));
            JoystickDeviceCaps caps = new JoystickDeviceCaps();
            caps.DeviceId = deviceId;
            caps.Manufacturer = GetManufacturer(joycaps.wMid);
            caps.Name = joycaps.szPname;
            caps.ProductId = joycaps.wPid;
            caps.ButtonCount = joycaps.wNumButtons;
            caps.PeriodMin = joycaps.wPeriodMin;
            caps.PeriodMax = joycaps.wPeriodMax;

            var axes = new int[][] {
                    new [] { joycaps.wXmin, joycaps.wXmax },
                    new [] { joycaps.wYmin, joycaps.wYmax },
                    new [] { joycaps.wZmin, joycaps.wZmax },
                    new [] { joycaps.wRmin, joycaps.wRmax },
                    new [] { joycaps.wUmin, joycaps.wUmax },
                    new [] { joycaps.wVmin, joycaps.wVmax }
            };

            caps.Capabilities = joycaps.wCaps;

            return caps;
        }

        /// <summary>
        /// Retrieves a list of the capabilities of all of the devices registered on the system.
        /// </summary>
        /// <returns>A list of the capabilities of all of the devices registered on the system.</returns>
        private static List<JoystickDeviceCaps> GetAllDeviceCaps()
        {
            List<JoystickDeviceCaps> devices = new List<JoystickDeviceCaps>();
            int count = DeviceCount;

            //JOYINFOEX joyinfo = new JOYINFOEX();

            for (int i = 0; i < count; i++)
            {
                //MMSYSERROR result = NativeMethods.joyGetPosEx(i, ref joyinfo);
                
                //if (result != MMSYSERROR.MMSYSERR_NOERROR)
                //{
                //    break;
                //}

                devices.Add(GetDeviceCaps(i));
            }

            return devices;
        }

        public JoystickState ReadState()
        {
            JOYINFOEX joyinfo = new JOYINFOEX();
            joyinfo.dwFlags = JOYINFOFLAGS.JOY_RETURNALL;
            NativeMethods.Throw(
                NativeMethods.joyGetPosEx(this.deviceId, ref joyinfo),
                NativeMethods.ErrorSource.Joystick);

            return null;
        }
    }
}
