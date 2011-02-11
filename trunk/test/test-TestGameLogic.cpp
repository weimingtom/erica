#include <unittest++/UnitTest++.h>
#include <iostream>
#include "Event.hpp"
#include "TestGameLogic.hpp"
#include "TestGameView.hpp"
#include "TestActor.hpp"
#include "TestController.hpp"
#include "TestEventParams.hpp"
using namespace std;
using namespace erica;


/**
 * 「ロジック」から「ビュー」へイベントの転送
 */
TEST (TestGameLogic_event_handle)
{
    TestGameLogic* logic = new TestGameLogic;
    TestActor*     actr  = new TestActor;
    TestGameView*   view  = new TestGameView (logic);
    TestController* ctrl  = new TestController;

    logic->add_actor (actr);
    view->add_controller (ctrl);

    ControllerStoreParams params;
    params.id = 100;
    logic->enqueue (new Event("CONTROLLER_STORE", &params, sizeof(params)));

    // この呼び出しでロジックが"CONTROLLER_STORE"イベントを処理し、
    // ビューに"CONTROLLER_STORE"イベントを処理する。
    logic->update (0);
    
    // この呼び出しでコントローラーが"ACTORE_STORE"イベントを処理し、
    // イベントを保存する.
    view->update (0);

    CHECK_EQUAL (1         , actr->events.size());
    CHECK_EQUAL ("CONTROLLER_STORE", *(actr->events[0]));
    CHECK_EQUAL (100          , ((ControllerStoreParams*)actr->events[0])->id);

    delete ctrl->events[0];
    delete logic;
    delete actr;
    delete view;
    delete ctrl;
}
