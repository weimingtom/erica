#ifndef __ERICA_EVENT_QUEUE_HPP__
#define __ERICA_EVENT_QUEUE_HPP__

#include <list>
#include <map>

namespace erica {
class Event;
class IEventListener;

/**
 *
 */
class EventQueue
{
public:
    EventQueue ();

    ~EventQueue ();

    void tick (int msec);

    void enqueue (const Event* event);

    void add_listener (int event_id, IEventListener* listner);

    void remove_listener (const IEventListener* listner);

private:
    std::list<const Event*>  events;
    std::multimap<int, IEventListener*> listners;
};


} // namespace erica {

#endif


