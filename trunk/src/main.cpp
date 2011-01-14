#include "BasicTPS.hpp"
#include "BasicPlayerView.hpp"
#include <iostream>
using namespace erica;
using namespace std;
const char* GAME_INIT_FILE = "Game.ini";

int main (int argc, char** argv) 
{
    GameLogic* game   = new BasicTPS ();
    game->load_game (GAME_INIT_FILE);

    GameView* player = game->get_view (0);
    player = 0;

    int msec = 0;
    while (1) {
        game->tick (msec);
        msec += 16;
    }

    return 0;
}

