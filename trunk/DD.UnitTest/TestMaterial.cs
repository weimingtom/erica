using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestMaterial {
        public class MyMaterial : Material {
            public MyMaterial (int value1, float value2, string value3) {
                this.Value1 = value1;
                this.Value2 = value2;
                this.Value3 = value3;
            }
            public int Value1 { get; set; }
            public float Value2 {get; set;}
            public string Value3 { get; set; }
        }

        [TestMethod]
        public void Test_GetHashValue () {
            var mat1 = new MyMaterial (1, 2.0f, "String");
            var mat2 = new MyMaterial (1, 2.0f, "String");
            var hash1 = mat1.GetHashValue ();
            var hash2 = mat2.GetHashValue ();

            Assert.AreEqual (hash1, hash2);

            mat1.Value1 = 0;
            mat2.Value1 = -1;

            hash1 = mat1.GetHashValue ();
            hash2 = mat2.GetHashValue ();

            Assert.AreNotEqual (hash1, hash2);
        }

    }
}
