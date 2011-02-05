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

void BasicAiView:: update_impl (int msec)
{
}

bool BasicAiView:: handle_impl (const Event* event)
{
    return false;
}

void BasicAiView:: render_impl () const
{
}


