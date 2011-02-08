#include "BasicTPSData.hpp"
#include <iostream>
using namespace erica;
using namespace std;


BasicTPSData:: BasicTPSData ()
{
    state = RUNNING;
}

BasicTPSData:: ~BasicTPSData ()
{
}


void BasicTPSData:: quit ()
{
    state = END_OF_GAME;
}

bool BasicTPSData:: end_of_game () const
{
    return (state == END_OF_GAME);
}

int BasicTPSData:: get_state () const
{
    return state;
}


