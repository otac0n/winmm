//-----------------------------------------------------------------------
// <copyright file="DataReadyEventArgs.cs" company="(none)">
//     Copyright (c) 2009 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace WinMM
{
    using System;

    /// <summary>
    /// Describes an DataReady event.
    /// </summary>
    public class DataReadyEventArgs : EventArgs
    {
        /// <summary>
        /// Holds the value of the data, specific to this event.
        /// </summary>
        private byte[] data;

        /// <summary>
        /// Initializes a new instance of the DataReadyEventArgs class with the specified data.
        /// </summary>
        /// <param name="data">The data specific to this event.</param>
        public DataReadyEventArgs(byte[] data)
        {
            this.data = data;
        }

        /// <summary>
        /// Gets the value of the data, specific to this event.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return this.data;
            }
        }
    }
}
