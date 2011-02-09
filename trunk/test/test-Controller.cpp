#include <unittest++/UnitTest++.h>
#include <iostream>
#include "BasicController.hpp"
using namespace std;
using namespace erica;

TEST (BasicController_default_variables)
{
    BasicController* ctrl = new BasicController;
    CHECK_EQUAL (0, ctrl->get_actor_id());

}

TEST (BasicController_set_variables)
{
    
BasicController* ctrl = new BasicController;
    CHECK_EQUAL (0, ctrl->get_actor_id());

    ctrl->set_actor_id (100);

    CHECK_EQUAL (100, ctrl->get_actor_id());
}

