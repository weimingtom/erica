using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DatabaseSample {
    public class MyCharacter : Component {

        string name;
        DB.AkatokiEntities1 db;
        DB.Character status;

        public MyCharacter (string name) {
            this.name = name;
            this.db = new DB.AkatokiEntities1 ();
            this.status = db.Characters.Find (name);
        }

        public string Name {
            get { return name; }
        }

        public string FullName {
            get { return status.FullName; }
        }

        public string FullNameYomi {
            get { return status.FullNameYomi; }
        }

        public string MagicItemName {
            get { return status.MagicItemName; }
        }

        public Texture GetTexture (string section, string category) {
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
