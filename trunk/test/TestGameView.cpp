#include "TestGameView.hpp"
#include "EventQueue.hpp"
#include "Event.hpp"
using namespace erica;
using namespace std;


TestGameView:: TestGameView (GameLogic* logic) : GameView(logic)
{
    // inイベント
    in->add_listener (this, "VIEW_STORE");
}

TestGameView:: ~TestGameView ()
{
    for (int i = 0; i < (int)events.size(); i++) {
        delete events[i];
    }
    events.clear ();
}
 
void TestGameView:: update_impl (int msec)
{

}

bool TestGameView:: handle_impl (const Event* ev)
{
    if (*ev == "VIEW_STORE") {
        events.push_back (ev);
        return true;
    }
    
    return false;
}

void TestGameView:: render_impl () const
{
}

