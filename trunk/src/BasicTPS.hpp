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

    
protected:

    /**
     * 
     */
    virtual void load_game_impl (const char* ini_file);

    /**
     * 
     */
    virtual void update_impl (int msec);

    /**
     * 
     */
    virtual bool handle_impl (const Event* event);    
   
private:
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
