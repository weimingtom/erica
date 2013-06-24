using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Window;

namespace DD {

    /// <summary>
    /// 入力レシーバー クラス
    /// </summary>
    /// <remarks>
    /// キーボードおよびマウスの入力を処理します。
    /// 自動でデフォルトのレシーバーが <see cref="World"/> オブジェクトに作成されるので、
    /// 自分でインスタンス化する事はまずありません（が、してもいいです）。
    /// キーはキー コード <see cref="KeyCode"/> で識別されますが、独自に別名 <see cref="string"/> を付ける事が可能です。
    /// 通常はゲームではこのエイリアス名を使用します。
    /// </remarks>
    public class InputReceiver : Component {

        #region Field
        KeyCode[] keys1;
        KeyCode[] keys2;
        Vector2 mouse1;
        Vector2 mouse2;
        List<KeyAlias> aliases;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public InputReceiver () {
            this.keys1 = new KeyCode[0];
            this.keys2 = new KeyCode[0];
            this.mouse1 = new Vector2 ();
            this.mouse2 = new Vector2 ();
            this.aliases = new List<KeyAlias> ();
        }
        #endregion

        #region Property
        /// <summary>
        /// キー エイリアスの個数
        /// </summary>
        public int AliasCount {
            get { return aliases.Count (); }
        }

        /// <summary>
        /// すべてのキー エイリアスを列挙する列挙子
        /// </summary>
        public IEnumerable<KeyAlias> Aliases {
            get { return aliases; }
        }

        /// <summary>
        /// マウス位置（ピクセル座標）
        /// </summary>
        public Vector2 MousePosition {
            get { return mouse1; }
        }

        /// <summary>
        /// マウス移動量（ピクセル座標）
        /// </summary>
        /// <remarks>
        /// マウス位置の前フレームとの差分です。
        /// （前フレーム位置から現在のフレーム位置を指す移動ベクトル）
        /// </remarks>
        public Vector2 MouseDelta {
            get { return mouse1 - mouse2; }
        }

        /// <summary>
        /// バッファリング中のキーの個数
        /// </summary>
        /// <remarks>
        /// 現在の押されているキーの個数です。
        /// </remarks>
        public int KeyCount {
            get { return keys1.Count (); }
        }

        /// <summary>
        /// バッファリング中のキーをすべて列挙する列挙子
        /// </summary>
        /// <remarks>
        /// 現在押されているキーです。
        /// </remarks>
        public IEnumerable<KeyCode> Keys {
            get { return keys1; }
        }

        /// <summary>
        /// キーが押されている事を確認するプロパティ
        /// </summary>
        /// <remarks>
        /// 何でも良いのでキーが押されている状態の時、毎フレーム <c>true</c> が返ります。
        /// Alt キーや Ctrl キーも含みます。
        /// </remarks>
        public bool AnyKey {
            get { return keys1.Length > 0; }
        }

        /// <summary>
        /// キーが押された事を1回だけ確認するプロパティ
        /// </summary>
        /// <remarks>
        /// 何でも良いのでキーが押された時、最初のフレームだけ <c>true</c> が返ります。
        /// Alt キーや Ctrl キーも含みます。
        /// </remarks>
        public bool AnyKeyDown {
            get { return (keys1.Length > 0) && (keys2.Length == 0); }
        }

        /// <summary>
        /// シフト キーが押されている事を確認するプロパティ
        /// </summary>
        /// <remarks>
        /// （左右どちらか、または両方の）シフトキーが押されている時、すべてのフレームで <c>true</c> が返ります。
        /// </remarks>
        public bool Shift {
            get { return Array.Exists(keys1,  (x => x == KeyCode.LeftShift || x == KeyCode.RightShift)); }
        }

        /// <summary>
        /// コントロール キーが押されている事を確認するプロパティ
        /// </summary>
        /// <remarks>
        /// （左右どちらか、または両方の）シフトキーが押されている時、すべてのフレームで <c>true</c> が返ります。
        /// </remarks>
        public bool Control {
            get { return Array.Exists(keys1, (x => x == KeyCode.LeftControl || x == KeyCode.RightControl)); }
        }

        /// <summary>
        /// アルト キーが押されている事を確認するプロパティ
        /// </summary>
        /// <remarks>
        /// （左右どちらか、または両方の）シフトキーが押されている時、すべてのフレームで <c>true</c> が返ります。
        /// </remarks>
        public bool Alt {
            get { return Array.Exists(keys1, (x => x == KeyCode.LeftAlt || x == KeyCode.RightAlt)); }
        }

        /// <summary>
        /// 現在押されているキーにエイリアス名でアクセスするインデクサー
        /// </summary>
        /// <param name="name">エイリアス名</param>
        /// <returns>押されている時 <c>true</c>、そうでないとき <c>false</c>.</returns>
        public bool this[string name] {
            get {
                return GetKey (name);
            }
        }

        /// <summary>
        /// 現在押されているキーにキー コードでアクセスするインデクサー
        /// </summary>
        /// <param name="code">キー コード</param>
        /// <returns>押されている時 <c>true</c>、そうでないとき <c>false</c>.</returns>
        public bool this[KeyCode code] {
            get {
                return GetKey(code);
            }
        }

        #endregion


        #region Method
        /// <summary>
        /// 指定コードのキーの取得
        /// </summary>
        /// <remarks>
        /// 指定のコードのキーが押されている時、毎フレーム <c>true</c> が返ります。
        /// </remarks>
        /// <param name="code">キー コード</param>
        /// <returns></returns>
        public bool GetKey (KeyCode code) {
            return keys1.Contains(code);
        }

        /// <summary>
        /// 指定エイリアス名のキーの取得
        /// </summary>
        /// <remarks>
        /// 指定のエイリアス名のキーが押されている時、毎フレーム <c>true</c> が返ります。
        /// エイリアス名は事前に登録されている必要があります。
        /// </remarks>
        /// <param name="name">エイリアス名</param>
        /// <returns></returns>
        public bool GetKey (string name) {
            var codes = from x in aliases
                        where x.Name == name
                        select x.KeyCode;
            return keys1.Intersect (codes).Count () > 0;
        }

        /// <summary>
        /// 1つ前のフレームの指定コードのキーの取得
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool GetKey2 (KeyCode code) {
            return keys2.Contains (code);
        }

        /// <summary>
        /// 1つ前の指定エイリアス名のキーの取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool GetKey2 (string name) {
            var codes = from x in aliases
                        where x.Name == name
                        select x.KeyCode;
            return keys2.Intersect (codes).Count () > 0;
        }

        /// <summary>
        /// 解放されたキーの取得
        /// </summary>
        /// <remarks>
        /// 指定のコードのキーが解放された時、1フレームだけ <c>true</c> が返ります。
        /// </remarks>
        /// <param name="code">キー コード</param>
        /// <returns></returns>
        public bool GetKeyDown (KeyCode code) {
            return keys1.Contains (code) && !keys2.Contains (code);
        }

        /// <summary>
        /// 解放されたキーの取得
        /// </summary>
        /// <remarks>
        /// 指定のエイリアス名のキーが解放された時、1フレームだけ <c>true</c> が返ります。
        /// エイリアス名は事前に登録されている必要があります。
        /// </remarks>
        /// <param name="name">エイリアス名</param>
        /// <returns></returns>
        public bool GetKeyDown (string name) {
            return GetKey (name) && !GetKey2 (name);
        }

        /// <summary>
        /// 解放されたキーの取得
        /// </summary>
        /// <remarks>
        /// 指定のコードのキーが解放された時、1フレームだけ <c>true</c> が返ります。
        /// </remarks>
        /// <param name="code">キー コード</param>
        /// <returns></returns>
        public bool GetKeyUp (KeyCode code) {
            return !GetKey (code) && GetKey2 (code);
        }

        /// <summary>
        /// 解放されたキーの取得
        /// </summary>
        /// <remarks>
        /// 指定のエイリアス名のキーが解放された時、1フレームだけ <c>true</c> が返ります。
        /// エイリアス名は事前に登録されている必要があります。
        /// </remarks>
        /// <param name="name">エイリアス名</param>
        /// <returns></returns>
        public bool GetKeyUp (string name) {
            return !GetKey (name) && GetKey (name);
        }

        /// <summary>
        /// エイリアスの登録
        /// </summary>
        /// <remarks>
        /// キー コードに別名（エイリアス）を登録します。1つのキーコードに複数の名前を付ける事も、
        /// 複数のキーコードに同一の名前を付ける事も可能です。
        /// </remarks>
        /// <param name="name">名前</param>
        /// <param name="code">キーコード</param>
        public void AddAlias (string name, KeyCode code) {
            if (name == null || name == "") {
                throw new ArgumentNullException ("Name is null or mepty");
            }
            this.aliases.Add (new KeyAlias(name, code));
        }

        /// <summary>
        /// エイリアスの削除
        /// </summary>
        /// <remarks>
        /// 指定の番号のエイリアスを削除します。
        /// </remarks>
        /// <param name="index">インデックス番号</param>
        public void RemoveAlias (int index) {
            if (index < 0 || index > AliasCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            this.aliases.RemoveAt (index);
        }

        /// <summary>
        /// エイリアスの取得
        /// </summary>
        /// <param name="index">インデックス番号</param>
        /// <returns></returns>
        public KeyAlias GetAlias (int index) {
            if (index < 0 || index > AliasCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            return aliases[index];
        }

        /// <summary>
        /// キー入力の追加
        /// </summary>
        /// <remarks>
        /// このインプット レシーバーにキー入力を入れます。
        /// これは物理的にキーボードのキーを押した時と等価です。
        /// ユーザーがこのメソッドを直接使用するのは避けるべきです。
        /// </remarks>
        /// <param name="code">キー コード</param>
        public void AddKeyInput (KeyCode code) {
            var length = keys1.Length;
            var newArray = new KeyCode[length+1];
            Array.Copy (keys1, newArray, length);
            newArray[length] = code;

            this.keys1 = newArray;
        }

        /// <inheritdoc/>
        public override void OnUpdate (long msec) {
            var g2d = Graphics2D.GetInstance ();

            // 押されているキーの更新
            this.keys2 = keys1;
            this.keys1 = g2d.GetKeys ().ToArray();
                
            // マウス位置の更新
            this.mouse2 = mouse1;
            this.mouse1 = g2d.GetMousePosition ();
        }
        #endregion

    }
}
