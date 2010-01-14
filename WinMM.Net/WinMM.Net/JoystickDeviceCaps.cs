//-----------------------------------------------------------------------
// <copyright file="JoystickDeviceCaps.cs" company="(none)">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Enumerates the capabilities of a Joystick device.
    /// </summary>
    public class JoystickDeviceCaps
    {
        /// <summary>
        /// Gets the system specific identifier of the device.
        /// </summary>
        public int DeviceId
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the device's manufacturer name.
        /// </summary>
        public string Manufacturer
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the device's product id.
        /// </summary>
        public int ProductId
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        public string Name
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the minimum amount of time allowed between automatic polls of the device's state.
        /// </summary>
        public int PeriodMin
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the maximum amount of time allowed between automatic polls of the device's state.
        /// </summary>
        public int PeriodMax
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the number of buttons in use by the device.
        /// </summary>
        public int ButtonCount
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets a value indicating whether or not the joystick has the first axis.
        /// </summary>
        public bool HasX
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the joystick has the second axis.
        /// </summary>
        public bool HasY
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the joystick has the third axis.
        /// </summary>
        public bool HasZ
        {
            get
            {
                return (this.Capabilities & JoystickCapabilityFlags.HasZ) != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the joystick has the fourth axis.
        /// </summary>
        public bool HasR
        {
            get
            {
                return (this.Capabilities & JoystickCapabilityFlags.HasR) != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the joystick has the fifth axis.
        /// </summary>
        public bool HasU
        {
            get
            {
                return (this.Capabilities & JoystickCapabilityFlags.HasU) != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the joystick has the sixth axis.
        /// </summary>
        public bool HasV
        {
            get
            {
                return (this.Capabilities & JoystickCapabilityFlags.HasV) != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the device has a point-of-view hat.
        /// </summary>
        public bool HasPov
        {
            get
            {
                return (this.Capabilities & JoystickCapabilityFlags.HasPov) != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the device has a point-of-view hat that supports continuous degree bearings.
        /// </summary>
        public bool HasContinuousPov
        {
            get
            {
                return (this.Capabilities & JoystickCapabilityFlags.HasPov) != 0 &&
                       (this.Capabilities & JoystickCapabilityFlags.PovCts) != 0;
            }
        }

        /// <summary>
        /// Gets or sets the capabilities of the device.
        /// </summary>
        internal int[][] AxisRanges
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the capabilities of the device.
        /// </summary>
        internal JoystickCapabilityFlags Capabilities
        {
            get;
            set;
        }
    }
}
