#include "Actor.hpp"
#include "EventQueue.hpp"
using namespace std;
using namespace erica;

Actor::Actor (EventQueue* in, EventQueue* out) : in(NULL), out(NULL)
{
}

Actor:: ~Actor ()
{
}


void Actor:: update (int msec)
{
}

int Actor:: get_actor_id () const
{
    return 0;
}
