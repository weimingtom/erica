#include "BasicController.hpp"
#include "BasicEventParams.hpp"
#include "EventQueue.hpp"
#include "Event.hpp"
#include <cstring>
using namespace erica;
using namespace std;


BasicController:: BasicController () : Controller()
{
}

BasicController:: ~BasicController ()
{
}

void BasicController:: update_impl (int msec)
{

}


void BasicController:: set_event_queue_impl (EventQueue* in, EventQueue* out)
{
    // イベントリスナーに登録.
    in->add_listener (this, "KEY_PRESSED");
}


bool BasicController:: handle_impl (const Event* ev)
{
    if (strcmp(ev->name(), "KEY_PRESSED") == 0) {
        char key = ((KeyPressedParams*)ev->params())->key;
        ActorWalkParams params;
        switch (key) {
        case 4: params.dir = ActorWalkParams::LEFT   ; break;
        case 6: params.dir = ActorWalkParams::RIGHT  ; break;
        case 2: params.dir = ActorWalkParams::FORWARD; break;
        case 8: params.dir = ActorWalkParams::BACK   ; break;
        default: return false;
        }
        out->enqueue (new Event("WALK", &params, sizeof(params)));
        return true;
    }

    return false;
}


