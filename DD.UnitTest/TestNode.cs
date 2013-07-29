﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestNode {
        [TestMethod]
        public void Test_New () {
            var node = new Node ("Node1");

            Assert.AreEqual ("Node1", node.Name);
            Assert.AreEqual (0, node.X);
            Assert.AreEqual (0, node.Y);
            Assert.AreEqual (null, node.Parent);
            Assert.AreEqual (true, node.Drawable);
            Assert.AreEqual (true, node.Updatable);
            Assert.AreEqual (true, node.Animatable);
            Assert.AreEqual (true, node.Collidable);
            Assert.AreEqual (0, node.ChildCount);
            Assert.AreEqual (0, node.ComponentCount);
            Assert.AreEqual (0x0000ffffu, node.GroupID);
            Assert.AreEqual (1.0f, node.Opacity);
            Assert.AreEqual (0, node.UserData.Count ());
            Assert.AreEqual (0, node.DrawPriority);
            Assert.AreEqual (0, node.UpdatePriority);
        }

        [TestMethod]
        public void Test_Name () {
            var node = new Node ("Name1");

            Assert.AreEqual ("Name1", node.Name);

            node.Name = "Name2";
            Assert.AreEqual ("Name2", node.Name);
        }

        [TestMethod]
        public void Test_UniqueID () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");

            Assert.AreNotEqual (node2.UniqueID, node1.UniqueID);
        }

        [TestMethod]
        public void Test_GroupID () {
            var node = new Node ("Node");

            node.GroupID = 0x12345678u;
            Assert.AreEqual (0x12345678u, node.GroupID);
        }

        [TestMethod]
        public void Test_Opacity () {
            var node = new Node ();

            node.Opacity = 0.5f;
            Assert.AreEqual (0.5f, node.Opacity);
        }

        [TestMethod]
        public void Test_Drawable () {
            var node = new Node ("Node");

            node.Drawable = false;
            Assert.AreEqual (false, node.Drawable);

            node.Drawable = true;
            Assert.AreEqual (true, node.Drawable);
        }

        [TestMethod]
        public void Test_Updatable () {
            var node = new Node ("Node");

            node.Updatable = false;
            Assert.AreEqual (false, node.Updatable);

            node.Updatable = true;
            Assert.AreEqual (true, node.Updatable);
        }

        [TestMethod]
        public void Test_Animatable () {
            var node = new Node ("Node");

            node.Animatable = false;
            Assert.AreEqual (false, node.Animatable);

            node.Animatable = true;
            Assert.AreEqual (true, node.Animatable);
        }

        [TestMethod]
        public void Test_Collidable () {
            var node = new Node ("Node");

            node.Collidable = false;
            Assert.AreEqual (false, node.Collidable);

            node.Collidable = true;
            Assert.AreEqual (true, node.Collidable);
        }

        [TestMethod]
        public void Test_DrawPriority () {
            var node = new Node ("Node");

            node.DrawPriority = 1;
            Assert.AreEqual (1, node.DrawPriority);

            node.DrawPriority = -1;
            Assert.AreEqual (-1, node.DrawPriority);
        }

        [TestMethod]
        public void Test_UpdatePriority () {
            var node = new Node ("Node");

            node.UpdatePriority = 1;
            Assert.AreEqual (1, node.UpdatePriority);

            node.UpdatePriority = -1;
            Assert.AreEqual (-1, node.UpdatePriority);
        }

        [TestMethod]
        public void Test_UserData () {
            var node = new Node ("Node");
            node.UserData.Add ("Key1", 1);
            node.UserData.Add ("Key2", 2);

            Assert.AreEqual (1, (int)node.UserData["Key1"]);
            Assert.AreEqual (2, (int)node.UserData["Key2"]);
        }


        [TestMethod]
        public void Test_AddChild () {
            var nod1 = new Node ("Node1");
            var nod2 = new Node ("Node2");
            var nod3 = new Node ("Node3");

            nod1.AddChild (nod2);
            nod1.AddChild (nod3);

            Assert.AreEqual (2, nod1.ChildCount);
            Assert.AreEqual (2, nod1.Children.Count ());
            Assert.AreEqual (nod2, nod1.GetChild (0));
            Assert.AreEqual (nod3, nod1.GetChild (1));

            Assert.AreEqual (nod1, nod2.Parent);
            Assert.AreEqual (nod1, nod3.Parent);
        }

        [TestMethod]
        public void Test_RemoveChild () {
            var nod1 = new Node ("Node1");
            var nod2 = new Node ("Node2");
            var nod3 = new Node ("Node3");

            nod1.AddChild (nod2);
            nod1.AddChild (nod3);

            Assert.AreEqual (2, nod1.ChildCount);

            nod1.RemoveChild (nod2);
            nod1.RemoveChild (nod3);

            Assert.AreEqual (0, nod1.ChildCount);
        }

        [TestMethod]
        public void Test_Downwards () {
            var nod1 = new Node ("Node1");
            var nod2 = new Node ("Node2");
            var nod3 = new Node ("Node3");
            var nod4 = new Node ("Node4");
            var nod5 = new Node ("Node5");

            nod1.AddChild (nod2);
            nod1.AddChild (nod3);
            nod2.AddChild (nod4);
            nod3.AddChild (nod5);

            // 幅優先
            var nodes = nod1.Downwards.ToArray ();
            Assert.AreEqual (5, nodes.Count ());
            Assert.AreEqual (nod1, nodes[0]);
            Assert.AreEqual (nod2, nodes[1]);
            Assert.AreEqual (nod3, nodes[2]);
            Assert.AreEqual (nod4, nodes[3]);
            Assert.AreEqual (nod5, nodes[4]);
        }

        [TestMethod]
        public void Test_Upwards () {
            var nod1 = new Node ("Node1");
            var nod2 = new Node ("Node2");
            var nod3 = new Node ("Node3");
            var nod4 = new Node ("Node4");
            var nod5 = new Node ("Node5");

            nod1.AddChild (nod2);
            nod1.AddChild (nod3);
            nod2.AddChild (nod4);
            nod3.AddChild (nod5);

            var nodes = nod5.Upwards.ToArray ();
            Assert.AreEqual (3, nodes.Count ());
            Assert.AreEqual (nod5, nodes[0]);
            Assert.AreEqual (nod3, nodes[1]);
            Assert.AreEqual (nod1, nodes[2]);
        }

        [TestMethod]
        public void Test_Root () {
            var nod1 = new Node ("Node1");
            var nod2 = new Node ("Node2");
            var nod3 = new Node ("Node3");
            nod1.AddChild (nod2);
            nod2.AddChild (nod3);

            Assert.AreEqual (nod1, nod1.Root);
            Assert.AreEqual (nod1, nod2.Root);
            Assert.AreEqual (nod1, nod3.Root);
        }


        [TestMethod]
        public void Test_Attach () {
            var node = new Node ();
            var comp1 = new Component ();
            var comp2 = new Component ();

            node.Attach (comp1);
            node.Attach (comp2);

            Assert.AreEqual (2, node.ComponentCount);
            Assert.AreEqual (2, node.Components.Count ());
            Assert.AreEqual (comp1, node.GetComponent (0));
            Assert.AreEqual (comp2, node.GetComponent (1));

            Assert.AreEqual (node, comp1.Node);
            Assert.AreEqual (node, comp2.Node);
        }

        [TestMethod]
        public void Test_Detach () {
            var node = new Node ();
            var comp1 = new Component ();
            var comp2 = new Component ();

            node.Attach (comp1);
            node.Attach (comp2);
            node.Detach (comp1);
            node.Detach (comp2);

            Assert.AreEqual (0, node.ComponentCount);
            Assert.AreEqual (null, comp1.Node);
            Assert.AreEqual (null, comp2.Node);
        }

        [TestMethod]
        public void Test_IsComponent () {
            var node = new Node ();
            var comp = new Sprite (16, 16);
            node.Attach (comp);

            Assert.AreEqual (true, node.Is<Sprite> ());
            Assert.AreEqual (false, node.Is<Camera> ());
        }


        [TestMethod]
        public void Test_GetComponent () {
            var node = new Node ();
            var comp1 = new Component ();
            var comp2 = new Component ();
            node.Attach (comp1);
            node.Attach (comp2);

            Assert.AreEqual (comp1, node.GetComponent<Component> ());
            Assert.AreEqual (comp1, node.GetComponent<Component> (0));
            Assert.AreEqual (comp2, node.GetComponent<Component> (1));
        }

        [TestMethod]
        public void Test_Find_by_Name () {
            var nod1 = new Node ("Node1");
            var nod2 = new Node ("Node2");
            var nod3 = new Node ("Node3");
            nod1.AddChild (nod2);
            nod1.AddChild (nod3);

            Assert.AreEqual (nod1, nod1.Find ("Node1"));
            Assert.AreEqual (nod2, nod1.Find ("Node2"));
            Assert.AreEqual (nod3, nod1.Find ("Node3"));
            Assert.AreEqual (null, nod1.Find ("Node4"));
        }

        [TestMethod]
        public void Test_Find_by_Predicate () {
            var nod1 = new Node ("Node1");
            var nod2 = new Node ("Node2");
            var nod3 = new Node ("Node3");
            nod1.AddChild (nod2);
            nod1.AddChild (nod3);

            Assert.AreEqual (nod1, nod1.Find (x => x.Name == "Node1"));
            Assert.AreEqual (nod2, nod1.Find (x => x.Name == "Node2"));
            Assert.AreEqual (nod3, nod1.Find (x => x.Name == "Node3"));
            Assert.AreEqual (null, nod1.Find (x => x.Name == "Node4"));
        }
    
        [TestMethod]
        public void Test_GlobalTransform () {
            var nod1 = new Node ();
            var nod2 = new Node ();
            var nod3 = new Node ();
            nod2.Translation = new Vector3 (1, 2, 3);
            nod2.Scale = new Vector3 (2, 2, 2);
            nod3.Translation = new Vector3 (2, 2, 2);

            nod1.AddChild (nod2);
            nod2.AddChild (nod3);

            Vector3 translation;
            Quaternion rotation;
            Vector3 scale;

            nod3.GlobalTransform.Decompress (out translation, out rotation, out scale);

            Assert.AreEqual (new Vector3 (5, 6, 7), translation);
            Assert.AreEqual (Quaternion.Identity, rotation);
            Assert.AreEqual (new Vector3 (2, 2, 2), scale);
        }

        [TestMethod]
        public void Test_LocalTransform () {
            var nod1 = new Node ();
            var nod2 = new Node ();
            var nod3 = new Node ();
            nod2.Translation = new Vector3 (1, 2, 3);
            nod2.Scale = new Vector3 (2, 2, 2);
            nod3.Translation = new Vector3 (2, 2, 2);

            nod1.AddChild (nod2);
            nod2.AddChild (nod3);

            Vector3 translation;
            Quaternion rotation;
            Vector3 scale;

            nod3.LocalTransform.Decompress (out translation, out rotation, out scale);

            Assert.AreEqual (new Vector3 (-2.5f, -3f, -3.5f), translation);
            Assert.AreEqual (Quaternion.Identity, rotation);
            Assert.AreEqual (new Vector3 (0.5f, 0.5f, 0.5f), scale);
        }

        [TestMethod]
        public void Test_ParentTransform () {
            var nod1 = new Node ();
            var nod2 = new Node ();
            var nod3 = new Node ();
            nod2.Translation = new Vector3 (1, 2, 3);
            nod2.Scale = new Vector3 (2, 2, 2);
            nod3.Translation = new Vector3 (2, 2, 2);

            nod1.AddChild (nod2);
            nod2.AddChild (nod3);

            Vector3 translation;
            Quaternion rotation;
            Vector3 scale;

            nod3.ParentTransform.Decompress (out translation, out rotation, out scale);

            Assert.AreEqual (new Vector3 (-0.5f, -1.0f, -1.5f), translation);
            Assert.AreEqual (Quaternion.Identity, rotation);
            Assert.AreEqual (new Vector3 (0.5f, 0.5f, 0.5f), scale);
        }



        [TestMethod]
        public void Test_Point () {
            var nod1 = new Node ();
            var nod2 = new Node ();
            var nod3 = new Node ();
            nod2.Translation = new Vector3 (1, 2, 3);
            nod3.Translation = new Vector3 (2, 2, 2);
            nod1.AddChild (nod2);
            nod2.AddChild (nod3);

            Vector3 T;
            Quaternion R;
            Vector3 S;

            nod3.GlobalTransform.Decompress (out T, out R, out S);

            Assert.AreEqual (T.X, nod3.Position.X);
            Assert.AreEqual (T.Y, nod3.Position.Y);
            Assert.AreEqual (T.Z, nod3.Position.Z);
        }


        [TestMethod]
        public void Test_SetGlobalTranslation () {
            var nod1 = new Node ();
            var nod2 = new Node ();
            var nod3 = new Node ();
            nod2.Translation = new Vector3 (1, 2, 3);
            nod2.Scale = new Vector3 (2, 2, 2);
            nod3.Translation = new Vector3 (2, 2, 2);

            nod1.AddChild (nod2);
            nod2.AddChild (nod3);

            // nod3.GlobalTransform = 
            //   T = (5,6,7)
            //   R = (0,0,0,1)
            //   S = (2,2,2)

            nod3.SetGlobalTranslation (5, 6, 7);

            Assert.AreEqual (new Vector3 (2, 2, 2), nod3.Translation);
        }

        [TestMethod]
        public void Test_SetGlobalScale () {
            var nod1 = new Node ();
            var nod2 = new Node ();
            var nod3 = new Node ();
            nod2.Translation = new Vector3 (1, 2, 3);
            nod2.Scale = new Vector3 (2, 2, 2);
            nod3.Translation = new Vector3 (2, 2, 2);

            nod1.AddChild (nod2);
            nod2.AddChild (nod3);

            // nod3.GlobalTransform = 
            //   T = (5,6,7)
            //   R = (0,0,0,1)
            //   S = (2,2,2)

            nod3.SetGlobalScale (2, 2, 2);

            Assert.AreEqual (new Vector3 (1, 1, 1), nod3.Scale);
        }

        [TestMethod]
        public void Test_SetGlobalRotation () {
            var nod1 = new Node ();
            var nod2 = new Node ();
            var nod3 = new Node ();
            nod2.Translation = new Vector3 (1, 2, 3);
            nod2.Scale = new Vector3 (2, 2, 2);
            nod2.Rotation = new Quaternion (45, 0, 0, 1);
            nod3.Translation = new Vector3 (2, 2, 2);
            nod3.Rotation = new Quaternion (45, 0, 0, 1);

            nod1.AddChild (nod2);
            nod2.AddChild (nod3);

            // nod3.GlobalTransform = 
            //   T = (1,7.656854,7)
            //   R = (0,0,0.7071068,0.7071068)
            //   S = (2,2,2)
            /*
            Vector3 T;
            Quaternion R;
            Vector3 S;

            nod3.GlobalTransform.Decompress (out T, out R, out S);

            Debug.WriteLine ("T = " + T);
            Debug.WriteLine ("R = " + R);
            Debug.WriteLine ("S = " + S);
             * */

            nod3.SetGlobalRotation (Quaternion.Set (0, 0, 0.7071068f, 0.7071068f, false));

            var expected = new Quaternion (45, 0, 0, 1);

            Assert.AreEqual (expected, nod3.Rotation);
        }

    }
}