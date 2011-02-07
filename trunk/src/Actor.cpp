#include "Actor.hpp"
#include "EventQueue.hpp"
using namespace std;
using namespace erica;

Actor::Actor () : out(NULL)
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

void Actor:: set_actor_id (int id)
{
}

EventQueue* Actor:: get_event_queue (int dir) const
{
    return NULL;
}

void Actor:: set_event_queue (EventQueue* in, EventQueue* out)
{
}
