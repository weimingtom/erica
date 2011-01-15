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
    BasicController (EventQueue* out);

    /**
     * デストラクタ.
     */
    virtual ~BasicController ();

private:

    /**
     * IEventListener::handle()の再実装.
     */
    virtual bool handle (const Event* event);

};


} // namespace erica {

#endif

