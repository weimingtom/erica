#include <unittest++/UnitTest++.h>
#include <iostream>
#include "TestGameLogic.hpp"
#include "Event.hpp"
#include "TestActor.hpp"
using namespace std;
using namespace erica;


TEST (TestGameLogic_event)
{
    TestGameLogic* logic = new TestGameLogic;
    TestActor*     actr  = new TestActor;
    logic->add_actor (actr);

    Event* ev = new Event ("Hartmann", NULL, 0);
    logic->enqueue (ev);

    // この呼び出しでイベントがアクターに送付される.
    logic->update (0);
    
    CHECK_EQUAL (1         , actr->events.size());
    CHECK_EQUAL ("Hartmann", *(actr->events[0]));
    
    delete ev;
}
