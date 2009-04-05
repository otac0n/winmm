//-----------------------------------------------------------------------
// <copyright file="WaveOutMessageReceivedEventArgs.cs" company="(none)">
//     Copyright (c) 2009 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace WinMM
{
    using System;

    /// <summary>
    /// Describes an event when a WaveOutMessage is recieved.
    /// </summary>
    public sealed class WaveOutMessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Holds the message recieved.
        /// </summary>
        private WaveOutMessage message;

        /// <summary>
        /// Initializes a new instance of the WaveOutMessageReceivedEventArgs class with the specified message.
        /// </summary>
        /// <param name="message">The message the new instance will describe.</param>
        public WaveOutMessageReceivedEventArgs(WaveOutMessage message)
        {
            this.message = message;
        }

        /// <summary>
        /// Gets the message recieved.
        /// </summary>
        public WaveOutMessage Message
        {
            get { return this.message; }
        }
    }
}
