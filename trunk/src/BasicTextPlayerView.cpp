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
    // nothing to do
}

bool BasicTextPlayerView:: handle_impl (const Event* ev)
{
    if (strcmp(ev->name(), "SYSTEM") == 0) {
        SystemParams params;
        params.action = ((SystemParams*)ev->params())->action;
        out->enqueue (new Event("SYSTEM", &params, sizeof(params)));
        return true;
    }
    return false;
}

void BasicTextPlayerView:: render_impl () const
{
    // nothing to do
}


