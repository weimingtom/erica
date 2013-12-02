using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {

    /// <summary>
    /// 戦闘時の部位選択パネル コンポーネント
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class MyPartsSelector : Component {
        public MyPartsSelector () {

       }

        /// <summary>
        /// 部位選択パネル ノードの作成
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="partsNames">表示する部位の名前</param>
        /// <param name="visible">有効無効の初期状態</param>
        /// <returns></returns>
       public static Node Create (Vector3 pos, string[] partsNames, bool visible) {
           var cmp = new MyPartsSelector ();

           var clip = new SoundClip ();
           clip.AddTrack (new SoundEffectTrack ("Data/共通/PinPon.wav"));

           var node = MyPanel.Create(pos);
           node.Attach (cmp);

           var panel = node.GetComponent<MyPanel>();

           foreach (var partsName in partsNames) {
               var btn = panel.AddButton (MyButton.Create (new Vector3 (0, 0, 0), 170, 30, partsName));
               btn.Text = partsName;
               btn.AddTexture (MyButton.Slot.Normal, new Texture ("Data/共通/部位選択ボタン(標準).png"));
               btn.AddTexture (MyButton.Slot.Focused, new Texture ("Data/共通/部位選択ボタン(フォーカス).png"));
               var name = partsName;
               btn.Clicked += delegate (object sender, EventArgs e) {
                   clip.Play ();
                   panel.Selected = name;
                   panel.Next ();
               };
           }

           var backNode = MyButton.Create (new Vector3 (180, 0, 0), 50, 110, "戻る");
           var back = backNode.GetComponent<MyButton> ();
           back.AddTexture (MyButton.Slot.Normal, new Texture ("Data/共通/戻るボタン(標準).png"));
           back.AddTexture (MyButton.Slot.Focused, new Texture ("Data/共通/戻るボタン(フォーカス).png"));
           back.Clicked += delegate (object sender, EventArgs e) {
               clip.Play ();
               panel.Selected = null;
               panel.Prev ();
           };

           node.AddChild (backNode);
    
           node.Translation = pos;
           node.Visible = visible;
           node.Collidable = visible;


           return node;
       }   
    }
}
