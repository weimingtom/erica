#include "GameView.hpp"
#include "EventQueue.hpp"
#include "Controller.hpp"
#include <iostream>
using namespace erica;
using namespace std;


GameView:: GameView (GameLogic* logic) : in(NULL), out(NULL), logic(NULL)
{
    in  = new EventQueue;
    out = new EventQueue;
}

GameView:: ~GameView ()
{
}

void GameView:: update (int msec)
{
    if (in) {
        in->update (msec);
    }
    if (out) {
        out->update (msec);
    }

    update_impl (msec);
}


void GameView:: enqueue (const Event* event)
{

}

void GameView:: add_controller (Controller* ctrl)
{
    ctrl->set_event_queue (in, out);
}

void GameView:: remove_controller (const Controller* ctrl)
{
}


