#include "Actor.hpp"
#include "UniqueID.hpp"
#include "EventQueue.hpp"
#include "Definitions.hpp"
using namespace std;
using namespace erica;

UniqueID unique_id (ACTOR_ID_AUTO_MIN, ACTOR_ID_AUTO_MAX);

Actor::Actor () : in(NULL), out(NULL), actor_id(0), name(NULL)
{
    actor_id = unique_id.get ();
}

Actor:: ~Actor ()
{
    unique_id.release (actor_id);
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

void Actor:: set_event_queue (EventQueue* i, EventQueue* o)
{
    in  = i;
    out = o;
    set_event_listener_impl ();
}
