#include "TestEventListener.hpp"
#include "Event.hpp"
using namespace std;
using namespace erica;


TestEventListener:: TestEventListener ()
{
}

TestEventListener:: ~TestEventListener ()
{
}


bool TestEventListener:: handle_impl (const Event* ev)
{
    events.push_back (ev);
    return true;
}



