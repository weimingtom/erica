#ifndef __ERICA_CONTROLLER_HPP__
#define __ERICA_CONTROLLER_HPP__

namespace erica {

class EventQueue;

/**
 *
 */
class Controller
{
public:
    Controller (EventQueue* queue);
    virtual ~Controller ();

protected:
    EventQueue* queue;
};


} // namespace erica {

#endif
