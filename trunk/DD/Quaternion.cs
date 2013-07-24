using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// クォータニオン構造体
    /// </summary>
    /// <remarks>
    /// 回転を表すクォータニオン構造体です。
    /// <see cref="X"/>,<see cref="Y"/>,<see cref="Z"/>の3つが虚数のベクトル要素を表し、
    /// <see cref="W"/>が実数のスカラー要素を表します。
    /// 一部を除きクォータニオンの長さは常に１を仮定しています。
    /// 通常内部で自動的に正規化されるので特に気にする必要はありません。
    /// 等価な回転は２つある事に注意してください。
    /// 例えば 90,(0,0,1)と -90,(0,0,-1) は同じ回転を表します。
    /// クォータニオンをAngle-Axis形式に戻すと基本的に前者が返ります（角度が0～360）。
    /// クォータニオンについて詳細は例えば「ゲームプログラミングのための3Dグラフィックス数学」を参照してください。
    /// この実装はJason Gregoryの「Game Engine Architecture」をそのまま利用しています。
    /// </remarks>
    public struct Quaternion : IEquatable<Quaternion> {

        #region Constructor
        /// <summary>
        ///  Angle-Axis形式で<see cref="Quaternion"/> オブジェクトを作成するコンストラクター
        /// </summary>       
        /// <remarks>
        /// 角度と回転軸を指定して <see cref="Quaternion"/> オブジェクトを作成します。
        /// 回転角は度数(degree)で指定し、値に制限はありません。回転軸は正規化されている必要はありません。
        /// 作成されるクォータニオンは正規化済みです。
        /// <c>Angle=0,Axis=(0,0,0)</c>はゼロクォータニオンを示す特殊な組み合わせで、このときに限り長さが0の回転軸を受け付けます。
        /// </remarks>
        /// <param name="angle">回転角度(in degree[0,360))</param>
        /// <param name="ax">回転軸のX要素</param>
        /// <param name="ay">回転軸のY要素</param>
        /// <param name="az">回転軸のZ要素</param>
        /// <returns></returns>
        public Quaternion (float angle, float ax, float ay, float az)
            : this () {
            if (angle != 0 && ax == 0 && ay == 0 && az == 0) {
                throw new ArgumentException ("Axis is (0,0,0), but Angle is not 0");
            }

            if (ax == 0 && ay == 0 && az == 0) {
                this.X = 0;
                this.Y = 0;
                this.Z = 0;
                this.W = 1;
            }
            else {
                var axis = new Vector3 (ax, ay, az).Normalize ();
                var theta = (Math.PI * angle / 180.0);
                
                this.X = (float)(axis.X * Math.Sin (theta / 2.0));
                this.Y = (float)(axis.Y * Math.Sin (theta / 2.0));
                this.Z = (float)(axis.Z * Math.Sin (theta / 2.0));
                this.W = (float)(Math.Cos (theta / 2.0));
            }
        }

        /// <summary>
        /// Angle-Axis形式で<see cref="Quaternion"/> オブジェクトを作成するコンストラクター
        /// </summary>
        /// <remarks>
        /// 角度と回転軸を指定して <see cref="Quaternion"/> オブジェクトを作成します。
        /// 指定する回転軸は正規化されている必要はありません。回転角は度数で <c>[0,360)</c> の範囲で指定します。
        /// 返されたクォータニオンは正規化済みです。
        /// <c>Angle=0,Axis=(0,0,0)</c>はゼロクォータニオンを示す特殊な組み合わせで、このときに限り長さが0の回転軸を受け付けます。
        /// </remarks>
        /// <param name="angle">回転角度 [0,360)</param>
        /// <param name="axis">回転軸</param>
        public Quaternion (float angle, Vector3 axis) : this(angle, axis.X, axis.Y, axis.Z){

        }

        #endregion

        #region Property
        /// <summary>
        /// X要素
        /// </summary>
        public float X {
            get;
            private set;
        }

        /// <summary>
        /// Y要素
        /// </summary>
        public float Y {
            get;
            private set;
        }

        /// <summary>
        /// Z要素
        /// </summary>
        public float Z {
            get;
            private set;
        }

        /// <summary>
        /// W要素
        /// </summary>
        public float W {
            get;
            private set;
        }

        /// <summary>
        /// コンポーネント数
        /// </summary>
        public int ComponentCount {
            get { return 4; }
        }

        /// <summary>
        /// コンポーネントにアクセスするインデクサー
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns></returns>
        public float this[int index] {
            get {
                switch (index) {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    case 3: return W;
                    default: throw new IndexOutOfRangeException ("Index is out of range");
                }
            }
            internal set {
                switch (index) {
                    case 0: this.X = value; break;
                    case 1: this.Y = value; break;
                    case 2: this.Z = value; break;
                    case 3: this.W = value; break;
                    default: throw new IndexOutOfRangeException ("Index is out of range");
                }
            }
        }

        /// <summary>
        /// クォータニオンの長さ
        /// </summary>
        public float Length {
            get {
                return (float)Math.Sqrt (X * X + Y * Y + Z * Z + W * W);
            }
        }

        /// <summary>
        /// クォータニオンの長さの自乗
        /// </summary>
        public float Length2 {
            get {
                return X * X + Y * Y + Z * Z + W * W;
            }
        }

        /// <summary>
        /// このクォータニオンと等価なAngle-Axis形式の回転角度 (in degree)
        /// </summary>
        /// <remarks>
        /// Angle-Axis形式で表したこのクォータニオンの回転角度を返します。
        /// 回転角度は度数 [0,360) の範囲で返ります。
        /// </remarks>
        public float Angle {
            get {
                return (float)(2 * Math.Acos (W) / Math.PI * 180);
            }
        }

        /// <summary>
        /// このクォータニオンと等価なAngleAxis形式の回転軸
        /// </summary>
        /// <remarks>
        /// Angle-Axis形式で表したこのクォータニオンの回転軸を返します。
        /// 返される回転軸は必ず正規化されています。
        /// ただし回転角度が0の場合は特殊で回転軸として(0,0,0)が返ります。
        /// <note>
        /// ゼロクォータニオンとして Angle=0, Axis=(0,0,0) を返すのは正しい仕様か？
        /// </note>
        /// </remarks>
        public Vector3 Axis {
            get {
                if (X == 0 && Y == 0 && Z == 0 && W == 1) {
                    return new Vector3 (0, 0, 0);
                }
                return new Vector3 (X, Y, Z).Normalize ();
            }
        }

        /// <summary>
        /// このクォータニオンと等価な回転行列
        /// </summary>
        /// <remarks>
        /// このクォータニオンと等価な回転行列を返します。
        /// </remarks>
        public Matrix3x3 Matrix3x3 {
            get {
                var m00 = 1 - 2 * Y * Y - 2 * Z * Z;
                var m01 = 2 * X * Y - 2 * W * Z;
                var m02 = 2 * X * Z + 2 * W * Y;
                var m10 = 2 * X * Y + 2 * W * Z;
                var m11 = 1 - 2 * X * X - 2 * Z * Z;
                var m12 = 2 * Y * Z - 2 * W * X;
                var m20 = 2 * X * Z - 2 * W * Y;
                var m21 = 2 * Y * Z + 2 * W * X;
                var m22 = 1 - 2 * X * X - 2 * Y * Y;
                return new Matrix3x3 (m00, m01, m02,
                                      m10, m11, m12,
                                      m20, m21, m22);
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// 単位クォータニオンを作成します
        /// </summary>
        /// <remarks>
        /// 単位クォータニオン((0,0,0),1)は回転角度0の回転を表します。
        /// このクォータニオンを何回適応してもベクトルは変化しません。
        /// また単位クォータニオンの逆数と共益は単位クォータニオンになります。
        /// </remarks>
        public static Quaternion Identity {
            get {
                var q = new Quaternion ();
                q.X = 0;
                q.Y = 0;
                q.Z = 0;
                q.W = 1;
                return q;
            }
        }

        /// <summary>
        /// 数値を直接指定してクォータニオンを作成
        /// </summary>
        /// <remarks>
        /// 引数の <paramref name="normalize"/> フラグを true に指定すると正規化します。
        /// ユーザーは自分が何をやっているのかよく理解している場合を除き、このメソッドを使うべきではありません。
        /// </remarks>
        /// <param name="x">X要素(ベクトル成分i)</param>
        /// <param name="y">Y要素(ベクトル成分j)</param>
        /// <param name="z">Z要素(ベクトル成分k)</param>
        /// <param name="w">W要素(スカラー成分)</param>
        /// <param name="normalize">正規化フラグ</param>
        /// <returns>作成したクォータニオン</returns>
        public static Quaternion Set (float x, float y, float z, float w, bool normalize) {
            var q = new Quaternion ();
            q.X = x;
            q.Y = y;
            q.Z = z;
            q.W = w;
            if (normalize) {
                q = q.Normalize ();
            }
            return q;
        }


        /// <summary>
        /// 回転行列から等価なクォータニオンの作成
        /// </summary>
        /// <remarks>
        /// 回転行列からクォータニオンが計算できない場合、単位クォータニオンが返ります。
        /// </remarks>
        /// <param name="m">回転行列</param>
        /// <returns>クォータニオン</returns>
        public static Quaternion CreateFromMatrix (Matrix3x3 m) {
            var q = new Quaternion ();
            var trace = m[0] + m[4] + m[8];
            if (trace > 0) {
                var s = (float)Math.Sqrt (trace + 1.0f);
                var t = 0.5f / s;
                q[0] = (m[7] - m[5]) * t;
                q[1] = (m[2] - m[6]) * t;
                q[2] = (m[3] - m[1]) * t;
                q[3] = s * 0.5f;
            }
            else {
                var i = 0;
                i = (m[4] > m[0]) ? 1 : i;
                i = (m[8] > m[i * 4]) ? 2 : i;
                var j = (i + 1) % 3;
                var k = (j + 1) % 3;
                var s = (float)Math.Sqrt ((m[i * 4] - (m[j * 4] + m[k * 4])) + 1.0f);
                var t = (s != 0) ? 0.5f / s : s;
                q[i] = s * 0.5f;
                q[j] = (m[j * 3 + i] + m[i * 3 + j]) * t;
                q[k] = (m[k * 3 + i] + m[i * 3 + k]) * t;
                q[3] = (m[k * 3 + j] - m[j * 3 + k]) * t;
            }
            return q;
        }
 
        /// <summary>
        /// 逆クォータニオン
        /// </summary>
        /// <remarks>
        /// 長さが１のクォータニオンの逆数は共役のクォータニオンと等しくなります。
        /// 逆数を取る前のクォータニオンが正規化されていれば、戻り値のクォータニオンも正規化されています。
        /// そうでなければ正規化されていません。
        /// クォータニオンとその逆クォータニオンをかけると単位クォータニオンになります。
        /// </remarks>
        /// <returns>作成された逆クォータニオン</returns>
        public Quaternion Inverse () {
            if (Length2 == 0) {
                throw new ArithmeticException ("Can't calculate inverse of this Quaternion");
            }
            return Quaternion.Set (-X / Length2, -Y / Length2, -Z / Length2, W / Length2, false);
        }

        /// <summary>
        /// 共役のクォータニオン
        /// </summary>
        /// <remarks>
        /// 逆数を取る前のクォータニオンが正規化されていれば、戻り値のクォータニオンも正規化されています。
        /// そうでなければ正規化されていません。
        /// 通常ユーザーがこのメソッドを使用することはありません。
        /// </remarks>
        /// <returns>作成された共役のクォータニオン</returns>
        public Quaternion Conjugate () {
            return Quaternion.Set (-X, -Y, -Z, W, false);
        }

        /// <summary>
        /// クォータニオンの対数
        /// </summary>
        /// <remarks>
        /// 戻り値のクォータニオンは正規化されていません。
        /// 通常ユーザーがこのメソッドを使用することはありません。
        /// </remarks>
        /// <returns>作成された対数クォータニオン</returns>
        public Quaternion Log () {
            var q = new Quaternion ();
            var a = (float)Math.Acos (W);
            if (a > 0) {
                q[0] = a * X / (float)Math.Sin (a);
                q[1] = a * Y / (float)Math.Sin (a);
                q[2] = a * Z / (float)Math.Sin (a);
                q[3] = 0;
            }
            return q;
        }

        /// <summary>
        /// クォータニオンの内積
        /// </summary>
        /// <remarks>
        /// クォータニオンの内積を計算します。
        /// 通常ユーザーがこのメソッドを使用することはありません。
        /// </remarks>
        /// <param name="q1">クォータニオン1</param>
        /// <param name="q2">クォータニオン2</param>
        /// <returns>内積</returns>
        public static float Dot (Quaternion q1, Quaternion q2) {
            return q1[0] * q2[0] + q1[1] * q2[1] + q1[2] * q2[2] + q1[3] * q2[3];
        }

        /// <summary>
        /// クォータニオンの float の積
        /// </summary>
        /// <remarks>
        /// 戻り値のクォータニオンは正規化されていません。
        /// 通常このメソッドをユーザーが使用する事はありません。
        /// </remarks>
        /// <param name="q">クォータニオン</param>
        /// <param name="f">float値</param>
        /// <returns></returns>
        public static Quaternion operator * (Quaternion q, float f) {
            return Quaternion.Set (q[0] * f, q[1] * f, q[2] * f, q[3] * f, false);
        }

        /// <summary>
        /// float とクォータニオンの積
        /// </summary>
        /// <remarks>
        /// 戻り値のクォータニオンは正規化されていません。
        /// 通常このメソッドをユーザーが使用する事はありません。
        /// </remarks>
        /// <param name="f">float値</param>
        /// <param name="q">クォータニオン</param>
        /// <returns></returns>
        public static Quaternion operator * (float f, Quaternion q) {
            return q * f;
        }

        /// <summary>
        /// クォータニオンと float の割り算
        /// </summary>
        /// <remarks>
        /// ゼロ割はNAN化します。
        /// 戻り値のクォータニオンは正規化されていません。
        /// 通常このメソッドをユーザーが使用する事はありません。
        /// </remarks>
        /// <param name="q">クォータニオン</param>
        /// <param name="f">float値</param>
        /// <returns></returns>
        public static Quaternion operator / (Quaternion q, float f) {
            return q * (1 / f);
        }

        /// <summary>
        /// クォータニオン同士の足し算
        /// </summary>
        /// <remarks>
        /// 戻り値のクォータニオンは正規化されていません。
        /// 通常このメソッドをユーザーが使用する事はありません。
        /// </remarks>
        /// <param name="q1">クォータニオン1</param>
        /// <param name="q2">クォータニオン2</param>
        /// <returns></returns>
        public static Quaternion operator + (Quaternion q1, Quaternion q2) {
            if (q1 == null || q2 == null) {
                throw new ArgumentNullException ("Quaternion is null");
            }
            return Quaternion.Set (q1[0] + q2[0], q1[1] + q2[1], q1[2] + q2[2], q1[3] + q2[3], false);
        }


        /// <summary>
        /// クォータニオンの球面補間
        /// </summary>
        /// <remarks>
        /// 引数 s=0 の時クォータニオン <paramref name="q1"/> に、s=1 の時クォータニオン <paramref name="q2"/> に等しくなります。
        /// 球面補完が計算できないクォータニオン (4次元空間で2つのクォータニオンの角度が0または180度）の場合は s の値にかかわらず q1 が返ります。
        /// このメソッドが例外やNANを返す事はありません。
        /// 補間結果は常にわずかな計算誤差を含みます。
        /// クォータニオンの定義から長さが１でないクォータニオンを球面補間した場合、結果はそれほど正しくありません。
        /// </remarks>
        /// <param name="s">補間位置[0,1]</param>
        /// <param name="q1">クォータニオン1 (s=0)</param>
        /// <param name="q2">クォータニオン2 (s=1)</param>
        /// <returns>補完されたクォータニオン</returns>
        public static Quaternion Slerp (float s, Quaternion q1, Quaternion q2) {
            if (s < 0 || s > 1) {
                throw new ArgumentException ("S is invalid");
            }
            var dot = Dot (q1, q2);
            if (1 - dot * dot < 0.000005) {
                return q1;
            }
            var th = Math.Acos (Dot (q1, q2));
            if (th > 0) {
                var w1 = (float)(Math.Sin ((1 - s) * th) / Math.Sin (th));
                var w2 = (float)(Math.Sin (s * th) / Math.Sin (th));
                return q1 * w1 + q2 * w2;
            }
            else {
                // NAN
                return q1;
            }
        }

        /// <summary>
        /// クォータニオンの正規化
        /// </summary>
        /// <remarks>
        /// クォータニオンを正規化して長さが１のクォータニオンを作成します。
        /// 元のクォータニオンは変更しません。
        /// 長さがゼロのクォータニオンを正規化するとNAN化します。
        /// </remarks>
        /// <returns>長さが1のクォータニオン</returns>
        public Quaternion Normalize () {
            var q = new Quaternion ();
            q.X = X / Length;
            q.Y = Y / Length;
            q.Z = Z / Length;
            q.W = W / Length;
            return q;
        }


        /// <summary>
        /// ベクトルの回転
        /// </summary>
        /// <remarks>
        /// このクォータニオンを使ってベクトル <c>v</c> を回転します。
        /// 計算式は<c>v' = qvq'</c> です (<c>q'</c>は共役のクォータニオンを示す記号）。
        /// </remarks>
        /// <param name="v">回転前のベクトル</param>
        /// <returns>回転後のベクトル</returns>
        public Vector3 Apply (Vector3 v) {
            var a = new Vector3 (X, Y, Z);
            return (W * W - a.Length2) * v + 2 * W * Vector3.Cross (a, v) + 2 * Vector3.Dot (a, v) * a;
        }

        /// <summary>
        /// クォータニオンの積
        /// </summary>
        /// <remarks>
        /// クォータニオンの積を計算します。<c>q' = q1q2</c>.
        /// 定義から<c>q' = (q1q2)vc(q1q2)' = q1(q2vq2')q1'</c>.
        /// 従って新しいクォータニオンはまずクォータニオンq2で回転し
        /// 、次にクォータニオンq1で回転する回転と等価です。
        /// 戻り値のクォータニオンは積を取る前のクォータニオンが両方とも正規化されていれば正規化されています。
        /// そうでなければ正規化されていません。
        /// </remarks>
        /// <param name="q1">クォータニオン1</param>
        /// <param name="q2">クォータニオン2</param>
        /// <returns>新しいクォータニオン</returns>
        public static Quaternion operator * (Quaternion q1, Quaternion q2) {
            var x = q1.W * q2.X + q1.X * q2.W + q1.Y * q2.Z - q1.Z * q2.Y;
            var y = q1.W * q2.Y - q1.X * q2.Z + q1.Y * q2.W + q1.Z * q2.X;
            var z = q1.W * q2.Z + q1.X * q2.Y - q1.Y * q2.X + q1.Z * q2.W;
            var w = q1.W * q2.W - q1.X * q2.X - q1.Y * q2.Y - q1.Z * q2.Z;
            return Quaternion.Set (x, y, z, w, false);
        }

        /// <summary>
        /// 全要素を float の配列に変換します
        /// </summary>
        /// <param name="q">変換元のクォータニオン</param>
        /// <returns>変換後のfloatの配列</returns>
        public static explicit operator float[] (Quaternion q) {
            return new float[] { q[0], q[1], q[2], q[3] };
        }

        /// <summary>
        /// 同値性を比較します
        /// </summary>
        /// <remarks>
        /// 同値性を比較する汎用の比較関数です。
        /// 要素が厳密に一致しなくても全ての要素で差が<see cref="GlobalSettings.Torrelance"/>以下であれば等しいと見なされます。
        /// (注意)クォータニオンはその性質上、値はまったく異なるが「等価」と見なせるものが存在する。
        /// 例えば45,(1,0,0)と-45(-1,0,0)は値は異なるが等価な回転と見なせる。
        /// このメソッドでは要素毎の値を調べるだけで上記の「等価」な回転は考慮していない。
        /// これに関してどう取り扱うかは現在悩み中。
        /// </remarks>
        /// <param name="obj">右辺のオブジェクト</param>
        /// <returns>等しいときtrue, そうでないときfalse</returns>
        public override bool Equals (object obj) {
            if (obj == null || this.GetType () != obj.GetType ()) {
                return false;
            }
            return this.Equals ((Quaternion)obj);
        }

        /// <summary>
        /// タイプセーフな同値性を比較
        /// </summary>
        /// <remarks>
        /// ２つのクォータニオンの同値性を判定します。
        /// <see cref="GlobalSettings.Torrelance"/> 以内の誤差を許容します。
        /// </remarks>
        /// <param name="q">クォータニオン</param>
        /// <returns>等しいときtrue, そうでないときfalse</returns>
        public bool Equals (Quaternion q) {
            return (Math.Abs (X - q.X) < GlobalSettings.Torrelance) &&
                   (Math.Abs (Y - q.Y) < GlobalSettings.Torrelance) &&
                   (Math.Abs (Z - q.Z) < GlobalSettings.Torrelance) &&
                   (Math.Abs (W - q.W) < GlobalSettings.Torrelance);
        }

        /// <summary>
        /// ２つのクォータニオンの等号による同値性の厳密な比較
        /// </summary>
        /// <remarks>
        /// 2つのクォータニオンを（誤差を許さず）厳密に比較します。
        /// </remarks>
        /// <param name="left">左項</param>
        /// <param name="right">右項</param>
        /// <returns>等しいときtrue, そうでないときfalse</returns>
        public static bool operator == (Quaternion left, Quaternion right) {
            return (left.X == right.Y) && (left.Y == right.Y) && (left.Z == right.Z) && (left.W == right.W);
        }

        /// <summary>
        /// 等号による同値性の厳密な比較
        /// </summary>
        /// <param name="left">左項</param>
        /// <param name="right">右項</param>
        /// <returns>等しくないときtrue, そうでないときfalse</returns>
        public static bool operator != (Quaternion left, Quaternion right) {
            return !(left == right);
        }

        /// <summary>
        /// 一意のハッシュ値を取得します
        /// </summary>
        /// <remarks>
        /// ハッシュ値は厳密な比較に基づき計算されます。
        /// したがって <c>A == B</c> のとき必ず <c>A.GetHasCode() == B.GetHashCode()</c> です。
        /// <see cref="GlobalSettings.Torrelance"/> の影響を受けません。
        /// </remarks>
        /// <returns></returns>
        public override int GetHashCode () {
            return X.GetHashCode () ^ Y.GetHashCode () ^ Z.GetHashCode () ^ W.GetHashCode ();
        }

        /// <inheritdoc/>
        public override string ToString () {
            return string.Format ("({0},{1},{2},{3})", X, Y, Z, W);
        }
        #endregion
    }
}
