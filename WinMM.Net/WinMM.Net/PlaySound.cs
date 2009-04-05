//-----------------------------------------------------------------------
// <copyright file="PlaySound.cs" company="(none)">
//     Copyright (c) 2009 John Gietzen. All rights reserved.
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
