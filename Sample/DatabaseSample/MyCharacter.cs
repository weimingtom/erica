using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DatabaseSample {
    public class MyCharacter : Component {

        string name;
        DB.Character data;

        public MyCharacter (string name) {
            this.name = name;
            this.data = Resource.GetDatabase<DB.AkatokiEntities1> ().Characters.Find (name);
            if (data == null) {
                throw new ArgumentException ("Name doesn't exist in Database");
            }
        }

        public string Name {
            get { return name; }
        }

        public string FullName {
            get { return data.FullName; }
        }

        public string FullNameYomi {
            get { return data.FullNameYomi; }
        }

        public string MagicItemName {
            get { return data.MagicItemName; }
        }

        public Texture GetTexture (string section, string category) {
            var db = Resource.GetDatabase<DB.AkatokiEntities1> ();
            var path = (from tex in db.Textures
                        where tex.Section == section
                        where tex.Category == category
                        where tex.CharacterName == name
                        select tex.FilePath).FirstOrDefault();

            return Resource.GetTexture (path);
        }

        public static Node Create (string name, Vector3 pos) {
            var cmp = new MyCharacter (name);

            var node = new Node (name);
            node.Attach (cmp);

            return node;
        }

    }
}
