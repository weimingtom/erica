#include "BasicActor.hpp"
#include "EventQueue.hpp"
using namespace erica;
using namespace std;


BasicActor:: BasicActor () : Actor()
{
}

BasicActor:: ~BasicActor()
{
}

bool BasicActor:: handle_impl (const Event* event)
{
    return false;
}


void BasicActor:: update_impl (int msec)
{

}

void BasicActor:: set_event_listener_impl ()
{
    // WALKイベントの監視
}
