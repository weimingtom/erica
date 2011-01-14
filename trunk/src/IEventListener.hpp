#ifndef __ERICA_IEVENT_LISTENER_HPP__
#define __ERICA_IEVENT_LISTENER_HPP__

namespace erica {

    class Event;

/**
 * イベントを受け取るための純粋インターフェース・クラス.
 */
class IEventListener
{
public:
    IEventListener ();
    virtual ~IEventListener ();

    bool accept (const Event* event);

protected:
    virtual bool handle (const Event* event) = 0;
};


} // namespace erica {


#endif
