using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

using XnaVector2 = Microsoft.Xna.Framework.Vector2;

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
        float ppm;
        long prev;
        #endregion

        #region ReadOnly
        readonly Vector2 gravity = new Vector2 (0, 9.8f);
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        private Physics2D () {
            this.wld = null;
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
        /// （物理演算用の）世界を作成します。物理エンジンを使用するにはこの世界の構築が必要です。
        /// 引数は物理世界の1mが何ピクセルに相当するかを指定します。
        /// Box2Dでは慣用的に1mは32ピクセルです。 Box2D は 0.1m ～ 10m の範囲がもっとも精度良くシミュレーション可能です。
        /// 物体がこの範囲の外側に飛び出しても計算に問題はありませんが精度が悪くなります。
        /// </remarks>
        /// <param name="ppm">1mあたりのピクセル数(推奨32) [Pixel/Meter]</param>
        public void CreateWorld (float ppm) {
            if (ppm <= 0) {
                throw new ArgumentException ("Pixel/Meter is invalid");
            }
            if (wld != null) {
                this.wld.Clear ();
            }
            this.wld = new FarseerPhysics.Dynamics.World (new XnaVector2 (gravity.X, gravity.Y));
            this.ppm = ppm;
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
        public Vector2 Gravity {
            get {
                if (wld == null) {
                    return gravity;
                }
                return new Vector2(wld.Gravity.X, wld.Gravity.Y);
            }
            set { SetGravity (value.X, value.Y); }
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
        /// 単位はMKS単位系です。初期値は 9.8 (m/s^2) です。
        /// </remarks>
        /// <param name="x">X方向の重力加速度 (m)</param>
        /// <param name="y">Y方向の重力加速度 (m)</param>
        public void SetGravity (float x, float y) {
            if (wld != null) {
                this.wld.Gravity = new XnaVector2 (x, y);
            }
        }

        /// <summary>
        /// 物理エンジンを1ステップ進める
        /// </summary>
        /// <remarks>
        /// 物理エンジンを1ステップ進めます。オブジェクトの位置･回転は計算結果に従って変更されます。
        /// またコリジョン イベントが発生する場合があります。
        /// <seealso cref="Component.OnPhysicsUpdate"/>
        /// <seealso cref="Component.OnCollisionEnter"/>
        /// <seealso cref="Component.OnCollisionExit"/>
        /// </remarks>
        /// <param name="ddworld">DDのワールド</param>
        /// <param name="msec">ワールド時刻 (msec)</param>
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
                body.Position = new XnaVector2 (T.X / PPM, T.Y / PPM);
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
