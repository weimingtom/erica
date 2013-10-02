using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;
using System.Diagnostics;

namespace DD {
    /// <summary>
    /// コリジョン解析機 クラス
    /// </summary>
    /// <remarks>
    /// コリジョンを解決するためのクラスです。通常はワールドクラスが自動的にインスタンス化し、
    /// ユーザーが直接これを使用することはありません。
    /// </remarks>
    public class CollisionAnalyzer : Component, System.IDisposable {

        #region Field
        CollisionWorld wld;
        CollisionDispatcher dispatcher;
        DbvtBroadphase broadphase;
        //ConstraintSolver solver;    // コリジョン計算だけの時は不要
        List<OverlappingPair> prevContacts;
        List<OverlappingPair> currContacts;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public CollisionAnalyzer () {
            var cc = new DefaultCollisionConfiguration ();
            dispatcher = new CollisionDispatcher (cc);
            broadphase = new DbvtBroadphase ();
            broadphase.OverlappingPairCache.SetInternalGhostPairCallback (new GhostPairCallback ());
            // solver = new SequentialImpulseConstraintSolver ();

            this.wld = new DiscreteDynamicsWorld (dispatcher, broadphase, null, cc);

            this.prevContacts = new List<OverlappingPair> ();
            this.currContacts = new List<OverlappingPair> ();
        }
        #endregion
        /// <summary>
        /// BulletSharpで使用される CollisionWorld オブジェクト
        /// </summary>
        /// <remarks>
        /// なんで　Data という名前でないのだろう
        /// </remarks>
        public BulletSharp.CollisionWorld CollisionWorld {
            get { return wld; }
        }

        /// <summary>
        /// 登録済みのコリジョン オブジェクトの個数
        /// </summary>
        public int CollisionObjectCount {
            get { return wld.NumCollisionObjects; }
        }

        /// <summary>
        /// 登録済みのコリジョン オブジェクトをすべて列挙する列挙子
        /// </summary>
        public IEnumerable<CollisionObject> CollisionObjects {
            get {
                var objs = new List<CollisionObject> ();
                foreach (var obj in wld.CollisionObjectArray) {
                    var a = obj as BulletSharp.CollisionObject;
                    var b = a.UserObject as CollisionObject;
                    objs.Add (b);
                }
                return objs;
            }
        }

        /// <summary>
        /// 現在重なりのあるコリジョン オブジェクト ペアの個数
        /// </summary>
        /// <remarks>
        /// コリジョン解析を行った後有効になります。
        /// </remarks>
        public int OverlapppingPairCount {
            get { return currContacts.Count (); }
        }

        /// <summary>
        /// 現在重なりのあるコリジョン オブジェクトをすべて列挙する列挙子
        /// </summary>
        /// <remarks>
        /// コリジョン解析を行った後有効になります。
        /// </remarks>
        public IEnumerable<OverlappingPair> OverlappingPairs {
            get { return currContacts; }
        }


        /// <summary>
        /// コリジョンの解析
        /// </summary>
        /// <remarks>
        /// コリジョンを解析し、コリジョン状態の変化に応じてコールバック関数の
        /// <see cref="Component.OnCollisionEnter"/> と <see cref="Component.OnCollisionExit"/> を呼び出します。 
        /// 通常 <see cref="World.CollisionUpdate"/> から毎フレーム自動的に呼び出され、ユーザーがこれを直接呼ぶ必要はありません。
        /// </remarks>
        public void Analyze () {

            // BulletPhysicsによるコリジョンの解決
            wld.PerformDiscreteCollisionDetection ();

            // 重なり情報の更新
            this.prevContacts = currContacts;
            this.currContacts = new List<OverlappingPair> ();

            var num = wld.Dispatcher.NumManifolds;
            for (var i = 0; i < num; i++) {
                var mani = wld.Dispatcher.GetManifoldByIndexInternal (i);
                var nodeA = ((mani.Body0 as GhostObject).UserObject as CollisionObject).Node;
                var nodeB = ((mani.Body1 as GhostObject).UserObject as CollisionObject).Node;
                this.currContacts.Add (new OverlappingPair (nodeA, nodeB));
            }

            InvokeCallbacks ();
        }

        ///  <summary>
        /// コリジョンコールバック関数の呼び出し
        /// </summary>
        /// <remarks>
        /// <see cref="Component.OnCollisionEnter"/> と <see cref="Component.OnCollisionExit"/> の呼び出し
        /// </remarks>
        void InvokeCallbacks () {
            var separated = prevContacts.Except (currContacts);
            var overlapped = currContacts.Except (prevContacts);

            foreach (var pair in separated) {
                foreach (var cmp in pair.NodeA.Components) {
                    cmp.OnCollisionExit (pair.NodeB);
                }
                foreach (var cmp in pair.NodeB.Components) {
                    cmp.OnCollisionExit (pair.NodeA);
                }
            }

            foreach (var pair in overlapped) {
                foreach (var cmp in pair.NodeA.Components) {
                    cmp.OnCollisionEnter (pair.NodeB);
                }
                foreach (var cmp in pair.NodeB.Components) {
                    cmp.OnCollisionEnter (pair.NodeA);
                }
            }
        }

        /// <summary>
        /// ノードが登録済みかどうかの問い合わせ
        /// </summary>
        /// <param name="node">対象ノード</param>
        /// <returns></returns>
        public bool IsRegistered (Node node) {
            if (node == null) {
                return false;
            }
            var ghost = node.GetComponent<CollisionObject> ();
            if (ghost == null) {
                return false;
            }
            return wld.CollisionObjectArray.Contains (ghost.Data);
        }

        /// <summary>
        /// ノードの登録
        /// </summary>
        /// <param name="node">対象ノード</param>
        public void AddGhostObject (Node node) {
            if (node == null) {
                throw new ArgumentNullException();
            }
            if (node.CollisionObject == null) {
                throw new ArgumentException ("Node has no CollisionObject");
            }
            wld.AddCollisionObject (node.CollisionObject.Data,
                                    (CollisionFilterGroups)node.GroupID,
                                    (CollisionFilterGroups)node.CollisionObject.CollisionMask);
        }

        /// <summary>
        /// ノードの削除
        /// </summary>
        /// <param name="node">対象ノード</param>
        public void RemoveGhostObject (Node node) {
            if (node == null) {
                return;
            }
            var ghost = node.GetComponent<CollisionObject> ();
            if (ghost == null) {
                return;
            }
            wld.RemoveCollisionObject (ghost.Data);
        }
        
        /// <summary>
        /// 重なりあうコリジョン オブジェクト ペアを列挙
        /// </summary>
        /// <param name="generation">世代（0=現在、1=1フレーム前、2以降=保存してない）</param>
        /// <returns></returns>
        public IEnumerable<OverlappingPair> GetOverlappingPairs (int generation = 0) {
            if (generation < 0 || generation > 1) {
                throw new ArgumentException ("Generation is invalid");
            }
            switch (generation) {
                case 0: return currContacts;
                case 1: return prevContacts;
                default: throw new NotImplementedException ("Sorry");
            }
        }

        /// <summary>
        /// 2つのコリジョン オブジェクトの距離
        /// </summary>
        /// <param name="colA">コリジョンA</param>
        /// <param name="colB">コリジョンB</param>
        /// <returns></returns>
        public static float Distance (CollisionObject colA, CollisionObject colB) {
            if (colA == null || colA.Shape == null || colB == null || colB.Shape == null ||
                colA.Data == null || colB.Data == null) {
                // 距離が定義できない場合は
                // NaNを返す
                return Single.NaN;
            }

            var shpA = colA.Data.CollisionShape as ConvexShape;
            var shpB = colB.Data.CollisionShape as ConvexShape;
            var solver = new VoronoiSimplexSolver ();

            var detector = new GjkPairDetector (shpA, shpB, solver, null);

            var traA = Matrix4x4.CreateFromTranslation (colA.Offset) * colA.Node.GlobalTransform;
            var traB = Matrix4x4.CreateFromTranslation (colB.Offset) * colB.Node.GlobalTransform;

            var input = new GjkPairDetector.ClosestPointInput ();
            input.TransformA = traA.ToBullet ();
            input.TransformB = traB.ToBullet ();
            input.MaximumDistanceSquared = Single.MaxValue;
            
            var output = new PointCollector ();

            detector.GetClosestPoints (input, output, null);

            if (!output.HasResult) {
                // 重なっていた場合は
                // 0を返す
                return 0;
            }

            // 正常な距離は0以上
            return output.Distance;
        }

        /// <summary>
        /// 2つのコリジョン オブジェクトの重なり判定
        /// </summary>
        /// <param name="colA">コリジョンA</param>
        /// <param name="colB">コリジョンB</param>
        /// <returns></returns>
        public static bool Overlapped (CollisionObject colA, CollisionObject colB) {
            if (colA == null || colA.Shape == null || colB == null || colB.Shape == null) {
                // 距離が定義できない場合は
                // falseを返す
                return false;
            }

            return Distance (colA, colB) == 0;
        }


        /// <summary>
        /// レイキャスト
        /// </summary>
        /// <param name="start">開始地点（ワールド座標）</param>
        /// <param name="end">終了地点（ワールド座標）</param>
        /// <param name="collideWith">コリジョン対象のビット列</param>
        /// <returns></returns>
        public IEnumerable<RaycastResult> RayCast (Vector3 start, Vector3 end, int collideWith = -1) {

            var from = new BulletSharp.Vector3 (start.X, start.Y, start.Z);
            var to = new BulletSharp.Vector3 (end.X, end.Y, end.Z);

            using (var result = new CollisionWorld.AllHitsRayResultCallback (from, to)) {
                result.CollisionFilterMask = (CollisionFilterGroups)collideWith;

                // BulletPhysics のレイキャスト
                wld.RayTest (from, to, result);

                if (!result.HasHit) {
                    return new RaycastResult[0];
                }


                var n = result.HitFractions.Count ();
                var results = new RaycastResult[n];
                for (var i = 0; i < n; i++) {
                    var frac = result.HitFractions[i];
                    var dist = frac * (start - end).Length;
                    var node = ((CollisionObject)result.CollisionObjects[i].UserObject).Node;
                    var point = result.HitPointWorld[i].ToDD ();
                    var normal =   result.HitNormalWorld[i].ToDD();
                    results[i] = new RaycastResult (frac, dist, node, point, normal);
                }
                return results.OrderBy (x => x.Fraction);
            }
        }

        /// <summary>
        /// スィープ判定
        /// </summary>
        /// <remarks>
        /// 指定のコリジョン形状を移動ベクトルだけ移動し、他のコリジョン物体と衝突するかどうかを検査します。
        /// </remarks>
        /// <param name="obj">コリジョン オブジェクト</param>
        /// <param name="move">移動ベクトル</param>
        /// <returns></returns>
        public RaycastResult Sweep (CollisionObject obj, Vector3 move) {
            if (obj == null) {
                return new RaycastResult ();
            }
            var start = (obj.Node.Position + obj.Offset).ToBullet();
            var end = start + move.ToBullet ();

            var proxy = obj.Data.BroadphaseHandle;
            var shape = obj.Data.CollisionShape;
            var rot = obj.Node.Orientation.ToBullet();

            using (var result = new CollisionWorld.ClosestConvexResultCallback (start, end)) {
                result.CollisionFilterGroup = (CollisionFilterGroups)obj.Node.GroupID;
                result.CollisionFilterMask = (CollisionFilterGroups)obj.CollisionMask;

                var from = Matrix.RotationQuaternion(rot) *  Matrix.Translation (start);
                var to = Matrix.RotationQuaternion(rot) * Matrix.Translation (end);

                // メモ：
                // ConvexSweepTest() は自分自身にもヒットする実装としない実装があるので、
                // ここで一時的に自分のコリジョンを無効化する必要がある。
               var tmp = proxy.CollisionFilterGroup;
                proxy.CollisionFilterGroup = CollisionFilterGroups.None;

                wld.ConvexSweepTest (shape as ConvexShape,
                                     from,
                                     to,
                                     result);

                proxy.CollisionFilterGroup = tmp;

                if (!result.HasHit) {
                    return new RaycastResult ();
                }
                var frac = result.ClosestHitFraction;
                var dist = frac * move.Length;
                var hitNode = (result.CollisionObject.UserObject as CollisionObject).Node;
                var point = result.HitPointWorld.ToDD ();
                var normal = result.HitNormalWorld.ToDD ();
                
                return new RaycastResult (frac, dist, hitNode, point, normal);
            }

        }

        /// <inheritdoc/>
        void System.IDisposable.Dispose () {
            if (wld != null) {
                foreach (var obj in wld.CollisionObjectArray.ToArray()) {
                    var col = obj as BulletSharp.CollisionObject;
                    wld.RemoveCollisionObject (col);
                    col.Dispose ();
                }

                wld.Dispose ();
                this.wld = null;
            }
        }

        /// <inheritdoc/>
        public override string ToString () {
            return string.Format("Objects: {0}", CollisionObjectCount);
        }
    }
}
