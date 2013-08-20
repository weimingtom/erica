using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using TiledSharp;
using System.IO;

namespace DD {
    /// <summary>
    /// タイル マップ生成 コンポーネント
    /// </summary>
    /// <remarks>
    /// Tiled Map Editor の TMX ファイルをパースして、
    /// DDの2Dマップとして構築するコンポーネントです。
    /// 構築が終わると子ノードとして複数のレイヤー ノード（タイルマップ、オブジェクト、背景画像）と、
    /// タイル マップのさらにその下に Width * Height 個のタイル ノードを構築します。
    /// <note>
    /// クォーター ビュー ("isometric") のゲームの場合、通常では1タイルの大きさは 横:縦 = 2:1 (角度26.5°) が使用される（整数倍が一番きれいに表示できる。その上の1：3は扁平すぎる）。
    /// 特に1タイル = 64ピクセル x 32ピクセル がよく使用される。
    /// この時タイル画像のタイル1枚の大きさはそれと同じである必要はなく、デザイナーさんは 64ピクセル x 64ピクセル で作る事が多い（ように思う）。
    /// 実装する時にタイルとタイル画像の違いに注意せよ > 俺
    /// またコリジョン形状は"Orgthogonal","Isometric"の両方で箱形コリジョンを使用する。
    /// "Isometric"の時もゲームロジックはすべて2Dで計算し、最終的な描画のみを2.5Dで表現する事に注意。
    /// </note>
    /// </remarks>
    public class TiledMapComposer : Component {

        #region Field
        string orientaion;
        int width;
        int height;
        int tileWidth;
        int tileHeight;
        List<Node> mapLayers;
        List<Node> objLayers;
        List<Node> imgLayers;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public TiledMapComposer () {
            this.orientaion = "";
            this.width = 0;
            this.height = 0;
            this.tileWidth = 0;
            this.tileHeight = 0;
            this.mapLayers = new List<Node> ();
            this.objLayers = new List<Node> ();
            this.imgLayers = new List<Node> ();
        }
        #endregion

        #region Property
        /// <summary>
        /// タイルマップの種類
        /// </summary>
        /// <remarks>
        /// 現在の所 "Orthorognal" （普通の直交マップ）にしか対応していません。
        /// "Isometoric", "Taggered?" への対応は未定です。
        /// </remarks>
        public string Orientation {
            get { return orientaion; }
        }

        /// <summary>
        /// マップのX方向のタイル数
        /// </summary>
        /// <remarks>
        /// マップのX方向のタイル数です。
        /// </remarks>
        public int Width {
            get { return width; }
        }

        /// <summary>
        /// マップのY方向のタイル数
        /// </summary>
        /// <remarks>
        /// マップのY方向のタイル数です。
        /// </remarks>
        public int Height {
            get { return height; }
        }

        /// <summary>
        /// タイルの幅（ピクセル数）
        /// </summary>
        /// <remarks>
        /// タイル1枚の横幅（ピクセル数）です。
        /// </remarks>
        public int TileWidth {
            get { return tileWidth; }
        }

        /// <summary>
        /// タイルの高さ（ピクセル数）
        /// </summary>
        /// <remarks>
        /// タイル1枚の高さ（ピクセル数）です。
        /// </remarks>
        public int TileHeight {
            get { return tileHeight; }
        }

        /// <summary>
        /// レイヤーの総数
        /// </summary>
        /// <remarks>
        /// 3種類のレイヤーをすべて合計したレイヤー数です。
        /// </remarks>
        public int LayerCount {
            get { return MapLayerCount + ObjectLayerCount + ImageLayerCount; }
        }

        /// <summary>
        /// マップ レイヤーの個数
        /// </summary>
        public int MapLayerCount {
            get { return mapLayers.Count (); }
        }

        /// <summary>
        /// オブジェクト レイヤーの個数
        /// </summary>
        public int ObjectLayerCount {
            get { return objLayers.Count (); }
        }

        /// <summary>
        /// 背景画像 レイヤーの個数
        /// </summary>
        public int ImageLayerCount {
            get { return imgLayers.Count (); }
        }

        /// <summary>
        /// すべてのレイヤー ノードを列挙する列挙子
        /// </summary>
        public IEnumerable<Node> Layers {
            get { return MapLayers.Concat (ObjectLayers).Concat (ImageLayers); }
        }

        /// <summary>
        /// マップ レイヤー ノードを列挙する列挙子
        /// </summary>
        public IEnumerable<Node> MapLayers {
            get { return mapLayers; }
        }

        /// <summary>
        /// オブジェクト レイヤー ノードを列挙する列挙子
        /// </summary>
        public IEnumerable<Node> ObjectLayers {
            get { return objLayers; }
        }

        /// <summary>
        /// 背景画像レイヤーを列挙する列挙子
        /// </summary>
        public IEnumerable<Node> ImageLayers {
            get { return imgLayers; }
        }

        #endregion

        #region Method

        /// <summary>
        /// 直交座標系の等角投影座標系への変換
        /// </summary>
        /// <remarks>
        /// 2Dの直交座標系（orthographic）を2.5Dの等角投影系（isometric）に変換します。
        /// いわゆるクォータービューのゲームはゲームロジックは通常通り2Dで作成し、
        /// このメソッドで2.5Dの表示位置を計算して、スプライトのオフセットにセットすることで
        /// 2.5Dで表示可能です。
        /// </remarks>
        /// <param name="or">直交座標系（ピクセル）</param>
        /// <returns></returns>
        public Vector3 OrthogonalToIsometric (Vector3 or) {
            var x = -or.X / 2 - or.Y * tileWidth / (2 * tileHeight);
            var y = or.X * tileHeight / (2 * tileWidth) - or.Y / 2;
            return new Vector3 (x, y, 0);
        }

        /// <summary>
        /// TMXファイルのロードと構築
        /// </summary>
        /// <remarks>
        /// Tiled Map Editor の TMX 形式のマップ ファイルを読み込んで、
        /// このノードの下に再構築します。
        /// <note>
        /// "Orthogonal"でも"Isometric"でもノード位置はまったく同じ。
        /// "Isometric"の場合マップが菱形になるようにスプライトのオフセットが設定される。
        /// コリジョンなどは"Orthogonal"と同一の物を使うべし。
        /// </note>
        /// </remarks>
        /// <param name="fileName">マップ ファイル名</param>
        /// <returns></returns>
        public bool LoadFromFile (string fileName) {
            if (Node == null) {
                throw new InvalidOperationException ("This component is not attached");
            }
            var map = new TmxMap (fileName);
            if (map == null) {
                return false;
            }
            if (!(map.Orientation == TmxMap.OrientationType.Orthogonal || map.Orientation == TmxMap.OrientationType.Isometric)) {
                throw new NotImplementedException ("Sorry, Orthogonal or Isometric only!");
            }

            // マップ情報
            //this.Node.Name = fileName;
            this.orientaion = map.Orientation.ToString ();
            this.width = map.Width;
            this.height = map.Height;
            this.tileWidth = map.TileWidth;
            this.tileHeight = map.TileHeight;
            this.mapLayers.Clear ();
            this.objLayers.Clear ();
            this.imgLayers.Clear ();
            foreach (var prop in map.Properties) {
                this.Node.UserData.Add (prop.Key, prop.Value);
            }

            var texIds = new Dictionary<string, string> ();

            // 使用するタイル セットのロード
            foreach (var tileset in map.Tilesets) {
                var data = tileset.Image.Data;
                if (data != null) {
                    // 現在のところ画像埋め込み型（data != null）には対応していません。
                    // （そもそもエディターが対応してないので作れない）
                    // data は GZipStream または ZlibStream 。両方とも stream から派生してるのがめんどくさそう。
                    throw new NotImplementedException ("Sorry, Embedded image is not implemented");
                }
                var src = tileset.Image.Source;
                if (src != null) {
                    Resource.GetTexture (src);
                    texIds.Add (tileset.Name, src);
                }
            }

            // タイル レイヤー
            foreach (var layer in map.Layers) {
                var layerNode = new Node (layer.Name);
                layerNode.Drawable = layer.Visible;
                layerNode.Opacity = (float)layer.Opacity;
                foreach (var prop in layer.Properties) {
                    layerNode.UserData.Add (prop.Key, prop.Value);
                }
                var layerComp = new GridMap (width, height);
                layerComp.TileWidth = tileWidth;
                layerComp.TileHeight = tileHeight;
                layerNode.Attach (layerComp);

                foreach (var tile in layer.Tiles) {
                    var x = tile.X;       // マップ座標系（タイル単位）
                    var y = tile.Y;
                    var gid = tile.Gid;
                    if (gid != 0) {
                        var tileset = map.Tilesets.Last (t => t.FirstGid <= gid);
                        var id = gid - tileset.FirstGid;
                        var tex = Resource.GetTexture (texIds[tileset.Name]);
                        var hTileNum = (tex.Width - tileset.Margin * 2 + tileset.Spacing) / (tileset.TileWidth + tileset.Spacing);
                        var ix = id % hTileNum;
                        var iy = id / hTileNum;
                        var tx = tileset.TileOffset.X + tileset.Margin + (tileset.TileWidth + tileset.Spacing) * ix;
                        var ty = tileset.TileOffset.Y + tileset.Margin + (tileset.TileHeight + tileset.Spacing) * iy;

                        var tileNode = new Node (layer.Name + "[" + x + "," + y + "]");
                        tileNode.Translation = new Vector3 (x * TileWidth, y * TileHeight, 0);

                        var spr = new Sprite (tex, tileset.TileWidth, tileset.TileHeight);
                        spr.SetTextureOffset (tx, ty);
                        if (orientaion == "Isometric") {
                            var sx = -x * TileWidth / 2 - y * TileWidth / 2; // -TileWidth / 2;
                            var sy = x * TileHeight / 2 - y * TileHeight / 2;
                            spr.SetOffset (sx, sy);
                        }
                        tileNode.Attach (spr);


                        layerNode.AddChild (tileNode);
                        layerComp[y, x] = tileNode;
                    }
                }

                this.Node.AddChild (layerNode);
                this.mapLayers.Add (layerNode);
            }

            // オブジェクト レイヤー
            foreach (var layer in map.ObjectGroups) {
                var layerNode = new Node (layer.Name);
                layerNode.Drawable = layer.Visible;
                layerNode.Opacity = (float)layer.Opacity;
                foreach (var prop in layer.Properties) {
                    layerNode.UserData.Add (prop.Key, prop.Value);
                }

                foreach (var obj in layer.Objects) {
                    var objNode = new Node (obj.Name);
                    objNode.SetTranslation (obj.X, obj.Y, 0);

                    var type = Type.GetType (obj.Type);
                    if (type != null) {
                        var comp = Activator.CreateInstance (type) as Component;
                        if (comp != null) {
                            foreach (var prop in obj.Properties) {
                                var propInfo = comp.GetType ().GetProperty (prop.Key);
                                if (propInfo != null) {
                                    SetPropertyValue (comp, propInfo, (string)prop.Value);
                                }
                            }
                            objNode.Attach (comp);
                        }
                    }
                    var p = obj.Points;
                    var pt = obj.Tile;

                    layerNode.AddChild (objNode);
                }

                this.Node.AddChild (layerNode);
                this.objLayers.Add (layerNode);
            }

            // 画像レイヤー
            foreach (var layer in map.ImageLayers) {
                var layerNode = new Node (layer.Name);
                layerNode.Drawable = layer.Visible;
                layerNode.Opacity = (float)layer.Opacity;
                foreach (var prop in layer.Properties) {
                    layerNode.UserData.Add (prop.Key, prop.Value);
                }

                var src = layer.Image.Source;
                if (src != null) {
                    var layerComp = new Sprite (Resource.GetTexture (src));
                    layerNode.Attach (layerComp);
                }
                var data = layer.Image.Data;
                if (data != null) {
                    throw new NotImplementedException ("Sorry, Embedded image is not implemented");
                }

                this.Node.AddChild (layerNode);
            }

            return true;
        }

        /// <summary>
        /// プロパティに値を代入
        /// </summary>
        /// <remarks>
        /// 文字列で表された値を確実に値に戻す方法はない。
        /// 実装は bool, int, float と文字列のみのパースだが、これで十分実用的。
        /// </remarks>
        /// <param name="obj">オブジェクト</param>
        /// <param name="propInfo">プロパティ情報</param>
        /// <param name="value">値の文字列</param>
        private void SetPropertyValue (object obj, PropertyInfo propInfo, string value) {

            bool b;
            int i;
            float f;

            if (propInfo.PropertyType == typeof (string)) {
                propInfo.SetValue (obj, value, null);
            }
            else if (bool.TryParse (value, out b)) {
                propInfo.SetValue (obj, b, null);
            }
            else if (int.TryParse (value, out i)) {
                propInfo.SetValue (obj, i, null);
            }
            else if (float.TryParse (value, out f)) {
                propInfo.SetValue (obj, f, null);
            }

        }

        /// <summary>
        /// レイヤー（ノード）の取得
        /// </summary>
        /// <remarks>
        /// レイヤー（3種類共通）を取得します。
        /// </remarks>
        /// <param name="index">レイヤー番号</param>
        /// <returns>ノード</returns>
        public Node GetLayer (int index) {
            if (index < 0 || index > LayerCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            return mapLayers[index];
        }

        #endregion


    }
}
