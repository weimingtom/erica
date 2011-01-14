#ifndef __ERICA_GAME_LOGIC_HPP__
#define __ERICA_GAME_LOGIC_HPP__

#include <vector>
namespace erica {

class GameView;
class EventQueue;
    class Event;


/**
 *
 */
class GameLogic
{
public:
    GameLogic ();
    virtual ~GameLogic ();

    void load_game (const char* ini_file);

    void tick (int msec);

    GameView* get_view (int id) const;

    void enqueue (const Event* event);

protected:
    virtual void update (int msec) = 0;
    
    virtual void create_game (const char* ini_file) = 0;

protected:
    EventQueue* in;
    EventQueue* out;
    std::vector<GameView*> views;
};


} // namespace erica {

#endif
