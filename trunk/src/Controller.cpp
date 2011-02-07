#include "Controller.hpp"
#include "EventQueue.hpp"
#include <iostream>
using namespace erica;
using namespace std;

Controller:: Controller () : out(NULL)
{
}

Controller:: ~Controller ()
{
}

void Controller:: set_event_queue (EventQueue* i, EventQueue* o)
{
    in  = i;
    out = o;
    set_event_queue_impl (i, o);
}

int Controller:: get_actor_id () const
{
    return 0;
}

void Controller:: set_actor_id (int id)
{
}
