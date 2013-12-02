using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {

    /// <summary>
    /// 味方キャラクター ステータス（1人）表示パネル コンポーネント
    /// </summary>
    /// <remarks>
    /// 画面の左側に表示される味方キャラクター ステータスを1人分表示します。
    /// 表示されるのは 顔グラフィック + HP/MP（数字、バー）です。
    /// </remarks>
    public class MyCharactreStatusPanel : Component {
        MyCharacter ch;
        string uniqueName;

        public MyCharactreStatusPanel (string uniqueName) {
            this.uniqueName = uniqueName;
        }

        /// <summary>
        /// キャラクター ステータス表示パネル ノードの作成
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="ch">キャラクター</param>
        /// <returns></returns>
        public static Node Create (Vector3 pos, MyCharacter ch) {
            var cmp = new MyCharactreStatusPanel (ch.Name);

            // 顔グラ
            var spr = new Sprite ();
            spr.AddTexture (ch.GetTexture ("戦闘", "ステータス", "顔", "標準顔"));

            // クリック領域
            var col = new CollisionObject ();
            col.Shape = new BoxShape (200 / 2, 52 / 2, 10);   // あってる？
            col.SetOffset (200 / 2, 52/2, 0);

            // アニメーション コントローラー
            var anim = new AnimationController ();

            // メールボックス
            // （メモ）
            // 自分以外のキャラクターステータスの
            // 表示/非表示はメッセージ通信で行う
            var mbox1 = new MailBox ("HideCharacterStatus");
            var mbox2 = new MailBox ("RollbackCharacterStatus");

            var node = new Node ();
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (anim);
            node.Attach (mbox1);
            node.Attach (mbox2);

            var bar = MyHpBar.Create (new Vector3 (40, 13, 0));
            var hp = MyNumber.Create (new Vector3 (140, 6, 0), 3, MyNumber.Type.Yellow);
            var mp = MyNumber.Create (new Vector3 (140, 27, 0), 3, MyNumber.Type.Green);
            node.AddChild (bar);
            node.AddChild (hp);
            node.AddChild (mp);

            node.AddUserData ("HP/MP Bar", bar.GetComponent<MyHpBar> ());
            node.AddUserData ("HP", hp.GetComponent<MyNumber> ());
            node.AddUserData ("MP", mp.GetComponent<MyNumber> ());

            // アニメーション データ
            var focusClip = new AnimationClip (300, "FocusAnimation");
            {
                var track = new AnimationTrack ("Translation", InterpolationType.Linear);
                track.AddKeyframe (0, new Vector3 (pos.X, pos.Y, pos.Z));
                track.AddKeyframe (300, new Vector3 (pos.X, 0, pos.Z));
                focusClip.WrapMode = WrapMode.Once;
                focusClip.AddTrack (node, track);
            }
            var fadeClip = new AnimationClip (300, "FadeAnimation");
            {
                var track = new AnimationTrack ("Translation", InterpolationType.Linear);
                track.AddKeyframe (0, new Vector3 (pos.X, pos.Y, pos.Z));
                track.AddKeyframe (300, new Vector3 (pos.X - 200, pos.Y, pos.Z));
                fadeClip.WrapMode = WrapMode.Once;
                fadeClip.AddTrack (node, track);
            }
            anim.AddClip (focusClip);
            anim.AddClip (fadeClip);

            node.Translation = pos;

            return node;
        }

        public override void OnUpdateInit (long msec) {
            this.ch = World.Find (uniqueName).GetComponent<MyCharacter> ();

            var bar = GetUserData<MyHpBar> ("HP/MP Bar");
            bar.MaxHp = ch.MaxHp;
            bar.MaxMp = ch.MaxMp;

        }

        public override void OnUpdate (long msec) {

            var bar = GetUserData<MyHpBar> ("HP/MP Bar");
            bar.Hp = ch.Hp;
            bar.Mp = ch.Mp;

            var hp = GetUserData<MyNumber> ("HP");
            hp.Value = (int)ch.Hp;

            var mp = GetUserData<MyNumber> ("MP");
            mp.Value = (int)ch.Mp;
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            switch (button) {
                case MouseButton.Left: {
                        Animation["FocusAnimation"].SetPlaybackPoisition (0, Time.CurrentTime);
                        Animation["FocusAnimation"].SetSpeed (1, Time.CurrentTime);
                        Animation["FocusAnimation"].Play ();
                        Animation["FadeAnimation"].Stop ();
                        SendMessage ("HideCharacterStatus", this);
                        break;
                    }
                case MouseButton.Right: {
                    SendMessage("RollbackCharacterStatus", null);
                        break;
                    }
            }
        }

        public override void OnMailBox (Node from, string address, object letter) {
            switch (address) {
                case "HideCharacterStatus": {
                        if (letter != this) {
                            Animation["FadeAnimation"].SetPlaybackPoisition (0, Time.CurrentTime);
                            Animation["FadeAnimation"].SetSpeed (1, Time.CurrentTime);
                            Animation["FadeAnimation"].Play ();
                            Animation["FocusAnimation"].Stop ();
                        }
                        break;
                    }
                case "RollbackCharacterStatus": {
                        var focus = Animation["FocusAnimation"];
                        if (focus.IsPlaying) {
                            focus.SetPlaybackPoisition (focus.Duration, Time.CurrentTime);
                            focus.SetSpeed (-1, Time.CurrentTime);
                        }
                        var fadeout = Animation["FadeAnimation"];
                        if (fadeout.IsPlaying) {
                            fadeout.SetPlaybackPoisition (fadeout.Duration, Time.CurrentTime);
                            fadeout.SetSpeed (-1, Time.CurrentTime);
                        }
                        break;
                    }
            }
        }

    }
}
