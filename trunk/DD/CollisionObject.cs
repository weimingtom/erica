using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;

namespace DD {
    /// <summary>
    /// コリジョン コンポーネント
    /// </summary>
    /// <remarks>
    /// ノードがコリジョン体としてふるまうようになるコンポーネントです。
    /// 物理演算とは関係ありません。
    /// </remarks>
    public class CollisionObject : Component, System.IDisposable {

        #region Filed
        CollisionShape shape;
        int collideWith;
        int ignoreWith;
        Vector3 offset;
        PairCachingGhostObject ghostObject;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public CollisionObject () {
            this.shape = null;
            this.ghostObject = null;
            this.offset = new Vector3 (0, 0, 0);
            this.collideWith = -1;
            this.ignoreWith = 0;
        }
        #endregion

        #region Property

        /// <summary>
        /// コリジョン対象のビットマスク
        /// </summary>
        public int CollideWith {
            get { return collideWith; }
            set {
                this.collideWith = value;
            }
        }

        /// <summary>
        /// コリジョン無視対象のビットマスク
        /// </summary>
        /// <remarks>
        /// このビットマスクは <see cref="CollideWith"/> より優先します。
        /// 従ってこのビットマスクに含まれるグループは <see cref="CollideWith"/> の値にかかわらずコリジョンの対象になりません。
        /// </remarks>
        public int IgnoreWith {
            get { return ignoreWith; }
            set { this.ignoreWith = value; }
        }

        /// <summary>
        /// 最終的なコリジョンマスク
        /// </summary>
        /// <remarks>
        /// <see cref="CollideWith"/> から <see cref="IgnoreWith"/> ビットを取り除いた物です。
        /// 最終的にこのビットマスクと <see cref="Node.GroupID"/> の積が0以上の場合コリジョンが発生します。
        /// </remarks>
        /// <seealso cref="CollideWith"/>
        /// <seealso cref="IgnoreWith"/>
        public int CollisionMask {
            get { return collideWith & ~IgnoreWith; }
        }

        /// <summary>
        /// コリジョン形状
        /// </summary>
        /// <remarks>
        /// 言うまでもなくコリジョンオブジェクトは形状を指定して初めて機能するようになります。
        /// 以前登録されていた形状と異なる形状を新たに登録可能です。
        /// </remarks>
        public CollisionShape Shape {
            get { return shape; }
            set {
                if (value == null) {
                    throw new ArgumentNullException ("Shape is null");
                }

                if (ghostObject != null) {
                    if(CollisionAnalyzer.IsRegistered(Node)){
                        CollisionAnalyzer.RemoveGhostObject (Node);
                        ghostObject.Dispose ();
                    }                    
                }

                this.shape = value;
                this.ghostObject = value.CreateGhostObject ();
                this.ghostObject.UserObject = this;
            }
        }

        /// <summary>
        /// 内部データ
        /// </summary>
        /// <remarks>
        /// ユーザーはこのプロパティを使用しません。
        /// </remarks>
        public BulletSharp.CollisionObject Data {
            get { return ghostObject; }
        }

        /// <summary>
        /// オフセット
        /// </summary>
        public Vector3 Offset {
            get { return offset; }
            set { this.offset = value; }
        }

        /// <summary>
        /// 現在このコリジョン形状と重なっているコリジョンの数
        /// </summary>
        /// <remarks>
        /// コリジョン マスクによってコリジョンが発生しないノードはカウントされません。
        /// </remarks>
        public int OverlappingObjectCount {
            get {
                if (ghostObject == null) {
                    return 0;
                }
                return ghostObject.NumOverlappingObjects;
            }
        }

        /// <summary>
        /// 現在このコリジョン形状と重なっているコリジョン ノードをすべて列挙する列挙子
        /// </summary>
        public IEnumerable<Node> OverlapObjects {
            get {
                if (ghostObject == null) {
                    return new Node[0];
                }
                return ghostObject.OverlappingPairs.Select (x => (x.UserObject as CollisionObject).Node);
            }

        }
        #endregion


        #region Method

        /// <summary>
        /// 現在重なっているノードの取得
        /// </summary>
        /// <remarks>
        /// 現在重なり合うノードの総数は <see cref="OverlappingObjectCount"/> で取得可能です。
        /// </remarks>
        /// <param name="index">インデックス</param>
        /// <returns></returns>
        public Node GetOverlappingObject (int index) {
            if (ghostObject == null) {
                throw new InvalidOperationException ("Ghost object is null");
            }
            if (index < 0 || index > OverlappingObjectCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of ragne");
            }
            var obj = ghostObject.OverlappingPairs.ElementAt (index);
            return (obj.UserObject as CollisionObject).Node;
        }

        /// <summary>
        /// オフセットの変更
        /// </summary>
        /// <param name="x">オフセット量のX（ワールド座標）</param>
        /// <param name="y">オフセット量のY（ワールド座標）</param>
        /// <param name="z">オフセット量のZ（ワールド座標）</param>
        public void SetOffset (float x, float y, float z) {
            this.offset = new Vector3 (x, y, z);
        }

        /// <summary>
        /// コリジョン形状1つに対するレイキャスト
        /// </summary>
        /// <param name="colA">キャスト対象のコリジョン オブジェクト</param>
        /// <param name="start">開始地点（ワールド座標）</param>
        /// <param name="end">終了地点（ワールド座標）</param>
        /// <returns>ヒットした時はその地点までの距離(0以上)の値、ヒットしなかった時は <see cref="Single.NaN"/>.</returns>
        public static float RayCast (CollisionObject colA, Vector3 start, Vector3 end) {
            var from = new BulletSharp.Vector3 (start.X, start.Y, start.Z);
            var to = new BulletSharp.Vector3 (end.X, end.Y, end.Z);
            var result = new CollisionWorld.ClosestRayResultCallback (from, to);

            colA.ghostObject.RayTest (from, to, result);

            if (!result.HasHit) {
                return Single.NaN;
            }
            return result.ClosestHitFraction * (end - start).Length;
        }

        /// <summary>
        /// DD側とコリジョン解析ワールドを同期します
        /// </summary>
        /// <remarks>
        /// DD側の座標をコリジョンワールドに反映します。
        /// </remarks>
        public void SyncWithCollisionWorld () {
            
            // 座標
            var T = Node.Position + offset;
            var R = Node.Orientation;
            var S = new Vector3 (1, 1, 1);

            
            ghostObject.WorldTransform = Matrix4x4.Compress (T, R, S).ToBullet ();
        }

        /// <inheritdoc/>
        public override void OnCollisionUpdateInit (long msec) {
            if (!CollisionAnalyzer.IsRegistered (Node)) {
                SyncWithCollisionWorld ();
                CollisionAnalyzer.AddGhostObject (Node);
            }
        }

        /// <inheritdoc/>
        public override void OnCollisionUpdate (long msec) {
            // DD側の座標をコリジョン解析ワールドの座標に反映
            // これ(sx,sy,sz)を(1,1,1)に直さなくて大丈夫か？
            //   --> SyncWithCollisionWOrld()に移管
            //ghostObject.WorldTransform = Node.GlobalTransform.ToBullet ();
            SyncWithCollisionWorld ();
        }



        /// <inheritdoc/>
        public override void  OnDetached(){
            // コリジョン ワールドから削除
            // ここで削除しなくても CollisionAnyzer が終了する時にすべて自動で削除される。
            // これは個別のゲームオブジェクトが単独で死ぬ時の処理。
            // 2つ目の CollisionAnalyzer の null チェックは消したらダメ。
            // コリジョンオブジェクトが World にアタッチされた時に
            // すでに CollisionAnalyzer が終了していることがある.
            if (ghostObject != null && CollisionAnalyzer != null) {
                if (CollisionAnalyzer.IsRegistered (Node)) {
                    CollisionAnalyzer.RemoveGhostObject (Node);
                }
            }
        }

        /// <inheritdoc/>
        void System.IDisposable.Dispose () {
            if (ghostObject != null) {
                // （注意）
                // このメソッドが呼ばれる時にはすでにデタッチされているので
                // ここでコリジョン ワールドから削除する事ができない
                // 削除せずに Dispose() を呼ぶとワールドのDispose()を呼んだタイミングで
                // BulletSharp が不正なメモリアクセスで落ちる。
                // 従って忘れずに OnDestroyed() でワールドから取り除いておくこと。
                ghostObject.Dispose ();
                this.ghostObject = null;
            }
        }

        #endregion


    }
}
