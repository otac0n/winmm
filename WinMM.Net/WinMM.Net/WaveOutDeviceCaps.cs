//-----------------------------------------------------------------------
// <copyright file="WaveOutDeviceCaps.cs" company="(none)">
//  Copyright © 2010 John Gietzen
//
//  Permission is hereby granted, free of charge, to any person obtaining
//  a copy of this software and associated documentation files (the
//  "Software"), to deal in the Software without restriction, including
//  without limitation the rights to use, copy, modify, merge, publish,
//  distribute, sublicense, and/or sell copies of the Software, and to
//  permit persons to whom the Software is furnished to do so, subject to
//  the following conditions:
//
//  The above copyright notice and this permission notice shall be
//  included in all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//  MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//  NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
//  BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
//  ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//  CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace WinMM
{
    /// <summary>
    /// Enumerates the capabilities of a WaveOut device.
    /// </summary>
    public class WaveOutDeviceCaps
    {
        /// <summary>
        /// Holds the system specific identifier of the device
        /// </summary>
        private int deviceId;

        /// <summary>
        /// Holds the device's manufacturer name.
        /// </summary>
        private string manufacturer;

        /// <summary>
        /// Holds the device's product id.
        /// </summary>
        private int productId;

        /// <summary>
        /// Holds the device's driver version.
        /// </summary>
        private int driverVersion;

        /// <summary>
        /// Holds the name of the device.
        /// </summary>
        private string name;

        /// <summary>
        /// Holds the number of channels the device is capable of playing.
        /// </summary>
        private int channels;

        /// <summary>
        /// Holds the capabilities of the device.
        /// </summary>
        private WAVECAPS capabilities;

        /// <summary>
        /// Gets or sets the system specific identifier of the device.
        /// </summary>
        public int DeviceId
        {
            get
            {
                return this.deviceId;
            }

            set
            {
                this.deviceId = value;
            }
        }

        /// <summary>
        /// Gets or sets the device's manufacturer name.
        /// </summary>
        public string Manufacturer
        {
            get
            {
                return this.manufacturer;
            }

            set
            {
                this.manufacturer = value;
            }
        }

        /// <summary>
        /// Gets or sets the device's product id.
        /// </summary>
        public int ProductId
        {
            get
            {
                return this.productId;
            }

            set
            {
                this.productId = value;
            }
        }

        /// <summary>
        /// Gets or sets the device's driver version.
        /// </summary>
        public int DriverVersion
        {
            get
            {
                return this.driverVersion;
            }

            set
            {
                this.driverVersion = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of channels the device is capable of playing.
        /// </summary>
        public int Channels
        {
            get
            {
                return this.channels;
            }

            set
            {
                this.channels = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a device supports pitch modulation.
        /// </summary>
        public bool SupportsPitch
        {
            get
            {
                return (this.Capabilities & WAVECAPS.WAVECAPS_PITCH) != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a device supports playback rate modification.
        /// </summary>
        public bool SupportsPlaybackRate
        {
            get
            {
                return (this.Capabilities & WAVECAPS.WAVECAPS_PLAYBACKRATE) != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a device supports volume changing.
        /// </summary>
        public bool SupportsVolume
        {
            get
            {
                return (this.Capabilities & WAVECAPS.WAVECAPS_VOLUME) != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a device supports stereo volume changing.
        /// </summary>
        public bool SupportsStereoVolume
        {
            get
            {
                return (this.Capabilities & WAVECAPS.WAVECAPS_LRVOLUME) != 0;
            }
        }

        /// <summary>
        /// Gets or sets the capabilities of the device.
        /// </summary>
        internal WAVECAPS Capabilities
        {
            get
            {
                return this.capabilities;
            }

            set
            {
                this.capabilities = value;
            }
        }
    }
}
