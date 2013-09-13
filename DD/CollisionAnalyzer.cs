using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;
using System.Diagnostics;

namespace DD {
    public class CollisionAnalyzer : Component, System.IDisposable {

        #region Field
        BulletSharp.CollisionWorld wld;
        #endregion

        #region Constructor
        public CollisionAnalyzer () {
            var cc = new DefaultCollisionConfiguration ();
            var dispatcher = new CollisionDispatcher (cc);
            var broadphase = new DbvtBroadphase ();
            //var solver = new SequentialImpulseConstraintSolver ();

            broadphase.OverlappingPairCache.SetInternalGhostPairCallback (new GhostPairCallback ());

            this.wld = new DiscreteDynamicsWorld (dispatcher, broadphase, null, cc);
        }
        #endregion

        public BulletSharp.CollisionWorld CollisionWorld {
            get { return wld; }
        }

        public int CollisionObjectCount {
            get { return wld.NumCollisionObjects; }
        }

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



        public void Analyze () {
            // コリジョンの準備 (最新の座標をコリジョン世界に反映)
            foreach (var node in Node.Downwards) {
                foreach (var cmp in node.Components) {
                    cmp.OnPrepareCollisions ();
                }
            }

            // コリジョンの解決
            wld.PerformDiscreteCollisionDetection ();

            // コンポーネントのコリジョン更新処理
            // OnColisionEnter()とかOnCollisionExit()の呼び出し
            foreach (var node in Node.Downwards) {
                foreach (var cmp in node.Components) {
                    cmp.OnCollisionResolved ();
                }
            }

        }

        private static Matrix ToBullet (Matrix4x4 m) {
            var mat = new Matrix ();
            m = m.Transpose ();
            mat.M11 = m[0];
            mat.M12 = m[1];
            mat.M13 = m[2];
            mat.M14 = m[3];
            mat.M21 = m[4];
            mat.M22 = m[5];
            mat.M23 = m[6];
            mat.M24 = m[7];
            mat.M31 = m[8];
            mat.M32 = m[9];
            mat.M33 = m[10];
            mat.M34 = m[11];
            mat.M41 = m[12];
            mat.M42 = m[13];
            mat.M43 = m[14];
            mat.M44 = m[15];
            return mat;
        }


        public static float Distance (CollisionObject colA, CollisionObject colB) {
            if (colA == null || colA.Shape == null || colB == null || colB.Shape == null) {
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
            input.TransformA = ToBullet (traA); // colA.ghostObject.WorldTransform;
            input.TransformB = ToBullet (traB); //traAcolB.ghostObject.WorldTransform;
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


        public IEnumerable<RaycastResult> RayCast (Vector3 start, Vector3 end, int collideWith = -1) {

            var from = new BulletSharp.Vector3 (start.X, start.Y, start.Z);
            var to = new BulletSharp.Vector3 (end.X, end.Y, end.Z);
            
            using (var result = new CollisionWorld.AllHitsRayResultCallback (from, to)) {
                result.CollisionFilterMask = (CollisionFilterGroups)collideWith;

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
                    var point = new Vector3 (result.HitPointWorld[i].X, result.HitPointWorld[i].Y, result.HitPointWorld[i].Z);
                    var normal = new Vector3 (result.HitNormalWorld[i].X, result.HitNormalWorld[i].Y, result.HitNormalWorld[i].Z);
                    results[i] = new RaycastResult (frac, dist, node, point, normal);
                }
                return results.OrderBy (x => x.Fraction);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// <note>
        /// </note>
        /// </remarks>
        /// <param name="obj"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        public RaycastResult Sweep (CollisionObject obj, Vector3 move) {
            if (obj == null) {
                return new RaycastResult ();
            }
            var start = obj.Node.Position + obj.Offset;
            var end = start + move;

            var proxy = obj.Data.BroadphaseHandle;
            var shape = obj.Data.CollisionShape;
            var from = new BulletSharp.Vector3 (start.X, start.Y, start.Z);
            var to = new BulletSharp.Vector3 (end.X, end.Y, end.Z);

            using (var result = new CollisionWorld.ClosestConvexResultCallback (from, to)) {
                result.CollisionFilterGroup = (CollisionFilterGroups)obj.Node.GroupID;
                result.CollisionFilterMask = (CollisionFilterGroups)obj.CollideWith;

                var tmp = proxy.CollisionFilterGroup;
                proxy.CollisionFilterGroup = CollisionFilterGroups.None;
                
                wld.ConvexSweepTest (shape as ConvexShape,
                                     Matrix.Translation (from),
                                     Matrix.Translation (to),
                                    result);

                proxy.CollisionFilterGroup = tmp;

                if (!result.HasHit) {
                    return new RaycastResult ();
                }
                var frac = result.ClosestHitFraction;
                var dist = frac * move.Length;
                var hitNode = ((CollisionObject)result.CollisionObject.UserObject).Node;
                var point = new Vector3 (result.HitPointWorld.X, result.HitPointWorld.Y, result.HitPointWorld.Z);
                var normal = new Vector3 (result.HitNormalWorld.X, result.HitNormalWorld.Y, result.HitNormalWorld.Z);

                return new RaycastResult (frac, dist, hitNode, point, normal);
            }

        }
        
        public void Dispose () {
            if (wld != null) {
                wld.Dispose ();
                this.wld = null;
            }
        }
    }
}
