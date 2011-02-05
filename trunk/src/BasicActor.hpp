#ifndef __ERICA_BASIC_ACTOR_HPP__
#define __ERICA_BASIC_ACTOR_HPP__

#include "Actor.hpp"
#include "IEventListener.hpp"

namespace erica {

class EventQueue;

/**
 * もとも基本的なアクターを表現するクラス.
 */
class BasicActor : public Actor, public IEventListener
{
public:
    /**
     * コンストラクタ.
     * @param[in] in   このアクターから出力されるイベントの入力キュー.
     * @param[in] out  このアクターから出力されるイベントの出力キュー.
     */
    BasicActor (EventQueue* in, EventQueue* out);

    /**
     * デストラクタ.
     */
    virtual ~BasicActor();

protected:

    /**
     * 
     */
    virtual bool handle_impl (const Event* event);
    
    /**
     * 
     */
    virtual void update_impl (int msec);

};


} // namespace erica {

#endif
