namespace WinMM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;

    public class JoystickState
    {
        public class ButtonState : IEnumerable<bool>
        {
            private readonly bool[] buttonFlags;

            internal ButtonState(bool[] buttonFlags)
            {
                this.buttonFlags = buttonFlags;
            }

            public bool this[int i]
            {
                get
                {
                    return this.buttonFlags[i];
                }
            }

            public int Length
            {
                get
                {
                    return this.buttonFlags.Length;
                }
            }

            public IEnumerator<bool> GetEnumerator()
            {
                foreach (var b in this.buttonFlags)
                {
                    yield return b;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.buttonFlags.GetEnumerator();
            }
        }

        public ButtonState Buttons
        {
            get;
            private set;
        }
    }
}