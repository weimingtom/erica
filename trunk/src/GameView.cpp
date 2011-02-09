#include "GameView.hpp"
#include "GameLogic.hpp"
#include "EventQueue.hpp"
#include "Controller.hpp"
#include "Exception.hpp"
#include <iostream>
using namespace erica;
using namespace std;


GameView:: GameView (GameLogic* log) : in(NULL), out(NULL), logic(NULL)
{
    if (log == NULL) {
        throw Exception (__FILE__, __func__, "GameLogic is NULL.");
    }
    logic = log;
    in    = new EventQueue;
    out   = new EventQueue;
}

GameView:: ~GameView ()
{
    delete in;
    delete out;
}



void GameView:: enqueue (const Event* ev)
{
    if (ev == NULL) {
        throw Exception (__FILE__, __func__, "Event is NULL.");
    }

    in->enqueue (ev);
}

void GameView:: add_controller (Controller* ctrl)
{
    if (ctrl == NULL) {
        throw Exception (__FILE__, __func__, "Controller is NULL.");
    }

    ctrl->set_event_queue (in, out);
    
    ctrls.insert (make_pair<int,Controller*>(ctrl->get_actor_id(), ctrl));
}

void GameView:: remove_controller (const Controller* ctrl)
{
    if (ctrl == NULL) {
        throw Exception (__FILE__, __func__, "Contoller is NULL.");
    }

    std::map<int, Controller*>::iterator it;
    for (it = ctrls.begin(); it != ctrls.end(); ) {
        if (it->second == ctrl) {
            ctrls.erase (it++);
        } else {
            it++;
        }
    }
    
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

    // イベント(out)の「ビュー」-->「ロジック」への転送
    while (out->size()) {
        logic->enqueue (out->dequeue());
    }

}

void GameView:: render () const
{
    render_impl ();
}
