using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {
    
    /// <summary>
    /// 攻撃ターゲット選択パネル コンポーネント
    /// </summary>
    /// <remarks>
    /// 戦闘時の攻撃ターゲットを選択するパネル クラス。
    /// <see cref="Panel"/> クラスを利用します。
    /// </remarks>
    public class MyTargetSelector : Component {
  
        public MyTargetSelector (){
        }

        /// <summary>
        /// 攻撃ターゲット選択パネルの作成
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="targetNames">表示するターゲットの名前</param>
        /// <param name="visible">有効無効の初期状態</param>
        /// <returns></returns>
        public static Node Create (Vector3 pos, string[] targetNames, bool visible) {
            var cmp = new MyTargetSelector ();

            var clip = new SoundClip ();
            clip.AddTrack (new SoundEffectTrack ("Data/共通/PinPon.wav"));

            var node = MyPanel.Create (pos);
            node.Attach (cmp);

            var panel = node.GetComponent<MyPanel> ();

            foreach (var targetName in targetNames) {
                var btn = panel.AddButton (MyButton.Create (new Vector3 (0, 0, 0), 170, 30, targetName));
                btn.Text = targetName;
                btn.AddTexture (MyButton.Slot.Normal, new Texture ("Data/共通/対象選択ボタン(標準).png"));
                btn.AddTexture (MyButton.Slot.Focused, new Texture ("Data/共通/対象選択ボタン(フォーカス).png"));
                var name = targetName;
                btn.Clicked += delegate (object sender, EventArgs e) {
                    clip.Play ();
                    panel.Selected = name;
                    panel.Next ();
                };
            }

            node.Translation = pos;
            node.Visible = visible;
            node.Collidable = visible;

            return node;
        }



    }
}
