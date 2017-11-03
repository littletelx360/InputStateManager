﻿// *************************************************************************** 
// This is free and unencumbered software released into the public domain.
// 
// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a compiled
// binary, for any purpose, commercial or non-commercial, and by any
// means.
// 
// In jurisdictions that recognize copyright laws, the author or authors
// of this software dedicate any and all copyright interest in the
// software to the public domain. We make this dedication for the benefit
// of the public at large and to the detriment of our heirs and
// successors. We intend this dedication to be an overt act of
// relinquishment in perpetuity of all present and future rights to this
// software under copyright law.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org>
// ***************************************************************************

using System;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Inputs.Inputs.Mouse
{
    [PublicAPI]
    public class Mouse
    {
        public enum Button
        {
            LEFT,
            RIGHT,
            MIDDLE,
            X_BUTTON1,
            X_BUTTON2
        }

        public MouseState OldState { get; set; }
        public MouseState State { get; set; }

        /// <summary>
        ///     Gets information about the current state. Including calculated delta values.
        /// </summary>
        public IsSub Is { get; }

        /// <summary>
        ///     Gets information about the previous state. No delta values included, since it has no 'old' state to refer to.
        /// </summary>
        public WasSub Was { get; }

        internal Mouse()
        {
            Is = new IsSub(GetState, GetOldState);
            Was = new WasSub(GetOldState);
        }

        internal MouseState GetState() => State;
        internal MouseState GetOldState() => OldState;

        internal void Update()
        {
            OldState = State;
            State = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

        internal static bool IsUp(MouseState state, Button button)
        {
            switch (button)
            {
                case Button.LEFT:
                    return state.LeftButton == ButtonState.Released;
                case Button.MIDDLE:
                    return state.MiddleButton == ButtonState.Released;
                case Button.RIGHT:
                    return state.RightButton == ButtonState.Released;
                case Button.X_BUTTON1:
                    return state.XButton1 == ButtonState.Released;
                case Button.X_BUTTON2:
                    return state.XButton2 == ButtonState.Released;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }
        }

        internal static bool IsDown(MouseState state, Button button)
        {
            switch (button)
            {
                case Button.LEFT:
                    return state.LeftButton == ButtonState.Pressed;
                case Button.MIDDLE:
                    return state.MiddleButton == ButtonState.Pressed;
                case Button.RIGHT:
                    return state.RightButton == ButtonState.Pressed;
                case Button.X_BUTTON1:
                    return state.XButton1 == ButtonState.Pressed;
                case Button.X_BUTTON2:
                    return state.XButton2 == ButtonState.Pressed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }
        }

        [PublicAPI]
        public class IsSub : WasSub
        {
            private Func<MouseState> State { get; set; }
            private Func<MouseState> OldState { get; set; }

            internal IsSub(Func<MouseState> mapping, Func<MouseState> oldMapping) : base(mapping)
            {
                State = mapping;
                OldState = oldMapping;
            }

            public bool Press(Button button)
                => IsDown(State(), button) && IsUp(OldState(), button);

            public bool Release(Button button)
                => IsDown(OldState(), button) && IsUp(State(), button);

            public Point PositionDelta => OldState().Position - State().Position;
            public int ScrollWheelDelta => State().ScrollWheelValue - OldState().ScrollWheelValue;

            public int XDelta => State().X - OldState().X;
            public int YDelta => State().Y - OldState().Y;
        }

        [PublicAPI]
        public class WasSub
        {
            private Func<MouseState> State { get; set; }

            internal WasSub(Func<MouseState> mapping)
            {
                State = mapping;
            }

            public bool Up(Button button) => IsUp(State(), button);
            public bool Down(Button button) => IsDown(State(), button);

            public Point Position => State().Position;
            public int ScrollWheelValue => State().ScrollWheelValue;
            public int X => State().X;
            public int Y => State().Y;
        }
    }
}