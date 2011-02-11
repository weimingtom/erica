
#ifndef __TEST_CONTROLLER__
#define __TEST_CONTROLLER__

#include "IEventListener.hpp"
#include "Controller.hpp"

namespace erica {
    class Event;
}

/**
 *
 */
class TestController : public erica::Controller, public erica::IEventListener
{
public:
    TestController ();
    virtual ~TestController ();

private:
    virtual void set_event_listener_impl (erica::EventQueue* in, erica::EventQueue* out);

    virtual void update_impl (int msec);

    virtual bool handle_impl (const erica::Event* ev);
};



#endif
