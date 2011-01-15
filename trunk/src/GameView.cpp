#include "GameView.hpp"
#include "EventQueue.hpp"
#include <iostream>
using namespace erica;
using namespace std;


GameView:: GameView (GameLogic* logic) : logic(0)
{
}

GameView:: ~GameView ()
{
}

void GameView:: tick (int msec)
{
    in->tick (msec);
    out->tick (msec);

    update (msec);
}


void GameView:: enqueue (const Event* event)
{
}

