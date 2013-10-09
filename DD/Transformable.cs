using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {

    /// <summary>
    /// ジオメトリ変換
    /// </summary>
    /// <remarks>
    /// ノードをジオメトリ変換可能にする実装。
    /// って意味不明だな。
    /// </remarks>
    public abstract class Transformable {
        #region Field
        float tx;   // Vector3で良かった・・・
        float ty;
        float tz;
        Quaternion rot;
        float sx;   // Vector3で良かった・・・
        float sy;
        float sz;
        Matrix4x4? matrix;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public Transformable () {
            this.tx = 0;
            this.ty = 0;
            this.tz = 0;
            this.sx = 1;
            this.sy = 1;
            this.sz = 1;
            this.rot = Quaternion.Identity;
            this.matrix = null;
        }
        #endregion

        #region Propety
        /// <summary>
        /// 位置X（ローカル座標系）
        /// </summary>
        public float X {
            get { return tx; }
        }

        /// <summary>
        /// 位置Y（ローカル座標系）
        /// </summary>
        public float Y {
            get { return ty; }
        }

        /// <summary>
        /// 位置Z（ローカル座標系）
        /// </summary>
        public float Z {
            get { return tz; }
        }

        /// <summary>
        /// 位置（ローカル座標系）
        /// </summary>
        public Vector3 Translation {
            get { return new Vector3 (tx, ty, tz); }
            set { SetTranslation (value.X, value.Y, value.Z); }
        }



        /// <summary>
        /// スケール成分（ローカル座標系）
        /// </summary>
        public Vector3 Scale {
            get { return new Vector3 (sx, sy, sz); }
            set { SetScale (value.X, value.Y, value.Z);}
        }

        /// <summary>
        /// 回転成分（ローカル座標系）
        /// </summary>
        public Quaternion Rotation {
            get { return rot; }
            set { SetRotation (value); }
        }

        /// <summary>
        /// TRS複合変換行列
        /// </summary>
        /// <remarks>
        /// このプロパティは T:平行移動、R：回転、S:スケール を1つの  <see cref="Matrix4x4"/> 型の複合変換行列にしたものです。
        /// この行列はローカル座標系から1つ上の親ノードの座標系に変換します。
        /// このプロパティは計算結果をキャッシュし、2回目以降は高速に動作します。
        /// </remarks>
        public Matrix4x4 Transform {
            get {
                if (matrix == null) {
                    var T = Matrix4x4.CreateFromTranslation (tx, ty, tz);
                    var R = Matrix4x4.CreateFromRotation (rot);
                    var S = Matrix4x4.CreateFromScale (sx, sy, sz);
                    this.matrix = T * R * S;
                }
                return matrix.Value;
            }
            set {
                Vector3 T;
                Quaternion R;
                Vector3 S;
                value.Decompress (out T, out R, out S);
                this.tx = T.X;
                this.ty = T.Y;
                this.tz = T.Z;
                this.rot = R;
                this.sx = S.X;
                this.sy = S.Y;
                this.sz = S.Z;
                this.matrix = value;
            }
        }
        #endregion


        #region Method

        /// <summary>
        /// 変換行列キャッシュの破棄
        /// </summary>
        /// <remarks>
        /// <see cref="Transformable"/> および <see cref="Node"/> は高速化のために一度計算した変換行列をキャッシュして再利用します。
        /// このメソッドはキャッシュを破棄し次回取得する際に再計算させます。
        /// ユーザーがこのメソッドを呼ぶ必要はありません。
        /// <note type="implement">
        /// このメソッドをオーバーライドした場合、かならずオーバーライドされた基底クラスのメソッドを呼び出してください。
        /// </note>
        /// </remarks>
        public virtual void InvalidateTransformCache () {
            this.matrix = null;
        }

        /// <summary>
        /// 平行移動量の変更（ローカル座標）
        /// </summary>
        /// <remarks>
        /// それまでセットされていたこのノードの平行移動量を破棄し、新しい値に変更します。
        /// </remarks>
        /// <param name="tx">X方向の平行移動量</param>
        /// <param name="ty">Y方向の平行移動量</param>
        /// <param name="tz">Z方向の平行移動量</param>
        public void SetTranslation (float tx, float ty, float tz) {
            this.tx = tx;
            this.ty = ty;
            this.tz = tz;

            InvalidateTransformCache ();
        }

        /// <summary>
        /// 回転成分の変更（ローカル座標）
        /// </summary>
        /// <remarks>
        /// 回転角度は度数(degree)で指定します。値に制限はありません。0度以下や360度以上も可能です。
        /// 回転軸は正規化されている必要はありません。
        /// </remarks>
        /// <param name="angle">回転角度 [0,360)</param>
        /// <param name="ax">回転軸X</param>
        /// <param name="ay">回転軸Y</param>
        /// <param name="az">回転軸Z</param>
        public void SetRotation (float angle, float ax, float ay, float az) {
            if (angle != 0 && (ax == 0 && ay == 0 && az == 0)) {
                throw new ArgumentException ("Rotation Axis is invalid");
            }
            this.rot = new Quaternion (angle, ax, ay, az);
            
            InvalidateTransformCache ();
        }

        /// <summary>
        /// 回転成分の変更
        /// </summary>
        /// <param name="rot">回転を表すクォータニオン</param>
        public void SetRotation (Quaternion rot) {
            if (1 - rot.Length > 0.01f) {
                throw new ArgumentException ("Quaternion is not normaized");
            }

            this.rot = rot;

            InvalidateTransformCache ();
        }

        /// <summary>
        /// スケール成分の変更
        /// </summary>
        /// <param name="sx">X方向の拡大率</param>
        /// <param name="sy">Y方向の拡大率</param>
        /// <param name="sz">Z方向の拡大率</param>
        public void SetScale (float sx, float sy, float sz) {
            this.sx = sx;
            this.sy = sy;
            this.sz = sz;

            InvalidateTransformCache ();
        }
        
        /// <summary>
        /// ノードの移動
        /// </summary>
        /// <remarks>
        /// 現在位置から引数で指定された分だけ移動します。
        /// </remarks>
        /// <param name="x">X方向の移動距離（ピクセル数）</param>
        /// <param name="y">Y方向の移動距離（ピクセル数）</param>
        /// <param name="z">Z方向の移動距離（ピクセル数）</param>
        public void Translate (float x, float y, float z) {
            this.tx += x;
            this.ty += y;
            this.tz += z;
        
            InvalidateTransformCache ();
        }

        /// <summary>
        /// ノードの回転
        /// </summary>
        /// <remarks>
        /// 現在セットされている回転にさらに、引数で指定された回転（度数）を加えます。
        /// </remarks>
        /// <param name="angle">回転角度 [0,360)</param>
        /// <param name="ax">回転軸X</param>
        /// <param name="ay">回転軸Y</param>
        /// <param name="az">回転軸Z</param>
        public void Rotate (float angle, float ax, float ay, float az) {
            if (ax == 0 && ay == 0 && az == 0) {
                throw new ArgumentException ("Rotation axis is invalid");
            }
            Rotate (new Quaternion (angle, ax, ay, az));
        }

        /// <summary>
        /// ノードの回転
        /// </summary>
        /// <remarks>
        /// 現在セットされている回転にさらに、引数で指定された回転（度数）を加えます。
        /// </remarks>
        /// <param name="angle">回転角度 [度数] </param>
        /// <param name="axis">回転軸</param>
        public void Rotate (float angle, Vector3 axis) {
            if (axis.Length2 == 0) {
                throw new ArgumentException ("Rotation axis is invalid");
            }
            Rotate (new Quaternion (angle, axis));
        }

        /// <summary>
        /// ノードの回転
        /// </summary>
        /// <remarks>
        /// 現在セットされている回転にさらに、引数で指定された回転（度数[0,360]）を加えます。
        /// </remarks>
        /// <param name="q">回転を表すクォータニオン</param>
        public void Rotate (Quaternion q) {
            this.rot = q * this.rot;
        
            InvalidateTransformCache ();
        }


        /// <summary>
        /// ノードの拡大・縮小
        /// </summary>
        /// <remarks>
        /// 現在の拡大・縮小率に引数で指定された拡大縮小率を掛けます。
        /// </remarks>
        /// <param name="sx">X方向の拡大・縮小率</param>
        /// <param name="sy">Y方向の拡大・縮小率</param>
        /// <param name="sz">Z方向の拡大・縮小率</param>
        public void Expand (float sx, float sy, float sz) {
            if (sx < 0 || sy < 0 || sz < 0) {
                throw new ArgumentException ("(sx,sy,sz) is invalid");
            }
            this.sx *= sx;
            this.sy *= sy;
            this.sz *= sz;
        
            InvalidateTransformCache ();
        }

        #endregion

    }
}
