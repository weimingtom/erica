#ifndef __ERICA_TEST_GAME_LOGIC_HPP__
#define __ERICA_TEST_GAME_LOGIC_HPP__

#include "GameLogic.hpp"
#include "IEventListener.hpp"


namespace erica {

    class Event;

/**
 * 
 */
    class TestGameLogic : public GameLogic, public IEventListener
{
public:

    /**
     * コンストラクタ.
     */
    TestGameLogic ();

    /**
     * デストラクタ.
     */
    virtual ~TestGameLogic ();


    std::vector<const Event*> events;

    
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


};


} // namespace erica {

#endif
