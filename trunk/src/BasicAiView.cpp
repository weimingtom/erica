#include "BasicAiView.hpp"
#include "Event.hpp"
using namespace erica;
using namespace std;

BasicAiView:: BasicAiView (GameLogic* logic) : GameView(logic)
{
}

BasicAiView:: ~BasicAiView ()
{
}

void BasicAiView:: update (int msec)
{
}

bool BasicAiView:: handle (const Event* event)
{
    return false;
}

void BasicAiView:: render () const
{
}


