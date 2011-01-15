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

void BasicPlayerView:: update (int msec)
{
}

bool BasicPlayerView:: handle (const Event* event)
{
    return false;
}

void BasicPlayerView:: render () const
{
}


