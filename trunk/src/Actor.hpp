#ifndef __ERICA_ACTOR_HPP__
#define __ERICA_ACTOR_HPP__

namespace erica {

class EventQueue;


/**
 *
 */
class Actor
{
public:
    Actor (EventQueue* queue);
    virtual ~Actor ();

    void tick (int msec);

protected:
    virtual void update (int msec) = 0;

protected:
    EventQueue* out;
};


} // namespace erica {

#endif
