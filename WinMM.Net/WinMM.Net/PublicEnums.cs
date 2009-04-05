//-----------------------------------------------------------------------
// <copyright file="PublicEnums.cs" company="(none)">
//  Copyright © 2009 John Gietzen
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
    /// Indicates a WaveOut message.
    /// </summary>
    public enum WaveOutMessage
    {
        /// <summary>
        /// Not Used.  Indicates that there is no message.
        /// </summary>
        None = 0x000,

        /// <summary>
        /// Indicates that the device has been opened.
        /// </summary>
        DeviceOpened = 0x3BB,

        /// <summary>
        /// Indicates that the device has been closed.
        /// </summary>
        DeviceClosed = 0x3BC,

        /// <summary>
        /// Indicates that playback of a write operation has been completed.
        /// </summary>
        WriteDone = 0x3BD
    }

    /// <summary>
    /// Indicates a WaveIn message.
    /// </summary>
    public enum WaveInMessage
    {
        /// <summary>
        /// Not Used.  Indicates that there is no message.
        /// </summary>
        None = 0x000,

        /// <summary>
        /// Indicates that the device has been opened.
        /// </summary>
        DeviceOpened = 0x3BE,

        /// <summary>
        /// Indicates that the device has been closed.
        /// </summary>
        DeviceClosed = 0x3BF,

        /// <summary>
        /// Indicates that playback of a write operation has been completed.
        /// </summary>
        DataReady = 0x3C0
    }

    /// <summary>
    /// Indicates a wave data sample format.
    /// </summary>
    public enum WaveFormatTag
    {
        /// <summary>
        /// Indicates an invalid sample format.
        /// </summary>
        Invalid = 0x00,

        /// <summary>
        /// Indicates raw Pulse Code Modulation data.
        /// </summary>
        Pcm = 0x01,

        /// <summary>
        /// Indicates Adaptive Differential Pulse Code Modulation data.
        /// </summary>
        Adpcm = 0x02,

        /// <summary>
        /// Indicates IEEE-Float data.
        /// </summary>
        Float = 0x03,

        /// <summary>
        /// Indicates a-law companded data.
        /// </summary>
        ALaw = 0x06,

        /// <summary>
        /// Indicates μ-law  companded data.
        /// </summary>
        MuLaw = 0x07,
    }

    /// <summary>
    /// Describes a variety of channels, frequencies, and bit-depths by which a wave signal may be expressed.
    /// </summary>
    [Flags]
    public enum WaveFormats
    {
        /// <summary>
        /// Monaural, 8-bit, 11025 Hz
        /// </summary>
        Mono8Bit11Khz = 1,

        /// <summary>
        /// Stereo, 8-bit, 11025 Hz
        /// </summary>
        Stereo8Bit11Khz = 2,

        /// <summary>
        /// Monaural, 16-bit, 11025 Hz
        /// </summary>
        Mono16Bit11Khz = 4,

        /// <summary>
        /// Stereo, 16-bit, 11025 Hz
        /// </summary>
        Stereo16Bit11Khz = 8,

        /// <summary>
        /// Monaural, 8-bit, 22050 Hz
        /// </summary>
        Mono8Bit22Khz = 16,

        /// <summary>
        /// Stereo, 8-bit, 22050 Hz
        /// </summary>
        Stereo8Bit22Khz = 32,

        /// <summary>
        /// Monaural, 16-bit, 22050 Hz
        /// </summary>
        Mono16Bit22Khz = 64,

        /// <summary>
        /// Stereo, 16-bit, 22050 Hz
        /// </summary>
        Stereo16Bit22Khz = 128,

        /// <summary>
        /// Monaural, 8-bit, 44100 Hz
        /// </summary>
        Mono8Bit44Khz = 256,

        /// <summary>
        /// Stereo, 8-bit, 44100 Hz
        /// </summary>
        Stereo8Bit44Khz = 512,

        /// <summary>
        /// Monaural, 16-bit, 44100 Hz
        /// </summary>
        Mono16Bit44Khz = 1024,

        /// <summary>
        /// Stereo, 16-bit, 44100 Hz
        /// </summary>
        Stereo16Bit44Khz = 2048,

        /// <summary>
        /// Monaural, 8-bit, 48000 Hz
        /// </summary>
        Mono8Bit48Khz = 4096,

        /// <summary>
        /// Stereo, 8-bit, 48000 Hz
        /// </summary>
        Stereo8Bit48Khz = 8192,

        /// <summary>
        /// Monaural, 16-bit, 48000 Hz
        /// </summary>
        Mono16Bit48Khz = 16384,

        /// <summary>
        /// Stereo, 16-bit, 48000 Hz
        /// </summary>
        Stereo16Bit48Khz = 32768,

        /// <summary>
        /// Monaural, 8-bit, 96000 Hz
        /// </summary>
        Mono8Bit96Khz = 65536,

        /// <summary>
        /// Stereo, 8-bit, 96000 Hz
        /// </summary>
        Stereo8Bit96Khz = 131072,

        /// <summary>
        /// Monaural, 16-bit, 96000 Hz
        /// </summary>
        Mono16Bit96Khz = 262144,

        /// <summary>
        /// Stereo, 16-bit, 96000 Hz
        /// </summary>
        Stereo16Bit96Khz = 524288,
    }
}
