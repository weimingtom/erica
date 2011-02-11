
#ifndef __TEST_ACTOR_HPP__
#define __TEST_ACTOR_HPP__


#include "IEventListener.hpp"
#include "Actor.hpp"
#include <vector>

namespace erica {
    class Event;
}

/**
 *
 */
class TestActor : public erica::Actor, public erica::IEventListener
{
public:
    TestActor ();

    virtual ~TestActor ();


    std::vector<const erica::Event*> events;

private:

    /**
     *
     */
    virtual void update_impl (int msec);

    /**
     *
     */
    virtual void set_event_listener_impl ();

    /**
     *
     */
    bool handle_impl (const erica::Event* ev);

};


#endif

