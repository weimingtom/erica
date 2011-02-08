#include "BasicTPSLogic.hpp"
#include "BasicTPSData.hpp"
#include "Event.hpp"
#include <iostream>
#include <cstring>
using namespace erica;
using namespace std;


BasicTPSLogic:: BasicTPSLogic () : GameLogic(), data(NULL)
{
    data = new BasicTPSData;
}

BasicTPSLogic:: ~BasicTPSLogic ()
{
}


void BasicTPSLogic:: load_game_impl (const char* init_file)
{
    // 未実装
}

void BasicTPSLogic:: update_impl (int msec)
{
    // 終了処理とか
}

bool BasicTPSLogic:: handle_impl (const Event* ev)
{
    if (strcmp (ev->name(), "GAME_QUIT") == 0) {
        data->quit ();
        return true;
    }
    // 以下略

    return false;
}

bool BasicTPSLogic:: end_of_game_impl () const
{
    return data->end_of_game();
}

