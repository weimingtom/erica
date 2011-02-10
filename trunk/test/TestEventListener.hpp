
#ifndef __TEST_EVENT_LISTENER__
#define __TEST_EVENT_LISTENER__

#include "IEventListener.hpp"
#include <vector>

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

    void clear ();

protected:
    virtual bool handle_impl (const erica::Event* event);

public:
    std::vector<const erica::Event*> events;


};


#endif

