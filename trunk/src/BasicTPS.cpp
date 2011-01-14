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


void BasicTPS:: create_game (const char* init_file)
{
}

void BasicTPS:: update (int msec)
{
    // 終了処理とか
}

bool BasicTPS:: handle (const Event* event)
{
    // Event "Spawn"
    // Event "Quit"
    return false;
}

