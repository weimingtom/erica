using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;

namespace DD {
    public class CollisionObject : Component {

        #region Filed
        CollisionShape shape;
        Vector3 offset;

        PairCachingGhostObject ghostObject;
        List<BulletSharp.CollisionObject> overlaps;
        int collideWith;
        int ignoreWith;
        #endregion

        #region Constructor
        public CollisionObject () {
            this.shape = null;
            this.ghostObject = null;
            this.overlaps = new List<BulletSharp.CollisionObject> ();
            this.offset = new Vector3 (0, 0, 0);
            this.collideWith = -1;
            this.ignoreWith = 0;
        }
        #endregion

        #region Property
        public int CollideWith {
            get {return collideWith;}
            set {
                this.collideWith = value;
            }
        }

        public int IgnoreWith {
            get { return ignoreWith; }
            set { this.ignoreWith = value; }
        }


        public CollisionShape Shape {
            get { return shape; }
            set {
                if (value == null) {
                    throw new ArgumentNullException ("Shape is null");
                }
                this.shape = value;
                this.ghostObject = value.CreateGhostObject ();
                this.ghostObject.UserObject = this;
            }
        }

        public BulletSharp.CollisionObject Data {
            get { return ghostObject; }
            set { this.ghostObject = value as PairCachingGhostObject; }
        }

        public Vector3 Offset {
            get { return offset; }
            set { this.offset = value; }
        }

        public int OverlapCount {
            get { return overlaps.Count (); }
        }

        public IEnumerable<Node> Overlaps {
            get {
                var nodes = new List<Node> ();
                foreach (var col in overlaps) {
                    nodes.Add (((CollisionObject)col.UserObject).Node);
                }
                return nodes;
            }

        }
        #endregion


        #region Method
        public void SetOffset (float x, float y, float z) {
            this.offset = new Vector3 (x, y, z);
        }


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


        public override void OnPrepareCollisions () {

            // コリジョン ワールドに登録
            var wld = CollisionAnalyzer.CollisionWorld;
            if (ghostObject != null && !wld.CollisionObjectArray.Contains (ghostObject)) {
                wld.AddCollisionObject (ghostObject, 
                                        (CollisionFilterGroups)Node.GroupID, 
                                        (CollisionFilterGroups)(collideWith & ~ignoreWith));
            }


            var T = Node.Position;
            var R = Node.Orientation;

            var matT = Matrix.Translation (T.X + offset.X,
                               T.Y + offset.Y,
                               T.Z + offset.Z);
            var axis = R.Axis;
            var angle = R.Angle / 180f * (float)Math.PI;
            var matR = Matrix.RotationAxis (new BulletSharp.Vector3 (axis.X, axis.Y, axis.Z), angle);

            // DDワールドの座標をコリジョン ワールドへ反映
            this.ghostObject.WorldTransform = matR * matT;
        }

        public override void OnCollisionResolved () {

            var prev = this.overlaps;

            // 現在の衝突中のコリジョン オブジェクト一覧
            var next = new List<BulletSharp.CollisionObject> ();
            foreach (var pair in ghostObject.OverlappingPairs) {
                next.Add (pair as BulletSharp.CollisionObject);
            }

            // On Collision Enter
            foreach (var col in next) {
                if (!prev.Contains (col)) {
                    foreach (var cmp in Node.Components) {
                        cmp.OnCollisionEnter (((CollisionObject)col.UserObject).Node);
                    }
                }
            }

            // On Collision Exit
            foreach (var col in prev) {
                if (!next.Contains (col)) {
                    foreach (var cmp in Node.Components) {
                        cmp.OnCollisionExit (((CollisionObject)col.UserObject).Node);
                    }
                }
            }


            this.overlaps = next;
        }

        public override void OnDestroyed () {
            if (ghostObject != null) {
                ghostObject.Dispose ();
                this.ghostObject = null;
            }
        }

        public override void OnDetached () {
            // コリジョン ワールドから削除
            var wld = CollisionAnalyzer.CollisionWorld;
            if (ghostObject != null && wld.CollisionObjectArray.Contains (ghostObject)) {
                wld.RemoveCollisionObject (ghostObject);
            }

        }
        #endregion

    }
}
