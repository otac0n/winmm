//-----------------------------------------------------------------------
// <copyright file="MainView.cs" company="(none)">
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

namespace EchoDemo
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using WinMM;

    /// <summary>
    /// Demo form for the WinMM.Net library.
    /// </summary>
    public partial class MainView : Form
    {
        /// <summary>
        /// Hold a locking object for disposing and writing to the waveIn and waveOut objects.
        /// </summary>
        private object actionLock = new object();

        /// <summary>
        /// Holds the object used to read data from the microphone.
        /// </summary>
        private WaveIn waveIn;

        /// <summary>
        /// Holds the object used to write data to the speakers.
        /// </summary>
        private WaveOut waveOut;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainView" /> class.
        /// </summary>
        public MainView()
        {
            InitializeComponent();
            
            // We are using a hard-coded format that should be supported by all consumer sound devices available today.
            WaveFormat format = WaveFormat.Pcm44Khz16BitMono;

            // We intialize the devices to the use the wave mapper devices, provided by Microsoft.
            // This device is built-in, and know to support the above specified format.
            this.waveIn = new WaveIn(WaveIn.WaveInMapperDeviceId);
            this.waveOut = new WaveOut(WaveOut.WaveOutMapperDeviceId);

            // We must start the wave-out device before we start the wave-in device, otherwise, the wave-in device may have data available
            // before the wave-out device is ready to play it.
            this.waveOut.Open(format);

            // Tweaking these values affects the internal buffering thread.
            // Setting too small of a QueueSize or too small of a BufferSize will cause buffer underruns, which will sound like choppy audio.
            // Setting too large of a BufferSize will increase the audio latency.
            // Setting a larger QueueSize increases memory usage.
            this.waveIn.BufferQueueSize = 200;
            this.waveIn.BufferSize = 64;
            this.waveIn.DataReady += new EventHandler<DataReadyEventArgs>(this.WaveIn_DataReady);
            this.waveIn.Open(format);
            this.waveIn.Start();

            // Next, we obtain each devices' capabilities.
            var waveInCaps = this.waveIn.Capabilities;
            var waveOutCaps = this.waveOut.Capabilities;

            // And, finally, we display the devices' information to the user.
            this.InDevice.Text = waveInCaps.Name + " (" + waveInCaps.Manufacturer + ")";
            this.InChannels.Text = waveInCaps.Channels.ToString() + " channels, driver version " + waveInCaps.DriverVersion;
            this.OutDevice.Text = waveOutCaps.Name + " (" + waveOutCaps.Manufacturer + ")";
            this.OutChannels.Text = waveOutCaps.Channels.ToString() + " channels, driver version " + waveOutCaps.DriverVersion;
        }

        /// <summary>
        /// Describes a delegate used to update the level-indicator bar.
        /// </summary>
        /// <param name="max">The value to which the bar should be set.</param>
        private delegate void SetLevelDelegate(int max);

        /// <summary>
        /// The callback function given to the <see cref="WinMM.MainView" /> device, to notify us when data is ready and pass that data back to us.
        /// </summary>
        /// <param name="sender">The object calling this event.</param>
        /// <param name="e">The arguments to the event.  This includes the data returned from the <see cref="WinMM.MainView" /> device.</param>
        private void WaveIn_DataReady(object sender, DataReadyEventArgs e)
        {
            // First, we pass the data directly along to the wave-out device for immediate playback.
            lock (this.actionLock)
            {
                if (this.waveOut != null)
                {
                    this.waveOut.Write(e.Data);
                }
            }

            // Then, we read the data as a binary stream, and find the maximum level of the sample.
            int max = 0;

            using (var ms = new MemoryStream(e.Data))
            {
                using (var br = new BinaryReader(ms))
                {
                    while (ms.Position != ms.Length)
                    {
                        max = Math.Max(max, Math.Abs((int)br.ReadInt16()));
                    }
                }
            }

            // Finally, we instruct the progress bar to show the new maximum value.
            this.SetLevel(max);
        }

        /// <summary>
        /// Sets the level indicator value in a thread-safe way.
        /// </summary>
        /// <param name="max">The value to which the indicator should be set.</param>
        private void SetLevel(int max)
        {
            if (this.LevelBar.InvokeRequired)
            {
                this.LevelBar.Invoke(new SetLevelDelegate(this.SetLevel), max);
            }
            else
            {
                this.LevelBar.Value = max;
            }
        }

        /// <summary>
        /// Frees up resources used by the form.
        /// </summary>
        /// <param name="sender">The object calling this event.</param>
        /// <param name="e">The arguments to the event.</param>
        private void MainView_FormClosed(object sender, FormClosedEventArgs e)
        {
            lock (this.actionLock)
            {
                this.waveIn.Dispose();
                this.waveIn = null;
            }

            lock (this.actionLock)
            {
                this.waveOut.Dispose();
                this.waveOut = null;
            }
        }
    }
}
