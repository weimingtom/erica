#include "TestGameLogic.hpp"
#include "Event.hpp"
#include "EventQueue.hpp"
using namespace erica;
using namespace std;

TestGameLogic:: TestGameLogic ()
{
    // inキューイベント
    in->add_listener (this, "LOGIC_STORE");
    
}

TestGameLogic:: ~TestGameLogic ()
{
    for (int i = 0; i < (int)events.size(); i++) {
        delete events[i];
    }
    events.clear ();
}

void TestGameLogic:: load_game_impl (const char* ini_file)
{
}

void TestGameLogic:: update_impl (int msec)
{
}

bool TestGameLogic:: end_of_game_impl () const
{
    return false;
}

bool TestGameLogic:: handle_impl (const Event* ev)
{
    if (*ev == "LOGIC_STORE") {
        events.push_back (ev);
        return true;
    }

    return false;
}
