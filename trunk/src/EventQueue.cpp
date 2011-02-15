#include "IEventListener.hpp"
#include "Hash.hpp"
#include "Event.hpp"
#include "EventQueue.hpp"
#include "Exception.hpp"
using namespace erica;
using namespace std;


EventQueue:: EventQueue ()
{
}

EventQueue:: ~EventQueue ()
{
    // 未処理のイベントはこのクラスがdeleteする.
    list<const Event*>::iterator it;
    for (it = events.begin(); it != events.end(); it++) {
        delete *it;
    }
    events.clear();
}

void EventQueue:: trigger ()
{
    list<const Event*>::iterator i;
    for (i = events.begin(); i != events.end(); ) {
        const Event* event = *i;
        bool         done  = false;
        multimap<unsigned long long, IEventListener*>::iterator j;
        multimap<unsigned long long, IEventListener*>::iterator begin = listeners.lower_bound (event->id());
        multimap<unsigned long long, IEventListener*>::iterator end   = listeners.upper_bound (event->id());
        for (j = begin; j != end; j++) {
            done = j->second->handle (event);
            if (done) {
                break;
            }
        }
        if (done) {
            events.erase (i++);
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

    pair<unsigned long long, IEventListener*> element (hash(event_name), listener);
    listeners.insert (element);
}

void EventQueue:: remove_listener (const IEventListener* listener)
{
    if (listener == NULL) {
        throw Exception (__FILE__, __func__, "Listener is NULL.");
    }

    multimap<unsigned long long, IEventListener*>::iterator it;
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
    events.clear ();
}

int EventQueue:: size () const
{
    return events.size ();
}
