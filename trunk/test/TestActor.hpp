
#ifndef __TEST_ACTOR_HPP__
#define __TEST_ACTOR_HPP__


#include "TestEventListener.hpp"
#include "Actor.hpp"

/**
 *
 */
class TestActor : public erica::Actor, public TestEventListener
{
public:
    TestActor ();
    virtual ~TestActor ();

private:

    /**
     *
     */
    virtual void update_impl (int msec);

    /**
     *
     */
    virtual void set_event_queue_impl (erica::EventQueue* in, erica::EventQueue* out);



};


#endif

