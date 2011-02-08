#ifndef __ERICA_BASIC_TEXT_PLAYER_VIEW_HPP__
#define __ERICA_BASIC_TEXT_PLAYER_VIEW_HPP__

#include "GameView.hpp"
#include "IEventListener.hpp"

namespace erica {


/**
 * テキストによる表示と人間が操作可能なコントロールを行う基本的なビュークラス.
 */
    class BasicTextPlayerView : public GameView, public IEventListener
{
public:

    /**
     * コンストラクタ.
     */
    BasicTextPlayerView (GameLogic* logic);

    /**
     * デストラクタ.
     */
    virtual ~BasicTextPlayerView ();


protected:

    /**
     * GameView::update()の再実装.
     */
    virtual void update_impl (int msec);

    /**
     * IEventListener::handle()の再実装.
     */
    virtual bool handle_impl (const Event* event);

    /**
     * GameView::render()の再実装.
     */
    virtual void render_impl () const;


private:

};


} // namespace erica {


#endif
