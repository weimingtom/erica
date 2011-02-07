#include <unittest++/UnitTest++.h>
#include <iostream>
#include "BasicTPS.hpp"
#include "Event.hpp"
#include "TestActor.hpp"
using namespace std;
using namespace erica;


TEST (BasicTPS_default_vriables)
{
    TestActor* actr = new TestActor;
    actr->set_actor_id (101);

    BasicTPS* logic = new BasicTPS;
    logic->add_actor (actr);

    Event* ev1 = new Event ("Hartmann", (void*)200, sizeof(void*), 101);
    logic->enqueue (ev1);

    // これでイベントがアクターに送付される.
    logic->update (0);
    
    const Event* ev2 = actr->get_event ();

    CHECK       (ev2 != NULL);
    CHECK_EQUAL ("Hartmann", ev2->name());
    CHECK_EQUAL (1         , ev2->id());
    CHECK_EQUAL (4         , ev2->size());
    CHECK_EQUAL (101       , ev2->get_actor_id());
    CHECK_EQUAL ((void*)200, ev2->params());
    
    delete ev1;
}
