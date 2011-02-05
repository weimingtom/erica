#include "BasicTPS.hpp"
#include "BasicTPSData.hpp"
#include "Event.hpp"
#include <iostream>
using namespace erica;
using namespace std;


BasicTPS:: BasicTPS () : GameLogic(), data(0)
{
}

BasicTPS:: ~BasicTPS ()
{
}


void BasicTPS:: load_game_impl (const char* init_file)
{
}

void BasicTPS:: update_impl (int msec)
{
    // 終了処理とか
}

bool BasicTPS:: handle_impl (const Event* event)
{
    // Event "Spawn"
    // Event "Quit"
    return false;
}

