#include "GameLogic.hpp"
#include "GameView.hpp"
#include "Event.hpp"
#include "EventQueue.hpp"
using namespace erica;
using namespace std;


GameLogic:: GameLogic ()
{
}

GameLogic:: ~GameLogic ()
{
}

void GameLogic:: load_game (const char* ini_file)
{
}

void GameLogic:: update (int msec)
{
    if (in) {
        in->update (msec);
    }
    if (out) {
        out->update (msec);
    }

    update_impl (msec);

    for (int i = 0; i < (int)views.size(); i++) {
        views[i]->update (msec);
    }

}

GameView* GameLogic:: get_game_view (int id) const
{
    return 0;
}

void GameLogic:: enqueue (const Event* event)
{
}

void GameLogic:: add_actor (Actor* actr)
{
}

void GameLogic:: remove_actor (const Actor* actr)
{
}

bool GameLogic:: end_of_game () const
{
    return true;
}

