//-----------------------------------------------------------------------
// <copyright file="WaveIn.cs" company="(none)">
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
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Xml;

    /// <summary>
    /// Encapsulates the waveIn commands in the <see cref="NativeMethods"/> class (from winmm.dll).  This provides a familiar format for using the WaveIn tools.
    /// </summary>
    public sealed class WaveIn : IDisposable
    {
        /// <summary>
        /// Indicates the DeviceID of the Microsoft Wave Mapper device.
        /// </summary>
        public const int WaveInMapperDeviceId = -1;

        /// <summary>
        /// Holds a list of manufactureres, read lazily from the assembly's resources.
        /// </summary>
        private static XmlDocument manufacturers;

        /// <summary>
        /// Holds the device's capabilities.
        /// </summary>
        private WaveInDeviceCaps capabilities;

        /// <summary>
        /// Hold a locking object for start/stop synchronization.
        /// </summary>
        private object startStopLock = new object();

        /// <summary>
        /// Hold a locking object for buffer synchronization.
        /// </summary>
        private object bufferingLock = new object();

        /// <summary>
        /// Holds the current recording format.
        /// </summary>
        private WaveFormat recordingFormat;

        /// <summary>
        /// Holds a flag indicating whether or not we are currently buffering.
        /// </summary>
        private bool buffering;

        /// <summary>
        /// Holds the number of samples to hold in each buffer in the queue.
        /// </summary>
        private int bufferSize = 200;

        /// <summary>
        /// Holds the size of the buffer queue size in buffers.
        /// </summary>
        private int bufferQueueSize = 30;

        /// <summary>
        /// Holds the number of buffers currently in the queue.
        /// </summary>
        private int bufferQueueCount;

        /// <summary>
        /// Holds a list of buffers to be released to the operating system.
        /// </summary>
        private Queue<IntPtr> bufferReleaseQueue = new Queue<IntPtr>();

        /// <summary>
        /// Holds the thread used to release completed buffers and add new buffers to the queue.
        /// </summary>
        private Thread bufferMaintainerThread;

        /// <summary>
        /// Holds this device's DeviceID.
        /// </summary>
        private int deviceId;

        /// <summary>
        /// Holds the handle to the device.
        /// </summary>
        private WaveInSafeHandle handle;

        /// <summary>
        /// Holds a reference to our our own callback.
        /// </summary>
        /// <remarks>
        /// We assign this a value in the constructor, and maintain it until at lease after either
        /// Dispose or the Finalizer is called to prevent the garbage collector from finalizing
        /// the instance we pass to the <see cref="NativeMethods.waveInOpen"/> method.
        /// </remarks>
        private WaveInProc callback;

        /// <summary>
        /// Initializes a new instance of the WaveIn class, based on an available Device Id.
        /// </summary>
        /// <param name="deviceId">The device identifier to obtain.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="deviceId"/> is not in the valid range.</exception>
        public WaveIn(int deviceId)
        {
            if ((deviceId >= DeviceCount || deviceId < 0) && deviceId != WaveInMapperDeviceId)
            {
                throw new ArgumentOutOfRangeException("deviceId", "The Device ID specified was not within the valid range.");
            }

            this.callback = new WaveInProc(this.InternalCallback);

            this.deviceId = deviceId;
        }

        /// <summary>
        /// Finalizes an instance of the WaveIn class and disposes of the native resources used by the instance.
        /// </summary>
        ~WaveIn()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Called when the system returns from the device.
        /// </summary>
        public event EventHandler<DataReadyEventArgs> DataReady;

        /// <summary>
        /// Gets the devices offered by the system.
        /// </summary>
        public static ReadOnlyCollection<WaveInDeviceCaps> Devices
        {
            get
            {
                return GetAllDeviceCaps().AsReadOnly();
            }
        }

        /// <summary>
        /// Gets or sets the size of the internal input buffers, in samples.
        /// </summary>
        public int BufferSize
        {
            get
            {
                return this.bufferSize;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", "The value you specified is too small");
                }

                this.bufferSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of internal input buffers to enqueue at the device level.
        /// </summary>
        public int BufferQueueSize
        {
            get
            {
                return this.bufferQueueSize;
            }

            set
            {
                if (this.bufferQueueSize <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", "The value you specified is too small");
                }

                this.bufferQueueSize = value;
            }
        }

        /// <summary>
        /// Gets this device's capabilities.
        /// </summary>
        public WaveInDeviceCaps Capabilities
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
        /// Gets the number of devices available on the system.
        /// </summary>
        private static int DeviceCount
        {
            get
            {
                return (int)NativeMethods.waveInGetNumDevs();
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
        /// Opens the device for writing with the specified format.
        /// </summary>
        /// <param name="waveFormat">The format of the device to open.</param>
        public void Open(WaveFormat waveFormat)
        {
            lock (this.startStopLock)
            {
                if (this.handle != null)
                {
                    throw new InvalidOperationException("The device is already open.");
                }

                WAVEFORMATEX wfx = new WAVEFORMATEX();
                wfx.nAvgBytesPerSec = waveFormat.AverageBytesPerSecond;
                wfx.wBitsPerSample = waveFormat.BitsPerSample;
                wfx.nBlockAlign = waveFormat.BlockAlign;
                wfx.nChannels = waveFormat.Channels;
                wfx.wFormatTag = (short)(int)waveFormat.FormatTag;
                wfx.nSamplesPerSec = waveFormat.SamplesPerSecond;
                wfx.cbSize = 0;

                this.recordingFormat = waveFormat.Clone();

                IntPtr tempHandle = new IntPtr();
                NativeMethods.Throw(
                    NativeMethods.waveInOpen(
                        ref tempHandle,
                        this.deviceId,
                        ref wfx,
                        this.callback,
                        (IntPtr)0,
                        WaveOpenFlags.CALLBACK_FUNCTION | WaveOpenFlags.WAVE_FORMAT_DIRECT),
                    NativeMethods.ErrorSource.WaveOut);
                this.handle = new WaveInSafeHandle(tempHandle);
            }
        }

        /// <summary>
        /// Closes the device.  If the device is playing, playback is stopped.
        /// </summary>
        /// <remarks>
        /// If the device is not currently open, this function does nothing.
        /// </remarks>
        public void Close()
        {
            lock (this.startStopLock)
            {
                if (this.handle != null)
                {
                    if (!this.handle.IsClosed && !this.handle.IsInvalid)
                    {
                        this.Stop();
                        this.handle.Close();
                    }

                    this.handle = null;
                }
            }
        }

        /// <summary>
        /// Begins recording.
        /// </summary>
        public void Start()
        {
            lock (this.startStopLock)
            {
                if (this.bufferMaintainerThread != null)
                {
                    throw new InvalidOperationException("The device has already been started.");
                }

                lock (this.bufferingLock)
                {
                    this.buffering = true;
                    Monitor.Pulse(this.bufferingLock);
                }

                this.bufferMaintainerThread = new Thread(new ThreadStart(this.MaintainBuffers));
                this.bufferMaintainerThread.IsBackground = true;
                this.bufferMaintainerThread.Name = "WaveIn MaintainBuffers thread. (DeviceID = " + this.deviceId + ")";
                this.bufferMaintainerThread.Start();

                NativeMethods.Throw(
                    NativeMethods.waveInStart(this.handle),
                    NativeMethods.ErrorSource.WaveIn);
            }
        }

        /// <summary>
        /// Stops recording.
        /// </summary>
        /// <remarks>
        /// If the device is not currently started, this call does nothing.
        /// </remarks>
        public void Stop()
        {
            lock (this.startStopLock)
            {
                if (this.bufferMaintainerThread != null)
                {
                    lock (this.bufferingLock)
                    {
                        this.buffering = false;
                        Monitor.Pulse(this.bufferingLock);
                    }

                    NativeMethods.Throw(
                        NativeMethods.waveInReset(this.handle),
                        NativeMethods.ErrorSource.WaveIn);

                    this.bufferMaintainerThread.Join();
                    this.bufferMaintainerThread = null;
                }
            }
        }

        /// <summary>
        /// Determines whether or not the device supports a given format.
        /// </summary>
        /// <param name="waveFormat">The format to check.</param>
        /// <returns>true, if the format is supported; false, otherwise.</returns>
        public bool SupportsFormat(WaveFormat waveFormat)
        {
            WAVEFORMATEX wfx = new WAVEFORMATEX();
            wfx.nAvgBytesPerSec = waveFormat.AverageBytesPerSecond;
            wfx.wBitsPerSample = waveFormat.BitsPerSample;
            wfx.nBlockAlign = waveFormat.BlockAlign;
            wfx.nChannels = waveFormat.Channels;
            wfx.wFormatTag = (short)(int)waveFormat.FormatTag;
            wfx.nSamplesPerSec = waveFormat.SamplesPerSecond;
            wfx.cbSize = 0;

            IntPtr dummy = new IntPtr(0);
            MMSYSERROR ret = NativeMethods.waveInOpen(
                ref dummy,
                this.deviceId,
                ref wfx,
                null,
                (IntPtr)0,
                WaveOpenFlags.WAVE_FORMAT_QUERY);

            if (ret == MMSYSERROR.MMSYSERR_NOERROR)
            {
                return true;
            }
            else if (ret == MMSYSERROR.WAVERR_BADFORMAT)
            {
                return false;
            }
            else
            {
                NativeMethods.Throw(ret, NativeMethods.ErrorSource.WaveIn);
                return false;
            }
        }

        /// <summary>
        /// Disposes of the managed and native resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Retrieves the capabilities of a device.
        /// </summary>
        /// <param name="deviceId">The DeviceID for which to retrieve the capabilities.</param>
        /// <returns>The capabilities of the device.</returns>
        private static WaveInDeviceCaps GetDeviceCaps(int deviceId)
        {
            WAVEINCAPS wicaps = new WAVEINCAPS();
            NativeMethods.waveInGetDevCaps((IntPtr)deviceId, ref wicaps, Marshal.SizeOf(wicaps.GetType()));
            WaveInDeviceCaps caps = new WaveInDeviceCaps();
            caps.DeviceId = (int)deviceId;
            caps.Channels = wicaps.wChannels;
            caps.DriverVersion = (int)wicaps.vDriverVersion;
            caps.Manufacturer = GetManufacturer(wicaps.wMid);
            caps.Name = wicaps.szPname;
            caps.ProductId = wicaps.wPid;

            return caps;
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
        /// Retrieves a list of the capabilities of all of the devices registered on the system.
        /// </summary>
        /// <returns>A list of the capabilities of all of the devices registered on the system.</returns>
        private static List<WaveInDeviceCaps> GetAllDeviceCaps()
        {
            List<WaveInDeviceCaps> devices = new List<WaveInDeviceCaps>();
            int count = DeviceCount;

            for (int i = 0; i < count; i++)
            {
                devices.Add(GetDeviceCaps(i));
            }

            devices.Add(GetDeviceCaps(WaveInMapperDeviceId));

            return devices;
        }

        /// <summary>
        /// Adds buffers to the device and cleans up completed buffers.
        /// </summary>
        private void MaintainBuffers()
        {
            try
            {
                while (this.buffering)
                {
                    lock (this.bufferingLock)
                    {
                        while ((this.bufferQueueCount >= this.bufferQueueSize && this.bufferReleaseQueue.Count == 0) && this.buffering)
                        {
                            Monitor.Wait(this.bufferingLock);
                        }
                    }

                    while (this.bufferQueueCount < this.bufferQueueSize && this.buffering)
                    {
                        this.AddBuffer();
                    }

                    while (this.bufferReleaseQueue.Count > 0)
                    {
                        this.ProcessDone();
                    }
                }

                while (this.bufferReleaseQueue.Count > 0 || this.bufferQueueCount > 0)
                {
                    lock (this.bufferingLock)
                    {
                        while (this.bufferReleaseQueue.Count == 0)
                        {
                            Monitor.Wait(this.bufferingLock, 1000);
                        }
                    }

                    while (this.bufferReleaseQueue.Count > 0)
                    {
                        this.ProcessDone();
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
        }

        /// <summary>
        /// Adds a buffer to the queue.
        /// </summary>
        private void AddBuffer()
        {
            // Allocate unmanaged memory for the buffer
            int bufferLength = this.bufferSize * this.recordingFormat.BlockAlign;
            IntPtr mem = Marshal.AllocHGlobal(bufferLength);

            // Initialize the buffer header, including a reference to the buffer memory
            WAVEHDR pwh = new WAVEHDR();
            pwh.dwBufferLength = bufferLength;
            pwh.dwFlags = 0;
            pwh.lpData = mem;
            pwh.dwUser = new IntPtr(12345);

            // Copy the header into unmanaged memory
            IntPtr header = Marshal.AllocHGlobal(Marshal.SizeOf(pwh));
            Marshal.StructureToPtr(pwh, header, false);

            // Prepare the header
            NativeMethods.Throw(
                NativeMethods.waveInPrepareHeader(this.handle, header, Marshal.SizeOf(typeof(WAVEHDR))),
                NativeMethods.ErrorSource.WaveOut);

            // Add the buffer to the device
            NativeMethods.Throw(
                NativeMethods.waveInAddBuffer(this.handle, header, Marshal.SizeOf(typeof(WAVEHDR))),
                NativeMethods.ErrorSource.WaveOut);

            lock (this.bufferingLock)
            {
                this.bufferQueueCount++;
                Monitor.Pulse(this.bufferingLock);
            }
        }

        /// <summary>
        /// Processes data and frees buffers that have been used by the application.
        /// </summary>
        private void ProcessDone()
        {
            IntPtr header;

            // Pull the header data back out of unmanaged memory
            lock (this.bufferingLock)
            {
                header = this.bufferReleaseQueue.Dequeue();
                Monitor.Pulse(this.bufferingLock);
            }

            WAVEHDR pwh = (WAVEHDR)Marshal.PtrToStructure(header, typeof(WAVEHDR));

            // Find and copy the buffer data
            IntPtr data = pwh.lpData;

            // Copy the data and fire the DataReady event if necessary
            if (pwh.dwBytesRecorded > 0 && this.DataReady != null)
            {
                byte[] newData = new byte[pwh.dwBytesRecorded];
                Marshal.Copy(data, newData, 0, (int)pwh.dwBytesRecorded);
                this.DataReady(this, new DataReadyEventArgs(newData));
            }

            // Unprepare the header
            NativeMethods.Throw(
                NativeMethods.waveInUnprepareHeader(this.handle, header, Marshal.SizeOf(typeof(WAVEHDR))),
                NativeMethods.ErrorSource.WaveIn);

            // Free the unmanaged memory
            Marshal.FreeHGlobal(data);
            Marshal.FreeHGlobal(header);
        }

        /// <summary>
        /// Fires when the operating system has a message about a device.
        /// </summary>
        /// <param name="waveInHandle">A handle to the device on which the message has been fired.</param>
        /// <param name="message">The message to be processed.</param>
        /// <param name="instance">A user instance value.</param>
        /// <param name="param1">Message parameter one.</param>
        /// <param name="param2">Message parameter two.</param>
        private void InternalCallback(IntPtr waveInHandle, WaveInMessage message, IntPtr instance, IntPtr param1, IntPtr param2)
        {
            if (message == WaveInMessage.DataReady)
            {
                lock (this.bufferingLock)
                {
                    this.bufferReleaseQueue.Enqueue(param1);
                    this.bufferQueueCount--;
                    Monitor.Pulse(this.bufferingLock);
                }
            }
        }

        /// <summary>
        /// Disposes of the managed and native resources used by this instance.
        /// </summary>
        /// <param name="disposing">true to dispose all resources, false to relase native resources only.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources.
            }

            if (this.handle != null)
            {
                this.Close();
            }
        }
    }
}
