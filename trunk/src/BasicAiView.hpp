#ifndef __ERICA_BASIC_AI_VIEW_HPP__
#define __ERICA_BASIC_AI_VIEW_HPP__

#include "GameView.hpp"
#include "IEventListener.hpp"

namespace erica {

    class Event;

/**
 * 基本的なAIが操作する表示とコントロールを行うビュークラス.
 */
    class BasicAiView : public GameView, public IEventListener
{
public:

    /**
     * コンストラクタ.
     */
    BasicAiView (GameLogic* logic);

    /**
     * デストラクタ.
     */
    virtual ~BasicAiView ();


protected:

    /**
     * GameViewクラスのupdate()の再実装.
     */
    virtual void update_impl (int msec);

    /**
     * IEventListenerクラスのhandle()の再実装.
     */
    virtual bool handle_impl (const Event* event);

    /**
     * GameViewクラスのrender()の再実装.
     */
    virtual void render_impl () const;
};

} // namespace erica {


#endif
