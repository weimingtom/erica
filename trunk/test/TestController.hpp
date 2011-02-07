
#ifndef __TEST_CONTROLLER__
#define __TEST_CONTROLLER__

#include "TestEventListener.hpp"
#include "Controller.hpp"


/**
 *
 */
class TestController : public erica::Controller, public TestEventListener
{
public:
    TestController ();
    virtual ~TestController ();

private:
    virtual void set_event_queue_impl (erica::EventQueue* in, erica::EventQueue* out);

    virtual void update_impl (int msec);

};



#endif
