using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ボタン状態はほとんどテストしていない
// 目視で見た方が確実

namespace DD.UnitTest {
    [TestClass]
    public class TestButton {
        [TestMethod]
        public void Test_New_1 () {
            var btn = new Button (ButtonType.Push);

            Assert.AreEqual (ButtonType.Push, btn.Type);
            Assert.AreEqual (ButtonState.Normal, btn.State);
            Assert.AreEqual (null, btn.Normal);
            Assert.AreEqual (null, btn.FocusedNormal);
            Assert.AreEqual (null, btn.Pressed);
            Assert.AreEqual (null, btn.FocusedPressed);
            Assert.AreEqual (false, btn.IsPressed);
            Assert.AreEqual (false, btn.IsFocused);
        }

        [TestMethod]
        public void Test_SetTexture () {
            var btn = new Button (ButtonType.Toggle);
            var tex1 = new Texture ("abstract7.png");
            var tex2 = new Texture ("image2x2.png");
            var tex3 = new Texture ("Explosion4x8.png");
            var tex4 = new Texture ("tmw_desert_spacing.png");

            btn.Normal = tex1;
            btn.Pressed = tex2;
            btn.FocusedNormal = tex3;
            btn.FocusedPressed = tex4;

            Assert.AreEqual (ButtonType.Toggle, btn.Type);
            Assert.AreEqual (tex1, btn.Normal);
            Assert.AreEqual (tex2, btn.Pressed);
            Assert.AreEqual (tex3, btn.FocusedNormal);
            Assert.AreEqual (tex4, btn.FocusedPressed);
        }

        [TestMethod]
        public void Test_Push_Button () {
            var btn = new Button (ButtonType.Push);

            Assert.AreEqual (ButtonState.Normal, btn.State);

            btn.OnMouseButtonPressed (MouseButton.Left, 0, 0);
            Assert.AreEqual (ButtonState.Pressed, btn.State);

            // プッシュ ボタンはリリースすると戻る
            btn.OnMouseButtonReleased (MouseButton.Left, 0, 0);
            Assert.AreEqual (ButtonState.Normal, btn.State);

            btn.OnMouseFocusIn (0, 0);
            Assert.AreEqual (ButtonState.FocusedNormal, btn.State);

            btn.OnMouseFocusOut (0, 0);
            Assert.AreEqual (ButtonState.Normal, btn.State);
        }

        [TestMethod]
        public void Test_Toggle_Button () {
            var btn = new Button (ButtonType.Toggle);

            Assert.AreEqual (ButtonState.Normal, btn.State);

            btn.OnMouseButtonPressed (MouseButton.Left, 0, 0);
            Assert.AreEqual (ButtonState.Pressed, btn.State);

            // トグル ボタンはリリースしても押されたまま
            btn.OnMouseButtonReleased (MouseButton.Left, 0, 0);
            Assert.AreEqual (ButtonState.Pressed, btn.State);

            btn.OnMouseButtonPressed (MouseButton.Left, 0, 0);
            Assert.AreEqual (ButtonState.Normal, btn.State);

            btn.OnMouseFocusIn (0, 0);
            Assert.AreEqual (ButtonState.FocusedNormal, btn.State);

            btn.OnMouseFocusOut (0, 0);
            Assert.AreEqual (ButtonState.Normal, btn.State);
        }

    }
}
