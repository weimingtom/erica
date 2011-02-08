#include "BasicTPSLogic.hpp"
#include "BasicTPSData.hpp"
#include "Event.hpp"
#include <iostream>
using namespace erica;
using namespace std;


BasicTPSLogic:: BasicTPSLogic () : GameLogic(), data(0)
{
}

BasicTPSLogic:: ~BasicTPSLogic ()
{
}


void BasicTPSLogic:: load_game_impl (const char* init_file)
{
}

void BasicTPSLogic:: update_impl (int msec)
{
    // 終了処理とか
}

bool BasicTPSLogic:: handle_impl (const Event* event)
{
    // Event "Spawn"
    // Event "Quit"
    return false;
}

bool BasicTPSLogic:: end_of_game_impl () const
{
    return false;
}

