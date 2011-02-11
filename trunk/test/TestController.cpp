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

void TestController:: set_event_listener_impl (EventQueue* in, EventQueue* out)
{

}

bool TestController:: handle_impl (const Event* ev)
{
    if (*ev == "KEY_PRESSED") {
        char key = ((KeyPressedParams*)ev->params())->key;
        ActorWalkParams params;
        switch (key) {
        case '4': params.dir = ActorWalkParams::LEFT   ; break;
        case '6': params.dir = ActorWalkParams::RIGHT  ; break;
        case '2': params.dir = ActorWalkParams::FORWARD; break;
        case '8': params.dir = ActorWalkParams::BACK   ; break;
        default: return false;
        }
        out->enqueue (new Event("WALK", &params, sizeof(params)));
        return true;
    }

    return false;
}


void TestController:: update_impl (int msec)
{

}

