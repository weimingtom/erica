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
     * @param[in] out  このコントローラーから出るイベントの出力先キュー.
     */
    BasicController ();

    /**
     * デストラクタ.
     */
    virtual ~BasicController ();


private:

    /**
     *
     */
    virtual void set_event_queue_impl (EventQueue* in, EventQueue* out);

    /**
     * IEventListener::handle()の実装.
     */
    virtual bool handle_impl (const Event* event);

};


} // namespace erica {

#endif

