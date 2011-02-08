#include <unittest++/UnitTest++.h>
#include <iostream>
#include "BasicTextPlayerView.hpp"
#include "Event.hpp"
#include "TestController.hpp"
using namespace std;
using namespace erica;


TEST (BasicPlayerView_default_variables)
{
    TestController* ctrl = new TestController;
    ctrl->set_actor_id (101);

    BasicTextPlayerView* view = new BasicTextPlayerView (NULL);
    view->add_controller (ctrl);

    Event* ev1 = new Event ("Hartmann", (void*)200, sizeof(void*), 101);
    view->enqueue (ev1);

    // これでイベントがコントローラーに送付される.
    view->update (0);
    
    const Event* ev2 = ctrl->get_event ();

    CHECK       (ev2 != NULL);
    CHECK_EQUAL ("Hartmann", ev2->name());
    CHECK_EQUAL (1         , ev2->id());
    CHECK_EQUAL (4         , ev2->size());
    CHECK_EQUAL (101       , ev2->get_actor_id());
    CHECK_EQUAL ((void*)200, ev2->params());
    
    delete ev1;
}
