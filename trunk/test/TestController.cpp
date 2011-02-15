#include "TestController.hpp"
#include "TestEventParams.hpp"
#include "Event.hpp"
#include "EventQueue.hpp"
using namespace std;
using namespace erica;


TestController:: TestController ()
{
}


TestController::  ~TestController ()
{
}

void TestController:: set_event_listener_impl ()
{
    in->add_listener (this, "LOGIC_STORE");
    in->add_listener (this, "ACTOR_STORE");
    in->add_listener (this, "CONTROLLER_STORE");

}

bool TestController:: handle_impl (const Event* ev)
{
    if (*ev == "LOGIC_STORE") {
        out->enqueue (ev);
        return true;
    }
    if (*ev == "ACTOR_STORE") {
        out->enqueue (ev);
        return true;
    }
    if (*ev == "CONTROLLER_STORE") {
        events.push_back (ev);
        return true;
    }

    return false;
}

void TestController:: update_impl (int msec)
{

}

