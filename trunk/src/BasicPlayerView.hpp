#ifndef __ERICA_BASIC_PLAYER_VIEW_HPP__
#define __ERICA_BASIC_PLAYER_VIEW_HPP__

#include "GameView.hpp"
#include "IEventListener.hpp"

namespace erica {

    namespace m3g {
        class World;
        class Node;
    };

    class Event;

/**
 * 基本的な人間が操作可能な表示とコントロールを行うビュークラス.
 */
    class BasicPlayerView : public GameView, public IEventListener
{
public:

    /**
     * コンストラクタ.
     */
    BasicPlayerView (GameLogic* logic);

    /**
     * デストラクタ.
     */
    virtual ~BasicPlayerView ();

private:
    /**
     * GameView::update()の再実装.
     */
    virtual void update (int msec);

    /**
     * IEventListener::handle()の再実装.
     */
    virtual bool handle (const Event* event);

    /**
     * GameView::render()の再実装.
     */
    virtual void render () const;


private:

    m3g::World* wld;
    std::map<int, m3g::Node*> nodes;
    

};

} // namespace erica {


#endif
