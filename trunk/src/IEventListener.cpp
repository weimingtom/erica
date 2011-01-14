#include "IEventListener.hpp"
#include "Event.hpp"
using namespace erica;
using namespace std;


IEventListener:: IEventListener ()
{
}

IEventListener:: ~IEventListener ()
{
}

bool IEventListener:: accept (const Event* event)
{
    return false;
}


