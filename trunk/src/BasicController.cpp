#include "BasicController.hpp"
#include "EventQueue.hpp"
using namespace erica;
using namespace std;

BasicController:: BasicController () : Controller()
{
}

BasicController:: ~BasicController ()
{
}

void BasicController:: set_event_queue_impl (EventQueue* in, EventQueue* out)
{
    // イベントリスナーに登録.
    in->add_listener (this, "ALL");
}


bool BasicController:: handle_impl (const Event* event)
{
    return false;
}


