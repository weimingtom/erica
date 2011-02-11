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
 * 「ビュー」から「ロジック」へイベントの転送
 */
TEST (TestGameView_event_handle)
{
    TestGameLogic*  logic = new TestGameLogic;
    TestActor*      actr  = new TestActor;
    TestGameView*   view  = new TestGameView (logic);
    TestController* ctrl  = new TestController;

    logic->add_actor (actr);
    view->add_controller (ctrl);

    ActorStoreParams params;
    params.id = 100;
    view->enqueue (new Event("ACTOR_STORE", &params, sizeof(params)));

    // この呼び出しでコントローラーが"ACTORE_STORE"イベントを処理し、
    // ロジックに"ACTORE_STORE"イベントを転送する。
    view->update (0);

    // この呼び出しでアクターが"ACTORE_STORE"イベントを処理し、
    // イベントを保存する.
    logic->update (0);

    CHECK_EQUAL (1            , actr->events.size());
    CHECK_EQUAL ("ACTOR_STORE", *(actr->events[0]));
    CHECK_EQUAL (100          , ((ActorStoreParams*)actr->events[0])->id);

    delete actr->events[0];
    delete logic;
    delete actr;
    delete view;
    delete ctrl;
}
