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

using InputStateManager;
using InputStateManager.Inputs.InputProviders.Interfaces;
using Microsoft.Xna.Framework.Input;
using Moq;
using NUnit.Framework;

namespace NUnitTests
{
    [TestFixture]
    [Category("InputStateManager")]
    public class KeyTests
    {
        private InputManager input;
        private Mock<IKeyInputProvider> providerMock;

        [SetUp]
        public void Setup()
        {
            providerMock = new Mock<IKeyInputProvider>();
            input = new InputManager(providerMock.Object, null, null, null);
        }

        [Test]
        public void KeyDownTriggers()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.Down(Keys.A));
        }

        [Test]
        public void KeyDownIsResetProperly()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsTrue(input.Key.Is.Down(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.Down(Keys.A));
            input.Update();
            Assert.IsFalse(input.Key.Is.Down(Keys.A));
        }

        [Test]
        public void KeyUpTriggers()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsFalse(input.Key.Is.Up(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.Up(Keys.A));
        }

        [Test]
        public void KeyUpIsResetProperly()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState())
                .Returns(new KeyboardState(Keys.A));
            input.Update();
            Assert.IsFalse(input.Key.Is.Up(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.Up(Keys.A));
            input.Update();
            Assert.IsFalse(input.Key.Is.Up(Keys.A));
        }

        [Test]
        public void KeyPressTriggers()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.Press(Keys.A));
        }

        [Test]
        public void KeyPressIsResetWhenKeyIsReleased()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsTrue(input.Key.Is.Press(Keys.A));
            input.Update();
            Assert.IsFalse(input.Key.Is.Press(Keys.A));
        }

        [Test]
        public void KeyPressIsResetWhenKeyIsStillDown()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.Press(Keys.A));
            input.Update();
            Assert.IsFalse(input.Key.Is.Press(Keys.A));
        }

        [Test]
        public void KeyReleaseTriggers()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsFalse(input.Key.Is.Release(Keys.A));
            input.Update();
            Assert.IsFalse(input.Key.Is.Release(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.Release(Keys.A));
        }

        [Test]
        public void KeyReleaseIsResetProperly()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState())
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsFalse(input.Key.Is.Release(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.Release(Keys.A));
            input.Update();
            Assert.IsFalse(input.Key.Is.Press(Keys.A));
        }

        [Test]
        public void WasDownGivesOldState()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsTrue(input.Key.Is.Down(Keys.A));
            Assert.IsFalse(input.Key.Was.Down(Keys.A));
            input.Update();
            Assert.IsFalse(input.Key.Is.Down(Keys.A));
            Assert.IsTrue(input.Key.Was.Down(Keys.A));
        }

        [Test]
        public void WasUpGivesOldState()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.A))
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsFalse(input.Key.Is.Up(Keys.A));
            Assert.IsTrue(input.Key.Was.Up(Keys.A));
            input.Update();
            Assert.IsTrue(input.Key.Is.Up(Keys.A));
            Assert.IsFalse(input.Key.Was.Up(Keys.A));
        }

        [Test]
        public void AltPressIsTriggeredAndReleasedWithLeftAndRightAlt()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.LeftAlt))
                .Returns(new KeyboardState(Keys.RightAlt))
                .Returns(new KeyboardState(Keys.LeftAlt, Keys.RightAlt))
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsTrue(input.Key.Is.AltPress);
            Assert.IsTrue(input.Key.Is.AltDown);
            input.Update();
            Assert.IsTrue(input.Key.Is.AltPress);
            input.Update();
            Assert.IsTrue(input.Key.Is.AltPress);
            input.Update();
            Assert.IsFalse(input.Key.Is.AltPress);
            Assert.IsTrue(input.Key.Is.AltRelease);
            Assert.IsTrue(input.Key.Is.AltUp);
        }

        [Test]
        public void CtrlPressIsTriggeredAndReleasedWithLeftAndRightCtrl()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.LeftControl))
                .Returns(new KeyboardState(Keys.RightControl))
                .Returns(new KeyboardState(Keys.LeftControl, Keys.RightControl))
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsTrue(input.Key.Is.CtrlPress);
            Assert.IsTrue(input.Key.Is.CtrlDown);
            input.Update();
            Assert.IsTrue(input.Key.Is.CtrlPress);
            input.Update();
            Assert.IsTrue(input.Key.Is.CtrlPress);
            input.Update();
            Assert.IsFalse(input.Key.Is.CtrlPress);
            Assert.IsTrue(input.Key.Is.CtrlRelease);
            Assert.IsTrue(input.Key.Is.CtrlUp);
        }

        [Test]
        public void ShiftPressIsTriggeredAndReleasedWithLeftAndRightShift()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.LeftShift))
                .Returns(new KeyboardState(Keys.RightShift))
                .Returns(new KeyboardState(Keys.LeftShift, Keys.RightShift))
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsTrue(input.Key.Is.ShiftPress);
            Assert.IsTrue(input.Key.Is.ShiftDown);
            input.Update();
            Assert.IsTrue(input.Key.Is.ShiftPress);
            input.Update();
            Assert.IsTrue(input.Key.Is.ShiftPress);
            input.Update();
            Assert.IsFalse(input.Key.Is.ShiftPress);
            Assert.IsTrue(input.Key.Is.ShiftRelease);
            Assert.IsTrue(input.Key.Is.ShiftUp);
        }

        [Test]
        public void WindowsPressIsTriggeredAndReleasedWithLeftAndRightWindows()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(Keys.LeftWindows))
                .Returns(new KeyboardState(Keys.RightWindows))
                .Returns(new KeyboardState(Keys.LeftWindows, Keys.RightWindows))
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsTrue(input.Key.Is.WindowsPress);
            Assert.IsTrue(input.Key.Is.WindowsDown);
            input.Update();
            Assert.IsTrue(input.Key.Is.WindowsPress);
            input.Update();
            Assert.IsTrue(input.Key.Is.WindowsPress);
            input.Update();
            Assert.IsFalse(input.Key.Is.WindowsPress);
            Assert.IsTrue(input.Key.Is.WindowsRelease);
            Assert.IsTrue(input.Key.Is.WindowsUp);
        }

        [Test]
        public void CapslockPressIsTriggeredAndReleased()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(new Keys[] { }, true))
                .Returns(new KeyboardState(new Keys[] { }, true))
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsTrue(input.Key.Is.CapsLockPress);
            Assert.IsTrue(input.Key.Is.CapsLockDown);
            input.Update();
            Assert.IsFalse(input.Key.Is.CapsLockPress);
            Assert.IsTrue(input.Key.Is.CapsLockDown);
            input.Update();
            Assert.IsFalse(input.Key.Is.CapsLockPress);
            Assert.IsTrue(input.Key.Is.CapsLockUp);
        }

        [Test]
        public void NumlockPressIsTriggeredAndReleased()
        {
            providerMock.SetupSequence(o => o.GetState())
                .Returns(new KeyboardState(new Keys[] { }, false, true))
                .Returns(new KeyboardState(new Keys[] { }, false, true))
                .Returns(new KeyboardState());
            input.Update();
            Assert.IsTrue(input.Key.Is.NumLockPress);
            Assert.IsTrue(input.Key.Is.NumLockDown);
            input.Update();
            Assert.IsFalse(input.Key.Is.NumLockPress);
            Assert.IsTrue(input.Key.Is.NumLockDown);
            input.Update();
            Assert.IsFalse(input.Key.Is.NumLockPress);
            Assert.IsTrue(input.Key.Is.NumLockUp);
        }
    }
}