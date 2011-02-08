#include "GameView.hpp"
#include "GameLogic.hpp"
#include "EventQueue.hpp"
#include "Controller.hpp"
#include <iostream>
using namespace erica;
using namespace std;


GameView:: GameView (GameLogic* logic) : in(NULL), out(NULL), logic(NULL)
{
    in  = new EventQueue;
    out = new EventQueue;
}

GameView:: ~GameView ()
{
}



void GameView:: enqueue (const Event* ev)
{
    in->enqueue (ev);
}

void GameView:: add_controller (Controller* ctrl)
{
    ctrl->set_event_queue (in, out);
    
    ctrls.insert (make_pair<int,Controller*>(ctrl->get_actor_id(), ctrl));
}

void GameView:: remove_controller (const Controller* ctrl)
{
    // 
}

/**
 * これ以下はNVI化した関数を呼び出す関数.
 */

void GameView:: update (int msec)
{
    // イベント(in)の処理
    in->trigger ();

    // ビューの更新
    update_impl (msec);

    
    // コントローラーの定期更新はなし
    //for (int i = 0; i < (int)ctrls.size(); i++) {
    //    ctrls[i]->update (msec);
    //}

    // イベントの「ビュー」-->「ロジック」への転送
    while (out->size()) {
        logic->enqueue (out->dequeue());
    }

}

void GameView:: render () const
{
    render_impl ();
}
