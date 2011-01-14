#ifndef __ERICA_BASIC_CONTROLLER_HPP__
#define __ERICA_BASIC_CONTROLLER_HPP__

#include "Controller.hpp"
#include "IEventListener.hpp"

namespace erica {

    class Event;
    class EventQueue;

/**
 *
 */
class BasicController : public Controller, public IEventListener
{
public:
    BasicController (EventQueue* out);
    virtual ~BasicController ();

protected:
    virtual bool handle (const Event* event);

};


} // namespace erica {

#endif

