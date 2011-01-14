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

void EventQueue:: tick (int msec)
{
}

void EventQueue:: enqueue (const Event* event)
{
}

void EventQueue:: add_listener (int event_id, IEventListener* listner)
{
}

void EventQueue:: remove_listener (const IEventListener* listner)
{
}


