#include "Actor.hpp"
#include "ActorIDManager.hpp"
#include "EventQueue.hpp"
#include "Definitions.hpp"
using namespace std;
using namespace erica;



Actor::Actor () : in(NULL), out(NULL), actor_id(0), name(NULL)
{
    actor_id = ActorIDManager:: get_unique_actor_id ();
}

Actor:: ~Actor ()
{
    ActorIDManager:: release_unique_actor_id (actor_id);
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
