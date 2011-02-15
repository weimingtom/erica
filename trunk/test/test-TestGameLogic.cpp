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
 * イベント処理のテスト.
 */
TEST (TestGameLogic_event_handle)
{
    TestGameLogic*  logic = new TestGameLogic;
    TestActor*      actr  = new TestActor;
    TestGameView*   view  = new TestGameView (logic);
    TestController* ctrl  = new TestController;
    logic->add_view (view);
    logic->add_actor (actr);
    view->add_controller (ctrl);



    LogicStoreParams params1;
    params1.id = 101;
    logic->enqueue (new Event("LOGIC_STORE", &params1, sizeof(params1)));

    ActorStoreParams params2;
    params2.id = 102;
    logic->enqueue (new Event("ACTOR_STORE", &params2, sizeof(params2)));

    ViewStoreParams params3;
    params3.id = 103;
    logic->enqueue (new Event("VIEW_STORE", &params3, sizeof(params3)));

    ControllerStoreParams params4;
    params4.id = 104;
    logic->enqueue (new Event("CONTROLLER_STORE", &params4, sizeof(params4)));
    
    // この呼び出しでロジックが全イベントを処理する.
    logic->update (0);

    // "LogicStore"イベント
    CHECK_EQUAL (1            , logic->events.size());
    CHECK_EQUAL ("LOGIC_STORE", *(logic->events[0]));
    CHECK_EQUAL (101          , ((LogicStoreParams*)logic->events[0]->params())->id);
    
    // "ActorStore"イベント
    CHECK_EQUAL (1            , actr->events.size());
    CHECK_EQUAL ("ACTOR_STORE", *(actr->events[0]));
    CHECK_EQUAL (102          , ((ActorStoreParams*)actr->events[0]->params())->id);

    // この呼び出しでビューが全イベントを処理する.
    view->update (0);

    // "ViewStore"イベント
    CHECK_EQUAL (1           , view->events.size());
    CHECK_EQUAL ("VIEW_STORE", *(view->events[0]));
    CHECK_EQUAL (103         , ((ViewStoreParams*)view->events[0]->params())->id);
    
    // "ControllerStore"イベント
    CHECK_EQUAL (1                 , ctrl->events.size());
    CHECK_EQUAL ("CONTROLLER_STORE", *(ctrl->events[0]));
    CHECK_EQUAL (104               , ((ControllerStoreParams*)ctrl->events[0]->params())->id);


    delete logic;
    delete actr;
    delete view;
    delete ctrl;
}
