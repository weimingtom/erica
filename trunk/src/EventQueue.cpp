#include "IEventListener.hpp"
#include "Event.hpp"
#include "EventQueue.hpp"
using namespace erica;
using namespace std;


EventQueue:: EventQueue ()
{
}

EventQueue:: ~EventQueue ()
{
}

void EventQueue:: trigger ()
{
}


void EventQueue:: enqueue (const Event* event)
{
}

Event* EventQueue:: dequeue ()
{
    return NULL;
}

void EventQueue:: add_listener (IEventListener* listener, const char* event_name)
{
}

void EventQueue:: remove_listener (const IEventListener* listner)
{
}


void EventQueue:: clear ()
{
}

int EventQueue:: size () const
{
    return 0;
}
