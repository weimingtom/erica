using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace DD.Physics {
    /// <summary>
    /// 2Dの物理エンジン デバイス クラス
    /// </summary>
    /// <remarks>
    /// 2Dの物理演算を行う物理エンジン デバイスのクラスです。
    /// シングルトン化され常に同じインスタンスが返ります。
    /// 物理エンジンではDDDのシーン グラフとは別に独自に物理演算用のワールドを作成します。
    /// 長さの単位は m （メーター）です。基本的に物理エンジン上ではMKS単位系を使用します。
    /// つまりメーター、キログラム、秒で、従って重力加速度は 9.8m/秒 です。
    /// メーターとピクセル数の対応は1mあたりのピクセル数 <see cref="PPM"/>（ピクセル数/メーター）で
    /// 表しますが、ユーザーが直接物理エンジン ワールドに触る事はないのであまり気にする必要はありません。
    /// </remarks>
    public class Physics2D {


        #region Field
        static Physics2D p2d;
        FarseerPhysics.Dynamics.World wld;
        Vector3 gravity;
        float ppm;
        long prev;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        private Physics2D () {
            this.wld = null;
            this.gravity = new Vector3 (0, 9.8f, 0);
            this.prev = 0;
        }

        /// <summary>
        /// <see cref="Physics2D"/> オブジェクトの取得
        /// </summary>
        /// <remarks>
        /// シングルトン化され常に同じインスタンスが返ります。
        /// 現在では2Dの物理エンジンとしてFarseerPhysics(Box2D)を使用しています。
        /// </remarks>
        /// <returns></returns>
        public static Physics2D GetInstance () {
            if (p2d == null) {
                p2d = new Physics2D ();
            }
            return p2d;
        }

        /// <summary>
        /// 物理世界の構築
        /// </summary>
        /// <remarks>
        /// 物理エンジン上に（物理演算用の）世界を作成します。単位はメートル(m)です。
        /// 物理エンジンを使用する前にこの世界を構築してください。
        /// 世界のサイズは「めやす」です。
        /// 物体がこの世界の外側に飛び出しても計算には問題はありません（極端に離れると精度が悪くなる）。
        /// <note>
        /// 現在このサイズは使用せず <see cref="PPM"/> = 32 を固定で使用している。
        /// まあ問題ない。
        /// </note>
        /// </remarks>
        /// <param name="width">幅 (m)</param>
        /// <param name="height">高さ (m)</param>
        /// <param name="depth">奥行き (m)</param>
        public void CreateWorld (float width, float height, float depth) {
            if (width < 0 || height < 0 || depth < 0) {
                throw new ArgumentException ("Size of physics world is invalid");
            }
            if (wld != null) {
                wld.Clear ();
            }
            this.wld = new FarseerPhysics.Dynamics.World (new Vector2 (gravity.X, gravity.Y));
            
            // 慣用的に32ピクセル=1mだが
            // 本当はワールド サイズから計算すべき
            this.ppm = 32;
        }
        #endregion

        #region Propety
        /// <summary>
        /// 重力加速度
        /// </summary>
        /// <remarks>
        /// 重力加速度を取得・設定するプロパティです。
        /// 初期値では 9.8 (m/s^2) です（注意：プラスです）。
        /// </remarks>
        public Vector3 Gravity {
            get { return gravity; }
            set { SetGravity (value.X, value.Y, value.Z); }
        }

        /// <summary>
        /// 1mあたりのピクセル数
        /// </summary>
        /// <remarks>
        /// DDD上のピクセル座標をこの値で割った物が物理エンジン上のサイズです。
        /// 初期値は 32 です。特にこだわりがなければ変更する必要はありません。
        /// </remarks>
        public float PPM {
            get { return ppm; }
            set {
                if (ppm < 1) {
                    throw new ArgumentException ("MPP(Pixel Per Meter) is invalid");
                }
                this.ppm = value;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 物理世界の取得
        /// </summary>
        /// <remarks>
        /// 物理演算で使用するワールドです。Farseer.Dynamics.Worldにキャストしてください。
        /// 通常ユーザーがこのメソッドを使用する事はありません。
        /// </remarks>
        /// <returns></returns>
        public object GetWorld () {
            return wld;
        }

        /// <summary>
        /// 重力加速度の変更
        /// </summary>
        /// <remarks>
        /// 単位はMKS単位系です。初期は 9.8 (m/s^2) です。
        /// </remarks>
        /// <param name="x">X方向 (m)</param>
        /// <param name="y">Y方向 (m)</param>
        /// <param name="z">Z方向 (m)</param>
        public void SetGravity (float x, float y, float z) {
            this.gravity = new Vector3 (x, y, z);
            if (wld != null) {
                wld.Gravity = new Vector2 (x, y);
            }
        }

        /// <summary>
        /// 物理エンジンを1ステップ進める
        /// </summary>
        /// <remarks>
        /// 物理エンジンを1ステップ進めます。
        /// <note>
        /// メモ： タイムステップ(dt)として実測値を使用しているが、これはまずい。
        /// ゲームをポーズしてる時も時間が進んでしまう。
        /// 後で修正する。
        /// </note>
        /// </remarks>
        /// <param name="ddworld">DDのワールド</param>
        public void Step (DD.World ddworld, long msec) {
            if (prev == 0) {
                prev = msec;
            }

            var colliders = from node in ddworld.Downwards
                            from comp in node.Components
                            where comp is Collider
                            select (Collider)comp;
            
            foreach (var col in colliders) {

                Vector3 T;
                Quaternion R;
                Vector3 S;
                col.Node.GlobalTransform.Decompress (out T, out R, out S);

                // クォータニオンの性質上(0,0,1)軸まわりの回転か、
                // (0,0,-1)軸まわりの回転のどちらかが返ってくる（両者は等価）。
                // Box2Dは(0,0,1)軸まわりの回転角で指定するので(0,0,-1)軸の時は反転が必要。
                var angle = R.Angle / 180f * (float)Math.PI;
                if (R.Axis.Z < 0) {
                    angle *= -1;
                }

                var body = (Body)col.Body;
                body.Position = new Vector2 (T.X / PPM, T.Y / PPM);
                body.Rotation = angle;
            }
            

            float dt = (msec - prev) / 1000f;
            wld.Step (dt);

            foreach (var node in ddworld.Downwards) {
                foreach (var comp in node.Components) {
                    comp.OnPhysicsUpdate ();
                }
            }

            this.prev = msec;
        }
        #endregion
    }
}
