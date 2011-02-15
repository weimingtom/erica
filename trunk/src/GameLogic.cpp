#include "GameLogic.hpp"
#include "GameView.hpp"
#include "Event.hpp"
#include "Actor.hpp"
#include "EventQueue.hpp"
#include "Exception.hpp"
#include <algorithm>
#include <iostream>
using namespace erica;
using namespace std;


GameLogic:: GameLogic () : in(NULL), out(NULL)
{
    in  = new EventQueue;
    out = new EventQueue;
}

GameLogic:: ~GameLogic ()
{
    delete in;
    delete out;
}


GameView* GameLogic:: get_game_view (int id) const
{
    if (id < 0 || id >= (int)views.size()) {
        throw Exception (__FILE__, __func__, "View ID is invalid, id=%d.\n", id);
    }
    return views[id];
}

void GameLogic:: enqueue (const Event* ev)
{
    if (ev == NULL) {
        throw Exception (__FILE__, __func__, "Event is NULL.");
    }
    in->enqueue (ev);
}

void GameLogic:: add_view (GameView* view)
{
    if (view == NULL) {
        throw Exception (__FILE__, __func__, "View is NULL.");
    }

    views.push_back  (view);
}

void GameLogic:: add_actor (Actor* actr)
{
    if (actr == NULL) {
        throw Exception (__FILE__, __func__, "Actor is NULL.");
    }

    actr->set_event_queue (in, out);

    actors.push_back (actr);
}

void GameLogic:: remove_view (const GameView* view)
{
    if (view == NULL) {
        throw Exception (__FILE__, __func__, "View is NULL.");
    }

    views.erase(remove(views.begin(), views.end(), view), views.end());
}


void GameLogic:: remove_actor (const Actor* actr)
{
    if (actr == NULL) {
        throw Exception (__FILE__, __func__, "Actor is NULL.");
    }

    actors.erase(remove(actors.begin(), actors.end(), actr), actors.end());
}


/**
 * ここ以下はNVIの実装関数を呼び出す関数。
 */

void GameLogic:: load_game (const char* ini_file)
{
    load_game_impl (ini_file);
}



void GameLogic:: update (int msec)
{
    // イベントの処理.
    in->trigger ();

    // ロジックの更新.
    update_impl (msec);
    
    // TODO: 再度検討
    //  「ロジック」から「ビュー」を更新するのは正しいのか？

    // ビューの更新.
    //for (int i = 0; i < (int)views.size(); i++) {
    //    views[i]->update (msec);
    //}

    // イベントの「ロジック」-->「ビュー」への転送
    while (out->size()) {
        const Event* ev = out->dequeue();
        for (int i = 0; i < (int)views.size(); i++) {
            // TODO：手抜き実装
            // このイベントは複数のビューから参照されるので
            // deleteしたら落ちる。
            views[i]->enqueue (ev);
        }
    }

}

bool GameLogic:: end_of_game () const
{
    return end_of_game_impl ();
}

std::ostream& GameLogic:: print (std::ostream& ou) const
{
    ou << "GameLogic: \n";
    ou << "  in.size = " << in->size() << "\n";
    ou << "  out.size = " << out->size() << "\n";
    ou << "  views.size = " << views.size() << "\n";
    ou << "  actors.size = " << actors.size() << "\n";
    return ou;
}

void GameLogic:: update_impl (int msec)
{

}

void GameLogic:: load_game_impl (const char* ini_file)
{
}

bool GameLogic:: end_of_game_impl () const
{
    return false;
}


ostream& operator<< (ostream& out, const GameLogic& logic)
{
    return logic.print (out);
}
