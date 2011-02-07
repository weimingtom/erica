#include "TestEventListener.hpp"
#include "Event.hpp"
using namespace std;
using namespace erica;


TestEventListener:: TestEventListener () : event(0)
{
}

TestEventListener:: ~TestEventListener ()
{
}

const Event* TestEventListener:: get_event () const
{
    return event;
}

bool TestEventListener:: handle_impl (const Event* ev)
{
    event = new Event (*ev);
    return false;
}



