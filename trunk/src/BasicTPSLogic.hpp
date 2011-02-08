#ifndef __ERICA_BASIC_TPS_LOGIC_HPP__
#define __ERICA_BASIC_TPS_LOGIC_HPP__

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
    class BasicTPSLogic : public GameLogic, public IEventListener
{
public:
    /**
     * コンストラクタ.
     */
    BasicTPSLogic ();

    /**
     * デストラクタ.
     */
    virtual ~BasicTPSLogic ();

    
protected:

    /**
     * GameLogic:: load_game()の再実装.
     */
    virtual void load_game_impl (const char* ini_file);

    /**
     * GameLogic:: update()の再実装.
     */
    virtual void update_impl (int msec);

    /**
     * GameLogic:: end_of_game()の再実装.
     */
    virtual bool end_of_game_impl () const;

    /**
     * IEventListener:: handle()の再実装.
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

    BasicTPSData*        data;
};


} // namespace erica {

#endif
