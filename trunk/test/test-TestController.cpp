#include <unittest++/UnitTest++.h>
#include <iostream>
#include "Event.hpp"
#include "TestGameLogic.hpp"
#include "TestActor.hpp"
#include "TestGameView.hpp"
#include "TestController.hpp"
#include "TestEventParams.hpp"
using namespace std;
using namespace erica;

TEST (TestController_listen_KEY_PRESSED)
{
    TestGameLogic*  logic = new TestGameLogic;
    TestActor*      actr  = new TestActor;
    TestGameView*   view  = new TestGameView (logic);
    TestController* ctrl  = new TestController;

    logic->add_actor (actr);
    view->add_controller (ctrl);

    KeyPressedParams params;
    params.key = '4';
    view->enqueue (new Event("KEY_PRESSED", &params, sizeof(params)));
    params.key = '6';
    view->enqueue (new Event("KEY_PRESSED", &params, sizeof(params)));
    params.key = '8';
    view->enqueue (new Event("KEY_PRESSED", &params, sizeof(params)));
    params.key = '2';
    view->enqueue (new Event("KEY_PRESSED", &params, sizeof(params)));

    // この呼び出しでコントローラーが"KEY_PRESSED"イベントを処理し、
    // ロジックに"WALK"イベントを送付する。
    view->update (0);

    // この呼び出しでアクターが"WALK"イベントを処理しする。
    logic->update (0);

    CHECK_EQUAL (4     , actr->events.size());
    CHECK_EQUAL ("WALK", *(actr->events[0]));
    CHECK_EQUAL ("WALK", *(actr->events[1]));
    CHECK_EQUAL ("WALK", *(actr->events[2]));
    CHECK_EQUAL ("WALK", *(actr->events[3]));
}
