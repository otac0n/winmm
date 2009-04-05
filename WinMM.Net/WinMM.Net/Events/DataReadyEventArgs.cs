//-----------------------------------------------------------------------
// <copyright file="DataReadyEventArgs.cs" company="(none)">
//  Copyright (c) 2009 John Gietzen
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
