#include "GameView.hpp"
#include "EventQueue.hpp"
#include <iostream>
using namespace erica;
using namespace std;


GameView:: GameView ()
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

void GameView:: add_game_logic (GameLogic* logic)
{
}

void GameView:: enqueue (const Event* event)
{
}

