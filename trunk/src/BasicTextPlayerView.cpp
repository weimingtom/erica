#include "BasicTextPlayerView.hpp"
#include "BasicEventParams.hpp"
#include "EventQueue.hpp"
#include "Event.hpp"
#include <cstring>
using namespace erica;
using namespace std;


BasicTextPlayerView:: BasicTextPlayerView (GameLogic* logic) : GameView(logic)
{
}

BasicTextPlayerView:: ~BasicTextPlayerView ()
{
}

void BasicTextPlayerView:: update_impl (int msec)
{
    //
    
}

bool BasicTextPlayerView:: handle_impl (const Event* ev)
{
    if (strcmp(ev->name(), "KEY_PRESSED") == 0) {
        char key = ((KeyPressedParams*)ev->params())->key;
        ActorWalkParams params;
        switch (key) {
        case 4: params.dir = ActorWalkParams::LEFT; break;
        case 6: params.dir = ActorWalkParams::RIGHT; break;
        case 2: params.dir = ActorWalkParams::FORWARD; break;
        case 8: params.dir = ActorWalkParams::BACK; break;
        }
        out->enqueue (new Event("WALK", &params, sizeof(params)));
        return true;
    }
    return false;
}

void BasicTextPlayerView:: render_impl () const
{
    //
}


