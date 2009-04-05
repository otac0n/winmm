//-----------------------------------------------------------------------
// <copyright file="WaveInSafeHandle.cs" company="(none)">
//     Copyright (c) 2009 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace WinMM
{
    using System;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Encapsulates a handle to a waveIn device.
    /// </summary>
    internal sealed class WaveInSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// Initializes a new instance of the WaveInSafeHandle class.
        /// </summary>
        public WaveInSafeHandle()
            : base(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the WaveInSafeHandle class.
        /// </summary>
        /// <param name="tempHandle">A temporary handle from which to initialize.  The temporart handle MUST NOT be released after this instance has been created.</param>
        public WaveInSafeHandle(IntPtr tempHandle)
            : base(true)
        {
            this.handle = tempHandle;
        }

        /// <summary>
        /// Releases the resuorces used by this handle.
        /// </summary>
        /// <returns>true, if disposing of the handle succeeded; false, otherwise.</returns>
        protected override bool ReleaseHandle()
        {
            if (!this.IsClosed)
            {
                NativeMethods.MMSYSERROR ret = NativeMethods.waveInClose(this);
                return ret == NativeMethods.MMSYSERROR.MMSYSERR_NOERROR;
            }
            else
            {
                return true;
            }
        }
    }
}
