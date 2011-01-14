#ifndef __ERICA_GAME_VIEW_HPP__
#define __ERICA_GAME_VIEW_HPP__

#include "IEventListener.hpp"

namespace erica {

    class GameLogic;
    class EventQueue;


/**
 *
 */
    class GameView
{
public:
    GameView ();
    virtual ~GameView ();

    void tick (int msec);

    void add_game_logic (GameLogic* logic);

    void enqueue (const Event* event);

protected:
    virtual void update (int msec) const = 0;

    virtual void render () const = 0;

protected:
    EventQueue* in;
    EventQueue* out;
    GameLogic*  logic;
};


} // namespace erica {

#endif




