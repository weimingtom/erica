#ifndef __ERICA_BASIC_CONTROLLER_HPP__
#define __ERICA_BASIC_CONTROLLER_HPP__

#include "Controller.hpp"
#include "IEventListener.hpp"

namespace erica {

    class Event;
    class EventQueue;

/**
 * 基本的なキー入力を処理するコントローラークラス.
 */
class BasicController : public Controller, public IEventListener
{
public:
    /**
     * コンストラクタ.
     */
    BasicController ();

    /**
     * デストラクタ.
     */
    virtual ~BasicController ();


protected:

    /**
     * Controller:: update()の実装.
     */
    
virtual void update_impl (int msec);

    /**
     * Controller:: set_event_listener()の実装.
     */
    virtual void set_event_listener_impl ();

    /**
     * IEventListener::handle()の実装.
     */
    virtual bool handle_impl (const Event* event);

};


} // namespace erica {

#endif

