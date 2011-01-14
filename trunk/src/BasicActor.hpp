#ifndef __ERICA_BASIC_ACTOR_HPP__
#define __ERICA_BASIC_ACTOR_HPP__

#include "Actor.hpp"
#include "IEventListener.hpp"

namespace erica {

class EventQueue;

/**
 *
 */
class BasicActor : public Actor, public IEventListener
{
public:
    BasicActor (EventQueue* out);
    virtual ~BasicActor();

protected:
    virtual void update (int msec);

    virtual bool handle (const Event* event);
    
};


} // namespace erica {

#endif
