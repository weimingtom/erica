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

void GameLogic:: tick (int msec)
{
    in->tick (msec);
    out->tick (msec);

    update (msec);

    for (int i = 0; i < (int)views.size(); i++) {
        views[i]->tick (msec);
    }

}

GameView* GameLogic:: get_view (int id) const
{
    return 0;
}

void GameLogic:: enqueue (const Event* event)
{
}

