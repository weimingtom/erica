#include "IEventListener.hpp"
#include "Event.hpp"
#include "EventQueue.hpp"
#include "EventManager.hpp"
#include "Exception.hpp"
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
    list<const Event*>::iterator i;
    for (i = events.begin(); i != events.end(); ) {
        bool         done  = false;
        const Event* event = *i;
        multimap<int, IEventListener*>::iterator j;
        multimap<int, IEventListener*>::iterator begin = listeners.lower_bound (event->id());
        multimap<int, IEventListener*>::iterator end   = listeners.upper_bound (event->id());
        for (j = begin; j != end; j++) {
            done = j->second->handle (event);
            if (done) {
                break;
            }
        }
        if (done) {
            events.erase (i++);
            delete event;
        } else {
            i++;
        }
    }
}


void EventQueue:: enqueue (const Event* ev)
{
    if (ev == NULL) {
        throw Exception (__FILE__, __func__, "Event is NULL.");
    }
    if (!EventManager::find(ev->name())) {
        throw Exception (__FILE__, __func__, "Event is not registered. name=%s", ev->name());
    }

    events.push_back (ev);
}

const Event* EventQueue:: dequeue ()
{
    if (!events.empty()) {
        const Event* ev = events.front ();
        events.pop_front();
        return ev;
    }

    return NULL;
}

void EventQueue:: add_listener (IEventListener* listener, const char* event_name)
{
    if (listener == NULL) {
        throw Exception (__FILE__, __func__, "Listerner is NULL.");
    }
    if (event_name == NULL) {
        throw Exception (__FILE__, __func__, "Event name is NULL.");
    }
    // TODO
    // 2重登録は無視すべき.

    int event_id = EventManager::find (event_name);
    listeners.insert (make_pair<int, IEventListener*>(event_id, listener));
}

void EventQueue:: remove_listener (const IEventListener* listener)
{
    if (listener == NULL) {
        throw Exception (__FILE__, __func__, "Listener is NULL.");
    }

    multimap<int, IEventListener*>::iterator it;
    for (it = listeners.begin(); it != listeners.end(); ) {
        if (it->second == listener) {
            listeners.erase (it++);
        } else {
            it++;
        }
    }
}


void EventQueue:: clear ()
{
    listeners.clear ();
}

int EventQueue:: size () const
{
    return listeners.size ();
}
