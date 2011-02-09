#include "BasicPlayerView.hpp"
#include "Event.hpp"
using namespace erica;
using namespace std;

BasicPlayerView:: BasicPlayerView (GameLogic* logic) : GameView(logic)
{
}

BasicPlayerView:: ~BasicPlayerView ()
{
}

void BasicPlayerView:: update_impl (int msec)
{
    // nothing to do
}

bool BasicPlayerView:: handle_impl (const Event* event)
{
    return false;
}

void BasicPlayerView:: render_impl () const
{
    // nothing to do
}


