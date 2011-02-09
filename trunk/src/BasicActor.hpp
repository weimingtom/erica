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
     */
    BasicActor ();

    /**
     * デストラクタ.
     */
    virtual ~BasicActor();

protected:

    /**
     * Actor::set_event_queue()の実装.
     */
    virtual void set_event_queue_impl (EventQueue* in, EventQueue* out);

    /**
     * IEventListener::handle()の実装.
     */
    virtual bool handle_impl (const Event* event);
    
    /**
     * Actor::update()の実装.
     */
    virtual void update_impl (int msec);

};


} // namespace erica {

#endif
