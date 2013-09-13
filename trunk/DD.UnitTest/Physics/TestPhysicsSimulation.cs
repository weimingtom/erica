using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;
using System.Diagnostics;

// メモ：
// 停止位置の正確な値は BulletPhysics のバージョンによって異なる
// だいたいあっていればいい

namespace DD.UnitTest.Physics {
    [TestClass]
    public class TestPhysicsSimulation {
        [TestMethod]
        public void Test_Gravity () {

            var ball1 = new Node ("FreeFall");
            ball1.Attach(new RigidBody ());
            ball1.RigidBody.Shape = new SphereShape (1);

            var ball2 = new Node ("NoFall");
            ball2.Attach (new RigidBody ());
            ball2.RigidBody.Shape = new SphereShape (1);
            ball2.RigidBody.Mass = 0;   // = static, no gravity
            ball2.Translate (10, 0, 0);

            var wld = new World ();
            wld.AddChild (ball1);
            wld.AddChild (ball2);

            wld.PhysicsSimulator.SetGravity (0, -9.8f, 0);

            for (var i = 0; i < 250; i++) {
                wld.PhysicsUpdate (4);
                Debug.WriteLine ("Ball1 = {0}, Ball2 = {1}", ball1.Position, ball2.Position);
            }

            // Ball1 のみ重力による自由落下 (d = 4.9t^2)
            Assert.AreEqual (new Vector3(0, -4.9817f, 0), ball1.Position);
            Assert.AreEqual (new Vector3 (10, 0, 0), ball2.Position);

            wld.Destroy ();
        }

        [TestMethod]
        public void Test_Static () {
            var ball1 = new Node ("Static");
            ball1.Attach (new RigidBody ());
            ball1.RigidBody.Shape = new SphereShape (1);
            ball1.RigidBody.Mass = 0;   // = static

            var ball2 = new Node ("Dynamic");
            ball2.Attach (new RigidBody ());
            ball2.RigidBody.Shape = new SphereShape (1);
            ball2.RigidBody.Mass = 1;   // = dynamic

            
            var wld = new World ();
            wld.AddChild (ball1);
            wld.AddChild (ball2);

            ball2.Translate (0, 10, 0);

            wld.PhysicsSimulator.SetGravity (0, -9.8f, 0);

            for (var i = 0; i < 1000; i++) {
                wld.PhysicsUpdate (33);
                Debug.WriteLine ("Ball1 = {0}, Ball2 = {1}", ball1.Position, ball2.Position);
            }

            // static 体は、
            //  - 重力の影響を受けない
            //  - 衝突しても動かない
            Assert.AreEqual (new Vector3 (0, 0, 0), ball1.Position);
            Assert.AreEqual (new Vector3 (0, 2, 0), ball2.Position);

            wld.Destroy ();
        }

        [TestMethod]
        public void Test_Sphere_to_Box () {

            var mat = new PhysicsMaterial ();
            mat.Restitution=1;

            var sph = new Node ("Sphere");
            sph.Attach(new RigidBody ());
            sph.RigidBody.Mass = 1;
            sph.RigidBody.Material = mat;
            sph.RigidBody.Shape = new SphereShape (1);

            var box = new Node ("Box");
            box.Attach(new RigidBody ());
            box.RigidBody.Mass = 0;
            box.RigidBody.Material = mat;
            box.RigidBody.Shape = new BoxShape (1,1,1);

            var wld = new World ("");
            wld.AddChild (sph);
            wld.AddChild (box);

            wld.PhysicsSimulator.SetGravity (0, -9.8f, 0);

            sph.Translate (0, 10, 0);

            for (var i = 0; i < 100; i++) {
                wld.PhysicsUpdate (33);
                Debug.WriteLine ("Sphere = {0}, Box = {1}", sph.Position, box.Position);
            }

            wld.Destroy ();
        }


        [TestMethod]
        public void Test_ApplyForce () {

            var ball1 = new Node ("Ball1");
            var rb1 = new RigidBody ();
            rb1.Mass = 1;
            rb1.Shape = new SphereShape (1);
            rb1.Material = new PhysicsMaterial ();
            rb1.Material.Restitution = 1;
            ball1.Attach (rb1);
       
            var ball2 = new Node ("Ball2");
            var rb2 = new RigidBody ();
            rb2.Mass = 1;
            rb2.Shape = new SphereShape (1);
            rb2.Material = new PhysicsMaterial ();
            rb2.Material.Restitution = 1;
            ball2.Attach (rb2);

           

            var wld = new World ("");
            wld.AddChild (ball1);
            wld.AddChild (ball2);

            wld.PhysicsSimulator.SetGravity (0, 0, 0);

            ball2.Translate (10, 0, 0);

            // ①→②
            //   ①②→ (運動量保存の法則)
            ball1.RigidBody.ApplyForce (1000, 0, 0);
            
            for (var i = 0; i < 100; i++) {
                wld.PhysicsUpdate (33);
                //Debug.WriteLine ("Ball1 = {0}, Ball2 = {1}", ball1.Position, ball2.Position);
            }

            Assert.AreEqual (new Vector3 (8.0333f, 0, 0), ball1.Position);
            Assert.AreEqual (new Vector3 (29.7445f, 0, 0), ball2.Position);

            wld.Destroy ();
        }


        [TestMethod]
        public void Test_ApplyImpulse () {

            var box1 = new Node ("Box1");
            box1.Attach (new RigidBody());
            box1.RigidBody.Shape = new BoxShape (1,1,1);
            box1.RigidBody.Material = new PhysicsMaterial ();
            box1.RigidBody.Mass = 1;
            box1.RigidBody.Material.Restitution = 1;

            var box2 = new Node ("Box2");
            box2.Attach (new RigidBody ());
            box2.RigidBody.Shape = new BoxShape (1, 1, 1);
            box2.RigidBody.Material = new PhysicsMaterial ();
            box2.RigidBody.Mass = 1;
            box2.RigidBody.Material.Restitution = 1;


            var wld = new World ("");
            wld.AddChild (box1);
            wld.AddChild (box2);

            wld.PhysicsSimulator.SetGravity (0, 0, 0);

            box2.Translate (10, 0, 0);

            
            // ①→②
            //   ①②→ (運動量保存の法則)
            // 
            // (impulse)=(force)x(dt)
            // ApplyForce()は BulletPhysics の1タイムステップ分(16msec)の力積を与えたに等しい
            // 自分で力積を計算するなら ApplyImpulse() を使用する 
            box1.RigidBody.ApplyImpulse (1000 * 0.0166667f, 0, 0);

            for (var i = 0; i < 100; i++) {
                wld.PhysicsUpdate (33);
                //Debug.WriteLine ("Box1 = {0}, Box2 = {1}", box1.Position, box2.Position);
            }

            Assert.AreEqual (new Vector3 (8.03333f, 0, 0), box1.Position);
            Assert.AreEqual (new Vector3 (29.7445f, 0, 0), box2.Position);

            wld.Destroy ();
        }

        [TestMethod]
        public void Test_Restitution () {

            var sph1 = new Node ("Sphere1");
            sph1.Attach (new RigidBody ());
            sph1.RigidBody.Material = new PhysicsMaterial ();
            sph1.RigidBody.Mass = 1;
            sph1.RigidBody.Material.Restitution = 0;   // 反射 0%
            sph1.RigidBody.Shape = new SphereShape (1);

            var sph2 = new Node ("Sphere2");
            sph2.Attach (new RigidBody ());
            sph2.RigidBody.Material = new PhysicsMaterial ();
            sph2.RigidBody.Mass = 1;
            sph2.RigidBody.Material.Restitution = 1;    // 反射 100%
            sph2.RigidBody.Shape = new SphereShape (1);

            var plane = new Node ("Plane");
            plane.Attach (new RigidBody ());
            plane.RigidBody.Material = new PhysicsMaterial ();
            plane.RigidBody.Mass = 0;
            plane.RigidBody.Material.Restitution = 1;
            plane.RigidBody.Shape = new BoxShape (100, 1, 100);

            var wld = new World ("");
            wld.AddChild (sph1);
            wld.AddChild (sph2);
            wld.AddChild (plane);

            wld.PhysicsSimulator.SetGravity (0, -9.8f, 0);

            sph1.Translate (0, 10, 0);
            sph2.Translate (10, 10, 0);

            for (var i = 0; i < 100; i++) {
                wld.PhysicsUpdate (33);
                Debug.WriteLine ("Sphere1 = {0}, Sphere2 = {1}", sph1.Position, sph2.Position);
            }

            //Assert.AreEqual (new Vector3 (0, 2.0000f, 0), sph1.Position);
            //Assert.AreEqual (new Vector3(10.00011f, 7.681699f,0.00011f), sph2.Position);

            wld.Destroy ();
        }

        [TestMethod]
        public void Test_Friction () {

            var box1 = new Node ("Box1");
            box1.Attach (new RigidBody ());
            box1.RigidBody.Material = new PhysicsMaterial ();
            box1.RigidBody.Mass = 1;
            box1.RigidBody.Material.Friction = 0.8f;   // 摩擦 0.5
            box1.RigidBody.Shape = new BoxShape (1,1,1);

            var box2 = new Node ("Box2");
            box2.Attach (new RigidBody ());
            box2.RigidBody.Material = new PhysicsMaterial ();
            box2.RigidBody.Mass = 1;
            box2.RigidBody.Material.Friction = 0;    // 摩擦 0
            box2.RigidBody.Shape = new BoxShape (1, 1, 1);

            var plane = new Node ("Plane");
            plane.Attach (new RigidBody ());
            plane.RigidBody.Material = new PhysicsMaterial ();
            plane.RigidBody.Mass = 0;
            plane.RigidBody.Material.Friction = 0.8f;
            plane.RigidBody.Shape = new BoxShape (100, 1, 100);

            var wld = new World ("");
            wld.AddChild (box1);
            wld.AddChild (box2);
            wld.AddChild (plane);

            wld.PhysicsSimulator.SetGravity (0, -9.8f, 0);

            box1.Translate (0, 2, 0);
            box2.Translate (10, 2, 0);

            // ①→ ②→
            //     ①・       ②→
            box1.RigidBody.ApplyForce (1000, 0, 0);
            box2.RigidBody.ApplyForce (1000, 0, 0);

            for (var i = 0; i < 100; i++) {
                wld.PhysicsUpdate (33);
                Debug.WriteLine ("Box1 = {0}, Box2 = {1}", box1.Position, box2.Position);
            }

            Assert.AreEqual (new Vector3 (18.9796f, 2, 0), box1.Position);
            Assert.AreEqual (new Vector3 (37.7778f, 2, 0), box2.Position);

            wld.Destroy ();
        }

        [TestMethod]
        public void Test_Rotation () {

            var sph = new Node ("Sphere1");
            sph.Attach (new RigidBody ());
            sph.RigidBody.Shape = new BoxShape (1, 1, 1);

            var plane = new Node ("Plane");
            plane.Attach (new RigidBody ());
            plane.RigidBody.Mass = 0;             // =static
            plane.RigidBody.Shape = new BoxShape (100, 1, 100);

            var wld = new World ("");
            wld.AddChild (sph);
            wld.AddChild (plane);

            wld.PhysicsSimulator.SetGravity (0, -9.8f, 0);

            sph.Translate (0, 10, 0);
            plane.Rotate (-45, 0, 0, 1);

            for (var i = 0; i < 100; i++) {
                wld.PhysicsUpdate (33);
                Debug.WriteLine ("Sphere = {0}", sph.Position);
            }

            Assert.AreEqual (new Vector3 (2.7034f, 0.7107f, 0), sph.Position);

            wld.Destroy ();
        }

        [TestMethod]
        public void Test_Damping () {

            var box = new Node ("Box1");
            box.Attach (new RigidBody ());
            box.RigidBody.Material = new PhysicsMaterial ();
            box.RigidBody.Mass = 1;
            box.RigidBody.Material.LinearDamping= 0.9f;   // 減衰 90%
            box.RigidBody.Shape = new BoxShape (1, 1, 1);

            var wld = new World ("");
            wld.AddChild (box);

            wld.PhysicsSimulator.SetGravity (0, 0, 0);

            box.RigidBody.ApplyForce (1000, 0, 0);
 
            for (var i = 0; i < 100; i++) {
                wld.PhysicsUpdate (33);
                Debug.WriteLine ("Box = {0}", box.Position);
            }

            Assert.AreEqual (new Vector3 (7.2191f, 0, 0), box.Position);

            wld.Destroy ();
        }

    }
}
