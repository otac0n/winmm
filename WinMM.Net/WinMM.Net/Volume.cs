//-----------------------------------------------------------------------
// <copyright file="Volume.cs" company="(none)">
//     Copyright (c) 2009 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace WinMM
{
    /// <summary>
    /// Describes a volume setting.
    /// </summary>
    public struct Volume
    {
        /// <summary>
        /// Holds the left volume value.
        /// </summary>
        private float left;

        /// <summary>
        /// Holds the right volume value.
        /// </summary>
        private float right;

        /// <summary>
        /// Initializes a new instance of the Volume struct with the given left and right volume values.
        /// </summary>
        /// <param name="leftVolume">The left volume value.</param>
        /// <param name="rightVolume">The right volume value.</param>
        public Volume(float leftVolume, float rightVolume)
        {
            this.left = leftVolume;
            this.right = rightVolume;
        }

        /// <summary>
        /// Initializes a new instance of the Volume struct with the given volume value.
        /// </summary>
        /// <param name="volume">The left and right volume value.</param>
        public Volume(float volume)
        {
            this.left = volume;
            this.right = volume;
        }

        /// <summary>
        /// Gets or sets the left volume value.
        /// </summary>
        public float Left
        {
            get
            {
                return this.left;
            }

            set
            {
                this.left = value;
            }
        }

        /// <summary>
        /// Gets or sets the right volume value.
        /// </summary>
        public float Right
        {
            get
            {
                return this.right;
            }

            set
            {
                this.right = value;
            }
        }

        /// <summary>
        /// Determines the equality of two Samples.
        /// </summary>
        /// <param name="volume1">The first Volume to compare.</param>
        /// <param name="volume2">The second Volume to compare.</param>
        /// <returns>true, if the Volumes are identical; false, otherwise.</returns>
        public static bool operator ==(Volume volume1, Volume volume2)
        {
            return volume1.Equals(volume2);
        }

        /// <summary>
        /// Determines the inequality of two Samples.
        /// </summary>
        /// <param name="volume1">The first Volume to compare.</param>
        /// <param name="volume2">The second Volume to compare.</param>
        /// <returns>false, if the Volumes are identical; true, otherwise.</returns>
        public static bool operator !=(Volume volume1, Volume volume2)
        {
            return !volume1.Equals(volume2);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare to this instance.</param>
        /// <returns>true if <paramref name="obj"/> has the same value as this instance; false, otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Volume vol = (Volume)obj;
            return this.Left == vol.Left && this.Right == vol.Right;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this Volume instance.</returns>
        public override int GetHashCode()
        {
            return 0;
        }
    }
}
