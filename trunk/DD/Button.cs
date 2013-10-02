using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using Stateless;

namespace DD {
    /// <summary>
    /// ボタン コンポーネント
    /// </summary>
    /// <remarks>
    /// クリック可能なプッシュ ボタンまたはトグル ボタンを実装するコンポーネントです。
    /// ボタン状態には「通常」「通常（フォーカス）」「押下」「押下（フォーカス）」の4つが存在します。
    /// 通常状態と押された状態の2つのテクスチャー指定は必須です。
    /// ボタンが機能するためには同じノードに <see cref="DD.CollisionShape"/> コンポーネントが
    /// アタッチされている事が必要です。
    /// </remarks>
    public class Button : Component {

        private enum ButtonTrigger {
            Press,
            Release,
            FocusIn,
            FocusOut,
        }

        #region Field
        ButtonType type;
        Texture normal;
        Texture focusedNormal;
        Texture pressed;
        Texture focusedPressed;
        Texture current;
        Stateless.StateMachine<ButtonState, ButtonTrigger> sm;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// 指定のサイズの <see cref="Button"/> オブジェクトを作成します。
        /// ボタンはプッシュ ボタンかトグル ボタンを選択可能です。
        /// </remarks>
        /// <param name="type">ボタン種別</param>
        public Button (ButtonType type) {
            this.type = type;
            this.normal = null;
            this.focusedNormal = null;
            this.pressed = null;
            this.focusedPressed = null;
            this.current = normal;
            this.sm = new StateMachine<ButtonState, ButtonTrigger> (ButtonState.Normal);
            switch (type) {
                case ButtonType.Push: {
                        this.sm.Configure (ButtonState.Normal)
                                    .Permit (ButtonTrigger.Press, ButtonState.Pressed)
                                    .Permit (ButtonTrigger.FocusIn, ButtonState.FocusedNormal)
                                    .Ignore (ButtonTrigger.Release)
                                    .Ignore (ButtonTrigger.FocusOut)
                                    .OnEntry (x => this.current = normal);
                        this.sm.Configure (ButtonState.FocusedNormal)
                                      .Permit (ButtonTrigger.Press, ButtonState.FocusedPressed)
                                      .Permit (ButtonTrigger.FocusOut, ButtonState.Normal)
                                      .Ignore (ButtonTrigger.Release)
                                    .OnEntry (x => this.current = focusedNormal ?? normal);
                        this.sm.Configure (ButtonState.Pressed)
                                    .Permit (ButtonTrigger.Release, ButtonState.Normal)
                                    .Permit (ButtonTrigger.FocusIn, ButtonState.FocusedPressed)
                                    .Ignore (ButtonTrigger.Press)
                                    .Ignore (ButtonTrigger.FocusOut)
                                    .OnEntry (x => this.current = pressed);
                        this.sm.Configure (ButtonState.FocusedPressed)
                                    .Permit (ButtonTrigger.Release, ButtonState.FocusedNormal)
                                    .Permit (ButtonTrigger.FocusOut, ButtonState.Pressed)
                                    .Ignore (ButtonTrigger.Press)
                                    .Ignore (ButtonTrigger.FocusIn)
                                    .OnEntry (x => this.current = focusedPressed ?? pressed);
                        break;
                    }
                case ButtonType.Toggle: {
                        this.sm.Configure (ButtonState.Normal)
                                    .Permit (ButtonTrigger.Press, ButtonState.Pressed)
                                    .Permit (ButtonTrigger.FocusIn, ButtonState.FocusedNormal)
                                    .Ignore (ButtonTrigger.Release)
                                    .Ignore (ButtonTrigger.FocusOut)
                                    .OnEntry (x => this.current = normal);
                        this.sm.Configure (ButtonState.FocusedNormal)
                                      .Permit (ButtonTrigger.Press, ButtonState.FocusedPressed)
                                      .Permit (ButtonTrigger.FocusOut, ButtonState.Normal)
                                      .Ignore (ButtonTrigger.Release)
                                    .OnEntry (x => this.current = focusedNormal ?? normal);
                        this.sm.Configure (ButtonState.Pressed)
                                    .Ignore (ButtonTrigger.Release)
                                    .Permit (ButtonTrigger.FocusIn, ButtonState.FocusedPressed)
                                    .Permit (ButtonTrigger.Press, ButtonState.Normal)
                                    .Ignore (ButtonTrigger.FocusOut)
                                    .OnEntry (x => this.current = pressed);
                        this.sm.Configure (ButtonState.FocusedPressed)
                                    .Ignore (ButtonTrigger.Release)
                                    .Permit (ButtonTrigger.FocusOut, ButtonState.Pressed)
                                    .Permit (ButtonTrigger.Press, ButtonState.FocusedNormal)
                                    .Ignore (ButtonTrigger.FocusIn)
                                    .OnEntry (x => this.current = focusedPressed ?? pressed);
                        break;
                    }
                default: throw new NotImplementedException ("Sorry");
            }
        }

        #endregion

        #region Propety

        /// <summary>
        /// ボタン種別
        /// </summary>
        public ButtonType Type {
            get { return type; }
        }

        /// <summary>
        /// ボタン状態
        /// </summary>
        public ButtonState State {
            get { return sm.State; }
        }

        /// <summary>
        /// 押されている状態
        /// </summary>
        public bool IsPressed {
            get { return (sm.State == ButtonState.Pressed || sm.State == ButtonState.FocusedPressed); }
        }

        /// <summary>
        /// フォーカスがあっている状態
        /// </summary>
        public bool IsFocused {
            get { return (sm.State == ButtonState.FocusedNormal || sm.State == ButtonState.FocusedPressed); }
        }

        /// <summary>
        /// 押されていないボタン画像
        /// </summary>
        /// <remarks>
        /// 必須です。
        /// </remarks>
        public Texture Normal {
            get { return normal; }
            set {
                this.normal = value;
                if (current == null) {
                    this.current = normal;
                }
            }
        }

        /// <summary>
        /// 押されたボタン画像（フォーカスあり）
        /// </summary>
        /// <remarks>
        /// フォーカス時に画像を変更する必要がない場合は <c>null</c> のままにしてください。
        /// </remarks>
        public Texture FocusedNormal {
            get { return focusedNormal; }
            set { this.focusedNormal = value; }
        }


        /// <summary>
        /// 押されたボタン画像
        /// </summary>
        /// <remarks>
        /// 必須です。
        /// </remarks>
        public Texture Pressed {
            get { return pressed; }
            set { this.pressed = value; }
        }

        /// <summary>
        /// 押されたボタン画像（フォーカスあり）
        /// </summary>
        /// <remarks>
        /// フォーカス時に画像を変更する必要がない場合は <c>null</c> のままにしてください。
        /// </remarks>
        public Texture FocusedPressed {
            get { return focusedPressed; }
            set { this.focusedPressed = value; }
        }

        #endregion

        #region Method


        /// <inheritdoc/>
        public override void OnDraw (object window) {
            if (current == null) {
                return;
            }

            var win = window as RenderWindow;
            var spr = new SFML.Graphics.Sprite (current.Data);

            Vector3 point;
            Quaternion rotation;
            Vector3 scale;
            Node.GlobalTransform.Decompress (out point, out rotation, out scale);

            // クォータニオンは指定したのと等価な軸が反対で回転角度[0,180]の回転で返ってくる事がある
            // ここで回転軸(0,0,-1)のものを(0,0,1)に変換する必要がある
            var angle = rotation.Angle;
            var axis = rotation.Axis;
            var dot = Vector3.Dot (axis, new Vector3 (0, 0, 1));
            if (dot < 0) {
                angle = 360 - angle;
                axis = -axis;
            }

            spr.Position = new Vector2f (point.X, point.Y);
            spr.Scale = new Vector2f (scale.X, scale.Y);
            spr.Rotation = angle;

            win.Draw (spr);
        }

        /// <inheritdoc/>
        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            // Console.WriteLine ("Clicked");
            sm.Fire (ButtonTrigger.Press);
        }

        /// <inheritdoc/>
        public override void OnMouseButtonReleased (MouseButton button, float x, float y) {
            //Console.WriteLine ("Released");
            sm.Fire (ButtonTrigger.Release);
        }

        /// <inheritdoc/>
        public override void OnMouseFocusIn (float x, float y) {
            //Console.WriteLine ("Focus In");
            sm.Fire (ButtonTrigger.FocusIn);
        }

        /// <inheritdoc/>
        public override void OnMouseFocusOut (float x, float y) {
            //Console.WriteLine ("Focus Out");
            sm.Fire (ButtonTrigger.FocusOut);
        }
        #endregion

    }
}
