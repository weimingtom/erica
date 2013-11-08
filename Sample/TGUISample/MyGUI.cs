using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TGUI;

using SFMLVector2 = SFML.Window.Vector2f;
using SFMLColor = SFML.Graphics.Color;

namespace DD.Sample.TGUISample {
    public class MyGUI : Component {

        public MyGUI () {
        }

        public static Node Create () {
            var cmp = new MyGUI ();

            var node = new Node ("GUI");
            node.Attach (cmp);

            return node;

            
        }

        public override void OnUpdateInit (long msec) {

            var bg = GUI.Add (new TGUI.Picture ("TGUISample/widgets/OPTION画面.png"));

            // ---------------------------------
            // テキスト設定 パネル
            // ---------------------------------
            var テキスト設定 = GUI.Add (new TGUI.Button ("TGUISample/widgets/テキスト設定.txt"));
            テキスト設定.Position = new SFMLVector2 (20, 24);
            テキスト設定.Size = new SFMLVector2 (140, 38);
            var panel1 = GUI.Add (new TGUI.Panel ());
            panel1.Position = new SFMLVector2 (12, 50);
            panel1.Size = new SFMLVector2 (300, 430);
            {
                // テキスト表示速度
                var textSpeed = panel1.Add (new TGUI.Button ("TGUISample/widgets/テキスト表示速度.txt"));
                textSpeed.Position = new SFMLVector2 (10, 10);

                // テキスト表示速度スライダー
                var textSlider = panel1.Add (new TGUI.Slider ("TGUISample/widgets/テキスト表示速度スライダー.txt"));
                textSlider.Position = new SFMLVector2 (60, 58);
                textSlider.Value = 5;
                {
                    var slow = panel1.Add (new TGUI.Button ("TGUISample/widgets/Slow.txt"));
                    slow.Position = new SFMLVector2 (10, 48);
                    var fast = panel1.Add (new TGUI.Button ("TGUISample/widgets/Fast.txt"));
                    fast.Position = new SFMLVector2 (220, 48);
                }

                // 区切り線
                var line1 = panel1.Add (new TGUI.Button ("TGUISample/widgets/区切り線.txt"));
                line1.Position = new SFMLVector2 (20, 100);

                // SKIPスピード
                var skipSpeed = panel1.Add (new TGUI.Button ("TGUISample/widgets/SKIPスピード.txt"));
                skipSpeed.Position = new SFMLVector2 (10, 120);

                var skipSpeedPanel = panel1.Add (new TGUI.Panel ());
                skipSpeedPanel.Position = new SFMLVector2 (160, 120);
                skipSpeedPanel.Size = new SFMLVector2 (200, 38);
                {
                    var normal = skipSpeedPanel.Add (new TGUI.RadioButton ("TGUISample/widgets/通常.txt"));
                    normal.Position = new SFMLVector2 (0, 0);
                    normal.Check ();
                    var fastest = skipSpeedPanel.Add (new TGUI.RadioButton ("TGUISample/widgets/最速.txt"));
                    fastest.Position = new SFMLVector2 (60, 0);
                }

                // SKIPテキスト
                var skipText = panel1.Add (new TGUI.Button ("TGUISample/widgets/SKIPテキスト.txt"));
                skipText.Position = new SFMLVector2 (10, 160);

                var skipTextPanel = panel1.Add (new TGUI.Panel ());
                skipTextPanel.Position = new SFMLVector2 (160, 160);
                skipTextPanel.Size = new SFMLVector2 (200, 38);
                {
                    var read = skipTextPanel.Add (new TGUI.RadioButton ("TGUISample/widgets/既読.txt"));
                    read.Position = new SFMLVector2 (0, 0);
                    read.Check ();
                    var unread = skipTextPanel.Add (new TGUI.RadioButton ("TGUISample/widgets/未読.txt"));
                    unread.Position = new SFMLVector2 (60, 0);
                }

                // SKIP選択肢後
                var skipAfter = panel1.Add (new TGUI.Button ("TGUISample/widgets/SKIP選択肢後.txt"));
                skipAfter.Position = new SFMLVector2 (10, 200);

                var skipAfterPanel = panel1.Add (new TGUI.Panel ());
                skipAfterPanel.Position = new SFMLVector2 (160, 200);
                skipAfterPanel.Size = new SFMLVector2 (200, 38);
                {
                    var stop = skipAfterPanel.Add (new TGUI.RadioButton ("TGUISample/widgets/停止.txt"));
                    stop.Position = new SFMLVector2 (0, 0);
                    stop.Check ();
                    var cont = skipAfterPanel.Add (new TGUI.RadioButton ("TGUISample/widgets/継続.txt"));
                    cont.Position = new SFMLVector2 (60, 0);
                }

                // 区切り線
                var line2 = panel1.Add (new TGUI.Button ("TGUISample/widgets/区切り線.txt"));
                line2.Position = new SFMLVector2 (20, 245);


                // AUTO表示速度
                var autoSpeed = panel1.Add (new TGUI.Button ("TGUISample/widgets/AUTO表示速度.txt"));
                autoSpeed.Position = new SFMLVector2 (10, 260);

                // AUTO表示速度スライダー
                var autoSlider = panel1.Add (new TGUI.Slider ("TGUISample/widgets/AUTO表示速度スライダー.txt"));
                autoSlider.Position = new SFMLVector2 (60, 308);
                autoSlider.Value = 5;

                var autoSlow = panel1.Add (new TGUI.Button ("TGUISample/widgets/Slow.txt"));
                autoSlow.Position = new SFMLVector2 (10, 298);

                var autoFast = panel1.Add (new TGUI.Button ("TGUISample/widgets/Fast.txt"));
                autoFast.Position = new SFMLVector2 (220, 298);

                // AUTO選択肢後
                var autoAfter = panel1.Add (new TGUI.Button ("TGUISample/widgets/AUTO選択肢後.txt"));
                autoAfter.Position = new SFMLVector2 (10, 336);

                var autoAfterPanel = panel1.Add (new TGUI.Panel ());
                autoAfterPanel.Position = new SFMLVector2 (160, 336);
                autoAfterPanel.Size = new SFMLVector2 (200, 38);
                {
                    var stop = autoAfterPanel.Add (new TGUI.RadioButton ("TGUISample/widgets/停止.txt"));
                    stop.Position = new SFMLVector2 (0, 0);
                    stop.Check ();

                    var cont = autoAfterPanel.Add (new TGUI.RadioButton ("TGUISample/widgets/継続.txt"));
                    cont.Position = new SFMLVector2 (60, 0);
                }

                // AUTOメッセージウィンドウ
                var autoWindow = panel1.Add (new TGUI.Button ("TGUISample/widgets/AUTOメッセージウィンドウ.txt"));
                autoWindow.Position = new SFMLVector2 (10, 374);

                var autoWindowPanel = panel1.Add (new TGUI.Panel ());
                autoWindowPanel.Position = new SFMLVector2 (160, 374);
                autoWindowPanel.Size = new SFMLVector2 (200, 38);
                {
                    var stop = autoWindowPanel.Add (new TGUI.RadioButton ("TGUISample/widgets/そのまま.txt"));
                    stop.Position = new SFMLVector2 (0, 0);
                    stop.Check ();
                    var cont = autoWindowPanel.Add (new TGUI.RadioButton ("TGUISample/widgets/変更.txt"));
                    cont.Position = new SFMLVector2 (60, 0);
                }

                // ---------------------------------
                // カーソル設定 パネル
                // ---------------------------------
                var カーソル設定 = GUI.Add (new TGUI.Button ("TGUISample/widgets/カーソル設定.txt"));
                カーソル設定.Position = new SFMLVector2 (20, 490);
                カーソル設定.Size = new SFMLVector2 (140, 38);
                var panel2 = GUI.Add (new TGUI.Panel ());
                panel2.Position = new SFMLVector2 (12, 524);
                panel2.Size = new SFMLVector2 (300, 50);

                // カーソル自動追尾
                var cursolTracking = panel2.Add (new TGUI.Button ("TGUISample/widgets/カーソル自動追尾.txt"));
                cursolTracking.Position = new SFMLVector2 (10, 0);

                var cursolTrackingPanel = panel2.Add (new TGUI.Panel ());
                cursolTrackingPanel.Position = new SFMLVector2 (180, 0);
                cursolTrackingPanel.Size = new SFMLVector2 (200, 38);
                {
                    var off = cursolTrackingPanel.Add (new TGUI.RadioButton ("TGUISample/widgets/OFF.txt"));
                    off.Position = new SFMLVector2 (0, 0);
                    off.Check ();
                    var on = cursolTrackingPanel.Add (new TGUI.RadioButton ("TGUISample/widgets/ON.txt"));
                    on.Position = new SFMLVector2 (50, 0);
                }

                // ---------------------------------
                // 画面設定 パネル
                // ---------------------------------
                var 画面設定 = GUI.Add (new TGUI.Button ("TGUISample/widgets/画面設定.txt"));
                画面設定.Position = new SFMLVector2 (340, 20);
                画面設定.Size = new SFMLVector2 (140, 38);

                var panel3 = GUI.Add (new TGUI.Panel ());
                panel3.Position = new SFMLVector2 (330, 56);
                panel3.Size = new SFMLVector2 (300, 140);

                // 画面解像度
                var resol = panel3.Add (new TGUI.Button ("TGUISample/widgets/画面解像度.txt"));
                resol.Position = new SFMLVector2 (0, 0);

                // 区切り線
                var line3 = panel3.Add (new TGUI.Button ("TGUISample/widgets/区切り線.txt"));
                line3.Position = new SFMLVector2 (20, 45);

                var resolPanel = panel3.Add (new TGUI.Panel ());
                resolPanel.Position = new SFMLVector2 (160, 0);
                resolPanel.Size = new SFMLVector2 (200, 38);
                {
                    var full = resolPanel.Add (new RadioButton ("TGUISample/widgets/Full.txt"));
                    full.Position = new SFMLVector2 (0, 0);
                    full.Check ();
                    var win = resolPanel.Add (new RadioButton ("TGUISample/widgets/Win.txt"));
                    win.Position = new SFMLVector2 (50, 0);
                }

                // ADV画面エフェクト
                var advEffect = panel3.Add (new TGUI.Button ("TGUISample/widgets/ADV画面エフェクト.txt"));
                advEffect.Position = new SFMLVector2 (0, 60);

                var advEffectPanel = panel3.Add (new TGUI.Panel ());
                advEffectPanel.Position = new SFMLVector2 (160, 60);
                advEffectPanel.Size = new SFMLVector2 (200, 38);
                {
                    var off = advEffectPanel.Add (new RadioButton ("TGUISample/widgets/Off.txt"));
                    off.Position = new SFMLVector2 (0, 0);
                    off.Check ();
                    var on = advEffectPanel.Add (new RadioButton ("TGUISample/widgets/On.txt"));
                    on.Position = new SFMLVector2 (50, 0);
                }

                // バトル画面エフェクト
                var btlEffect = panel3.Add (new TGUI.Button ("TGUISample/widgets/バトル画面エフェクト.txt"));
                btlEffect.Position = new SFMLVector2 (0, 98);

                var btlEffectPanel = panel3.Add (new TGUI.Panel ());
                btlEffectPanel.Position = new SFMLVector2 (160, 98);
                btlEffectPanel.Size = new SFMLVector2 (200, 38);
                {
                    var off = btlEffectPanel.Add (new RadioButton ("TGUISample/widgets/Off.txt"));
                    off.Position = new SFMLVector2 (0, 0);
                    off.Check ();
                    var on = btlEffectPanel.Add (new RadioButton ("TGUISample/widgets/On.txt"));
                    on.Position = new SFMLVector2 (50, 0);
                }

                // ---------------------------------
                // 右クリック設定 パネル
                // ---------------------------------
                var 右クリック設定 = GUI.Add (new TGUI.Button ("TGUISample/widgets/右クリック設定.txt"));
                右クリック設定.Position = new SFMLVector2 (340, 200);
                右クリック設定.Size = new SFMLVector2 (140, 38);

                var panel4 = GUI.Add (new TGUI.Panel ());
                panel4.Position = new SFMLVector2 (330, 230);
                panel4.Size = new SFMLVector2 (300, 140);

                // MENU
                var menu = panel4.Add (new TGUI.RadioButton ("TGUISample/widgets/MENU.txt"));
                menu.Position = new SFMLVector2 (0, 0);
                menu.Check ();

                // STATUS
                var status = panel4.Add (new TGUI.RadioButton ("TGUISample/widgets/STATUS.txt"));
                status.Position = new SFMLVector2 (70, 0);

                // BACKLOG
                var backlog = panel4.Add (new TGUI.RadioButton ("TGUISample/widgets/BACKLOG.txt"));
                backlog.Position = new SFMLVector2 (165, 0);

                // AUTO
                var auto = panel4.Add (new TGUI.RadioButton ("TGUISample/widgets/AUTO.txt"));
                auto.Position = new SFMLVector2 (0, 40);

                // SKIP
                var skip = panel4.Add (new TGUI.RadioButton ("TGUISample/widgets/SKIP.txt"));
                skip.Position = new SFMLVector2 (65, 40);

                // DELETE
                var delete = panel4.Add (new TGUI.RadioButton ("TGUISample/widgets/メッセージウィンドウ消去.txt"));
                delete.Position = new SFMLVector2 (130, 40);

                // ---------------------------------
                // サウンド設定 パネル
                // ---------------------------------
                var サウンド設定 = GUI.Add (new TGUI.Button ("TGUISample/widgets/サウンド設定.txt"));
                サウンド設定.Position = new SFMLVector2 (340, 320);
                サウンド設定.Size = new SFMLVector2 (140, 38);

                var panel5 = GUI.Add (new TGUI.Panel ());
                panel5.Position = new SFMLVector2 (330, 350);
                panel5.Size = new SFMLVector2 (300, 220);

                // BGM
                var bgm = panel5.Add (new TGUI.Button ("TGUISample/widgets/BGM.txt"));
                bgm.Position = new SFMLVector2 (0, 0);

                var bgmPanel = panel5.Add (new TGUI.Panel ());
                bgmPanel.Position = new SFMLVector2 (180, 0);
                bgmPanel.Size = new SFMLVector2 (100, 30);
                {
                    var off = bgmPanel.Add (new RadioButton ("TGUISample/widgets/Off(小).txt"));
                    off.Position = new SFMLVector2 (0, 10);
                    off.Check ();
                    var on = bgmPanel.Add (new RadioButton ("TGUISample/widgets/On(小).txt"));
                    on.Position = new SFMLVector2 (50, 10);
                }

                // BGMスライダー
                var bgmSlider = panel5.Add (new TGUI.Slider ("TGUISample/widgets/BGMスライダー.txt"));
                bgmSlider.Position = new SFMLVector2 (60, 48);
                bgmSlider.Value = 5;

                var bgmMin = panel5.Add (new TGUI.Button ("TGUISample/widgets/Min.txt"));
                bgmMin.Position = new SFMLVector2 (10, 38);

                var bgmMax = panel5.Add (new TGUI.Button ("TGUISample/widgets/Max.txt"));
                bgmMax.Position = new SFMLVector2 (220, 38);

                // AMBIENT SOUND
                var ambSound = panel5.Add (new TGUI.Button ("TGUISample/widgets/AMBIENT SOUND.txt"));
                ambSound.Position = new SFMLVector2 (0, 70);

                var ambSoundPanel = panel5.Add (new TGUI.Panel ());
                ambSoundPanel.Position = new SFMLVector2 (180, 70);
                ambSoundPanel.Size = new SFMLVector2 (100, 30);
                {
                    var off = ambSoundPanel.Add (new RadioButton ("TGUISample/widgets/Off(小).txt"));
                    off.Position = new SFMLVector2 (0, 10);
                    off.Check ();
                    var on = ambSoundPanel.Add (new RadioButton ("TGUISample/widgets/On(小).txt"));
                    on.Position = new SFMLVector2 (50, 10);
                }

                // AMBIENT SOUNDスライダー
                var ambSoundSlider = panel5.Add (new TGUI.Slider ("TGUISample/widgets/AMBIENT SOUNDスライダー.txt"));
                ambSoundSlider.Position = new SFMLVector2 (60, 118);
                ambSoundSlider.Value = 5;

                var ambSoundMin = panel5.Add (new TGUI.Button ("TGUISample/widgets/Min.txt"));
                ambSoundMin.Position = new SFMLVector2 (10, 108);

                var ambSoundMax = panel5.Add (new TGUI.Button ("TGUISample/widgets/Max.txt"));
                ambSoundMax.Position = new SFMLVector2 (220, 108);

                // SOUND EFFECT
                var sndEffect = panel5.Add (new TGUI.Button ("TGUISample/widgets/SOUND EFFECT.txt"));
                sndEffect.Position = new SFMLVector2 (0, 140);

                var sndEffectPanel = panel5.Add (new TGUI.Panel ());
                sndEffectPanel.Position = new SFMLVector2 (180, 140);
                sndEffectPanel.Size = new SFMLVector2 (100, 30);
                {
                    var off = sndEffectPanel.Add (new RadioButton ("TGUISample/widgets/Off(小).txt"));
                    off.Position = new SFMLVector2 (0, 10);
                    off.Check ();
                    var on = sndEffectPanel.Add (new RadioButton ("TGUISample/widgets/On(小).txt"));
                    on.Position = new SFMLVector2 (50, 10);
                }

                // SOUND EFFECTスライダー
                var sndEffectSlider = panel5.Add (new TGUI.Slider ("TGUISample/widgets/SOUND EFFECTスライダー.txt"));
                sndEffectSlider.Position = new SFMLVector2 (60, 190);
                sndEffectSlider.Value = 5;

                var sndEffectMin = panel5.Add (new TGUI.Button ("TGUISample/widgets/Min.txt"));
                sndEffectMin.Position = new SFMLVector2 (10, 180);

                var sndEffectMax = panel5.Add (new TGUI.Button ("TGUISample/widgets/Max.txt"));
                sndEffectMax.Position = new SFMLVector2 (220, 180);

                // 音声設定
                var voice = GUI.Add (new TGUI.Button ("TGUISample/widgets/音声設定.txt"));
                voice.Position = new SFMLVector2 (450, 550);

                // --------------------------
                // 右パネル
                // --------------------------

                var panel6 = GUI.Add (new Panel ());
                panel6.Position = new SFMLVector2 (640, 440);
                panel6.Size = new SFMLVector2 (160, 160);
       
                // デフォルトに戻す
                var defalt = panel6.Add (new TGUI.Button ("TGUISample/widgets/デフォルトに戻す.txt"));
                defalt.Position = new SFMLVector2 (10, 0);

                // タイトルに戻る
                var title = panel6.Add (new TGUI.Button ("TGUISample/widgets/タイトルに戻る.txt"));
                title.Position = new SFMLVector2 (10, 30);

                // ゲームを終了する
                var end = panel6.Add (new TGUI.Button ("TGUISample/widgets/ゲームを終了する.txt"));
                end.Position = new SFMLVector2 (10, 60);

               // 戻る
                var back = panel6.Add (new TGUI.Button ("TGUISample/widgets/戻る.txt"));
                back.Position = new SFMLVector2 (40, 85);

            }

        }
    }
}
