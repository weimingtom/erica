using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Window;
using SFML.Graphics;

namespace DD {
    public class Square : Component{
        float width;
        float height;
        float lineWidth;
        Color color;
        Vector3 offset;

        public Square (float width, float height, float lineWidth) {
            this.width = width;
            this.height = height;
            this.lineWidth = lineWidth;
            this.color = Color.White;
        }

        public float Width {
            get { return width; }
            set { this.width = value; }
        }

        public float Height{
            get{return height;}
            set { this.height = value; }
        }

        public float LineWidth {
            get { return lineWidth; }
            set { this.lineWidth = value; }
        }

        public Color Color {
            get { return color; }
            set { this.color = value; }
        }

        public Vector3 Offset {
            get { return offset; }
            set { this.offset = value; }
        }

        public void Resize (float width, float height) {
            this.width = width;
            this.height = height;
        }

        public void SetColor (byte r, byte g, byte b, byte a) {
            this.color = new Color (r, g, b, a);
        }

        public void SetOffset (float x, float y, float z) {
            this.offset = new Vector3 (x, y, z);
        }

        public override void OnDraw (object window) {
            Vector3 T;
            Quaternion R;
            Vector3 S;
            Node.GlobalTransform.Decompress (out T, out R, out S);

            // クォータニオンは指定したのと等価な軸が反対で回転角度[0,180]の回転で返ってくる事がある
            // ここで回転軸(0,0,-1)のものを(0,0,1)に変換する必要がある
            var angle = R.Angle;
            var axis = R.Axis;
            var dot = Vector3.Dot (axis, new Vector3 (0, 0, 1));
            if (dot < 0) {
                angle = 360 - angle;
                axis *= -1;
            }

            var opacity = Node.Upwards.Aggregate (1.0f, (x, node) => x * node.Opacity);
            var col = new Color (color.R, color.G, color.B, (byte)(color.A * opacity));

            var spr1 = new SFML.Graphics.Sprite ();
            spr1.Texture = Resource.GetDefaultTexture ().Data;
            spr1.TextureRect = new SFML.Graphics.IntRect (0, 0, (int)width, (int)lineWidth);
            spr1.Position = new Vector2f (T.X + offset.X, T.Y + offset.Y);
            spr1.Scale = new Vector2f (S.X, S.Y);
            spr1.Rotation = angle;
            spr1.Color = color.ToSFML();

            var spr2 = new SFML.Graphics.Sprite ();
            spr2.Texture = Resource.GetDefaultTexture ().Data;
            spr2.TextureRect = new SFML.Graphics.IntRect (0, 0, (int)width, (int)lineWidth);
            spr2.Origin = new Vector2f (0, -(height - lineWidth));
            spr2.Position = new Vector2f (T.X + offset.X, T.Y + offset.Y);
            spr2.Scale = new Vector2f (S.X, S.Y);
            spr2.Rotation = angle;
            spr2.Color = color.ToSFML ();

            var spr3 = new SFML.Graphics.Sprite ();
            spr3.Texture = Resource.GetDefaultTexture ().Data;
            spr3.TextureRect = new SFML.Graphics.IntRect (0, 0, (int)lineWidth, (int)height);
            spr3.Position = new Vector2f (T.X + offset.X, T.Y + offset.Y);
            spr3.Scale = new Vector2f (S.X, S.Y);
            spr3.Rotation = angle;
            spr3.Color = color.ToSFML ();

            var spr4 = new SFML.Graphics.Sprite ();
            spr4.Texture = Resource.GetDefaultTexture ().Data;
            spr4.TextureRect = new SFML.Graphics.IntRect (0, 0, (int)lineWidth, (int)height);
            spr4.Origin = new Vector2f (-(width - LineWidth), 0);
            spr4.Position = new Vector2f (T.X + offset.X, T.Y + offset.Y);
            spr4.Scale = new Vector2f (S.X, S.Y);
            spr4.Rotation = angle;
            spr4.Color = color.ToSFML ();


            var win = window as RenderWindow;
            win.Draw (spr1);
            win.Draw (spr2);
            win.Draw (spr3);
            win.Draw (spr4);
        }


    }
}
