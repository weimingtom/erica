#include "GameLogic.hpp"
#include "GameView.hpp"
#include "Event.hpp"
#include "EventQueue.hpp"
#include <algorithm>
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
    return views[id];
}

void GameLogic:: enqueue (const Event* ev)
{
    in->enqueue (ev);
}

void GameLogic:: add_actor (Actor* actr)
{
    actors.push_back (actr);
}

void GameLogic:: remove_actor (const Actor* actr)
{
//    actors.erase(std::remove(actors.begin(), actors.end(), actr), actors.end());
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

    // ビューの更新.
    for (int i = 0; i < (int)views.size(); i++) {
        views[i]->update (msec);
    }

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

