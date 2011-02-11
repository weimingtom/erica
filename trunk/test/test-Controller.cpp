#include <unittest++/UnitTest++.h>
#include <iostream>
#include "TestController.hpp"
#include "Definitions.hpp"
using namespace std;
using namespace erica;

TEST (TestController_default_variables)
{
    TestController* ctrl = new TestController;

    // コントローラーのデフォルト・
    CHECK_EQUAL (0, ctrl->get_actor_id());

    delete ctrl;
}

TEST (TestController_set_variables)
{
    TestController* ctrl = new TestController;
    CHECK_EQUAL (0, ctrl->get_actor_id());

    ctrl->set_actor_id (ACTOR_ID_USER_MIN);
    CHECK_EQUAL (ACTOR_ID_USER_MIN, ctrl->get_actor_id());

    ctrl->set_actor_id (ACTOR_ID_USER_MAX-1);
    CHECK_EQUAL (ACTOR_ID_USER_MAX-1, ctrl->get_actor_id());

    delete ctrl;
}

