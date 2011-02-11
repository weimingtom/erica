#include "Controller.hpp"
#include "EventQueue.hpp"
#include "Definitions.hpp"
#include "Exception.hpp"
#include <iostream>
using namespace erica;
using namespace std;

Controller:: Controller () : in(NULL), out(NULL), actor_id(0)
{
}

Controller:: ~Controller ()
{
}

void Controller:: set_event_queue (EventQueue* i, EventQueue* o)
{
    in  = i;
    out = o;
    set_event_listener_impl ();
}

int Controller:: get_actor_id () const
{
    return actor_id;
}

void Controller:: set_actor_id (int id)
{
    if (id < ACTOR_ID_USER_MIN || id >= ACTOR_ID_USER_MAX) {
        throw Exception (__FILE__, __func__, "ActorID is invalid. id=%d\n", id);
    }

    actor_id = id;
}
