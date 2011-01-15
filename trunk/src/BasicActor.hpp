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
     * @param[in] out  このアクターから出力されるイベントの出力先キュー.
     */
    BasicActor (EventQueue* out);

    /**
     * デストラクタ.
     */
    virtual ~BasicActor();

private:

    /**
     * IEventListener::handle()の再実装.
     */
    virtual bool handle (const Event* event);
    
    /**
     * Actor::update()の再実装.
     */
    virtual void update (int msec);

};


} // namespace erica {

#endif
