#include "TestActor.hpp"
#include "EventQueue.hpp"
#include "Event.hpp"
#include <iostream>
using namespace std;
using namespace erica;

TestActor:: TestActor ()
{
}

TestActor:: ~TestActor ()
{
    for (int i = 0; i < (int)events.size(); i++) {
        delete events[i];
    }
    events.clear();
}

void TestActor:: set_event_listener_impl ()
{
    in->add_listener (this, "ACTOR_STORE");
    in->add_listener (this, "VIEW_STORE");
    in->add_listener (this, "CONTROLLER_STORE");
}


bool TestActor:: handle_impl (const Event* ev)
{
    if (*ev == "ACTOR_STORE") {
        events.push_back (ev);
        return true;
    }
    if (*ev == "VIEW_STORE") {
        out->enqueue (ev);
        return true;
    }
    if (*ev == "CONTROLLER_STORE") {
        out->enqueue (ev);
        return true;
    }

    return false;
}

void TestActor:: update_impl (int msec)
{
}

