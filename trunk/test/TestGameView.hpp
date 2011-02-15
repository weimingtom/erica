#ifndef __ERICA_TEST_GAME_VIEW_HPP__
#define __ERICA_TEST_GAME_VIEW_HPP__

#include "GameView.hpp"
#include "IEventListener.hpp"
#include <vector>

namespace erica {
    
    class Event;

/**
 * 
 */
class TestGameView : public GameView, public IEventListener
{
public:

    /**
     * コンストラクタ.
     */
    TestGameView (GameLogic* logic);

    /**
     * デストラクタ.
     */
    virtual ~TestGameView ();

    std::vector<const Event*> events;

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
