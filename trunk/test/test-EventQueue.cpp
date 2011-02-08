#include <unittest++/UnitTest++.h>
#include <iostream>
#include "Event.hpp"
#include "EventQueue.hpp"
#include "TestEventListener.hpp"
using namespace std;
using namespace erica;


TEST (EvnetQueue_default_variables) 
{
    EventQueue* in = new EventQueue;
    Event*      ev1 = new Event ("Hartmann", (void*)100, sizeof(void*), 1);
    Event*      ev2 = new Event ("Hartmann", (void*)200, sizeof(void*), 2);

    in->enqueue (ev1);
    in->enqueue (ev2);
    CHECK_EQUAL (2, in->size());
    
    ev1 = in->dequeue ();
    CHECK_EQUAL (1, in->size());

    in->clear ();
    CHECK_EQUAL (0, in->size());
}


TEST (EvnetQueue_update)
{
    TestEventListener* listener = new TestEventListener;
    EventQueue*       in      = new EventQueue;
    Event*            ev1     = new Event ("Hartmann", (void*)100, sizeof(void*), 1);

    in->add_listener (listener, "Hartmann");
    in->enqueue (ev1);

    // キューに入っているイベントが処理されて
    // リスナーのhandle()関数が呼ばれる。
    in->trigger ();

    const Event* ev2 = listener->get_event ();
    CHECK       (ev2 != NULL);
    CHECK       (ev2 != ev1);
    CHECK_EQUAL (ev1->name()        , ev2->name());
    CHECK_EQUAL (ev1->size()        , ev2->size());
    CHECK_EQUAL (ev1->id()          , ev2->id());
    CHECK_EQUAL (ev1->get_actor_id(), ev2->get_actor_id());
    CHECK_EQUAL (ev1->params()      , ev2->params());
}
