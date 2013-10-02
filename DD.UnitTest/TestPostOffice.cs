using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestPostOffice {
        public class MyLetter : EventArgs {
            public MyLetter (string text) {
                this.Text = text;
            }
            public string Text { get; private set; }
        }
        public class MyComponent : Component {
            public int ReceivedMail { get; private set; }
            public override void OnMailBox (Node from, string to, object args) {
                this.ReceivedMail += 1;
            }
        }

        [TestMethod]
        public void Test_New () {
            var po = new PostOffice ();

            Assert.AreEqual (0, po.MailCount);
            Assert.AreEqual (0, po.Mails.Count ());
        }

        [TestMethod]
        public void Test_Post () {
            var node = new Node ("Node1");

            var po = new PostOffice ();
            po.Post (node, "Node2", null);

            Assert.AreEqual (1, po.MailCount);
            Assert.AreEqual (1, po.Mails.Count ());
        }

        [TestMethod]
        public void Test_Deliver () {
            var node = new Node ("Node");
            var mbox = new MailBox ("Address");
            node.Attach (mbox);

            var received = false;
            Node from = null;
            string address = "";
            string letter = "";

            mbox.Action += (frm, addr, let) => {
                received = true;
                from = frm;
                address = addr;
                letter = (string)let;
            };

            var wld = new World ();
            wld.AddChild (node);

            wld.PostOffice.Post (node, "Address", "Hello World");
            wld.PostOffice.Deliver ();

            Assert.AreEqual (true, received);
            Assert.AreEqual (node, from);
            Assert.AreEqual ("Address", address);
            Assert.AreEqual ("Hello World", letter);
        }

        /// <summary>
        /// メールボックスの中で自分自身に送るといろいろ不都合がある（あった）
        /// </summary>
        [TestMethod]
        public void Test_Deliver_to_MySelf () {
            var node = new Node ("Node");
            var mbox1 = new MailBox ("Address1");
            var mbox2 = new MailBox ("Address2");
            node.Attach (mbox1);
            node.Attach (mbox2);

            mbox1.Action += (frm, addr, let) => {
                // 2ndアドレスに送信
                mbox1.World.PostOffice.Post (node, "Address2", "Hello World 2");
            };

            var received = false;
            Node from = null;
            string address = "";
            string letter = "";

            mbox2.Action += (frm, addr, let) => {
                received = true;
                from = frm;
                address = addr;
                letter = (string)let;
            };

            var wld = new World ();
            wld.AddChild (node);

            wld.PostOffice.Post (node, "Address1", "Hello World 1");
            
            // node --> Address1
            wld.PostOffice.Deliver ();

            // node --> Address2
            wld.PostOffice.Deliver ();

            Assert.AreEqual (true, received);
            Assert.AreEqual (node, from);
            Assert.AreEqual ("Address2", address);
            Assert.AreEqual ("Hello World 2", letter);
        }

        [TestMethod]
        public void Test_Deliver_to_All () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            var mbox1 = new MailBox ("Node1");
            var mbox2 = new MailBox ("Node1");
            node1.Attach (mbox1);
            node2.Attach (mbox2);

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);

            var recved1 = false;
            var recved2 = false;

            mbox1.Action += (from, address, letter) => {
                recved1 = true;
            };
            mbox2.Action += (from, address, letter) => {
                recved2 = true;
            };

            // 宛先が "All" の場合は全ノードが受信する
            wld.PostOffice.Post (node1, "All", null);
            wld.PostOffice.Deliver ();

            Assert.AreEqual (true, recved1);
            Assert.AreEqual (true, recved2);
        }

        [TestMethod]
        public void Test_Receive_All () {
            var node1 = new Node ("Node1");

            // メールボックスの宛名が "All" の場合は全メッセージを受信する
            var mbox1 = new MailBox ("All");
            node1.Attach (mbox1);

            var recved = false;

            mbox1.Action += (from, address, args) => {
                recved = true;
            };

            var wld = new World ();
            wld.AddChild (node1);

            wld.PostOffice.Post (node1, "SomeAddress", null);
            wld.PostOffice.Deliver ();

            Assert.AreEqual (true, recved);
        }

    }
}
