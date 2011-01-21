#include "Actor.hpp"
#include "EventQueue.hpp"
using namespace std;
using namespace erica;

Actor::Actor (EventQueue* in, EventQueue* out) : out(0)
{
}

Actor:: ~Actor ()
{
}

void Actor:: tick (int msec)
{
}

