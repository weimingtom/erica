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


bool IEventListener:: handle (const Event* event)
{
    return handle_impl (event);
}


