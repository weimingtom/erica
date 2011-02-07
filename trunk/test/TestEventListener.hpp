
#ifndef __TEST_EVENT_LISTENER__
#define __TEST_EVENT_LISTENER__

#include "IEventListener.hpp"

namespace erica {
    class Event;
}

/**
 *
 */
class TestEventListener : public erica::IEventListener
{
public:
    TestEventListener ();
    virtual ~TestEventListener ();

    const erica::Event* get_event () const;

protected:
    virtual bool handle_impl (const erica::Event* event);

private:
    const erica::Event* event;
};


#endif

