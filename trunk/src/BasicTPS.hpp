#ifndef __ERICA_BASIC_TPS_HPP__
#define __ERICA_BASIC_TPS_HPP__

#include <vector>
#include "GameLogic.hpp"
#include "IEventListener.hpp"


namespace erica {

    class BasicTPSData;
    class Event;
    class Actor;

/**
 * 基本的なTPS(サード・パーソン・シューティング)の進行管理を行うロジッククラス.
 */
    class BasicTPS : public GameLogic, public IEventListener
{
public:
    /**
     * コンストラクタ.
     */
    BasicTPS ();

    /**
     * デストラクタ.
     */
    virtual ~BasicTPS ();

    
private:

    /**
     * GameView::create_game()の再実装.
     */
    virtual void create_game (const char* ini_file);

    /**
     * GameView::update()の再実装.
     */
    virtual void update (int msec);

    /**
     * IEventListener::handle()の再実装.
     */
    virtual bool handle (const Event* event);    
   
    /**
     * 敵キャラクターの作成.
     */
    void spawn_enemy ();

    /**
     * 自キャラクターの弾の作成.
     */
    void spawn_my_bullet ();

private:
    std::vector<Actor*>  actors;
    BasicTPSData*        data;
};


} // namespace erica {

#endif
