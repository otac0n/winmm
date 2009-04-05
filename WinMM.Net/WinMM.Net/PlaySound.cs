//-----------------------------------------------------------------------
// <copyright file="PlaySound.cs" company="(none)">
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
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Provides a familiar interface to the functions made available by winmm.dll
    /// </summary>
    public sealed class PlaySound
    {
        /// <summary>
        /// Prevents a default instance of the PlaySound class from being created.
        /// </summary>
        private PlaySound() 
        {
        }

        /// <summary>
        /// Asynchronously plays a system sound.
        /// </summary>
        /// <param name="systemSoundName">The name of the system sound to play.</param>
        /// <remarks>
        /// If the specified sound cannot be found, the call is ignored.
        /// If the sound can be found in the registry, the sound buffer is purged before playing.
        /// </remarks>
        public static void PlaySystemSound(string systemSoundName)
        {
            uint res = NativeMethods.PlaySound(systemSoundName, (IntPtr)0, NativeMethods.PLAYSOUNDFLAGS.SND_ALIAS | NativeMethods.PLAYSOUNDFLAGS.SND_PURGE | NativeMethods.PLAYSOUNDFLAGS.SND_NODEFAULT | NativeMethods.PLAYSOUNDFLAGS.SND_ASYNC);
            if (res == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        /// <summary>
        /// Purges the sound buffer of playing sounds.
        /// </summary>
        public static void StopAllSounds()
        {
            uint res = NativeMethods.PlaySound(null, (IntPtr)0, NativeMethods.PLAYSOUNDFLAGS.SND_PURGE);
            if (res == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        /// <summary>
        /// Asynchronously plays a sound specified by filename.
        /// </summary>
        /// <param name="soundFileName">The filename of the file to play.</param>
        public static void PlaySoundFile(string soundFileName)
        {
            uint res = NativeMethods.PlaySound(soundFileName, (IntPtr)0, NativeMethods.PLAYSOUNDFLAGS.SND_FILENAME | NativeMethods.PLAYSOUNDFLAGS.SND_PURGE | NativeMethods.PLAYSOUNDFLAGS.SND_NODEFAULT | NativeMethods.PLAYSOUNDFLAGS.SND_ASYNC);
            if (res == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}
