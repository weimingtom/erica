#include <unittest++/UnitTest++.h>
#include <iostream>
#include "Event.hpp"
#include "TestEventParams.hpp"
using namespace std;
using namespace erica;


TEST (Regist_Events)
{
    /**
     * ここで使用されるイベント一覧.
     */
    Event::regist (1, "Hartmann");

    map<int, const char*> events = Event:: get_registered_events ();
    CHECK_EQUAL (1, events.size());
    CHECK_EQUAL ("Hartmann", events.find(1)->second);
}

TEST (Event_defualt_variables)
{
    Event* ev = new Event("Hartmann", (void*)200, sizeof(void*), 101);

    CHECK_EQUAL ("Hartmann", ev->name());
    CHECK_EQUAL (1         , ev->id());
    CHECK_EQUAL (4         , ev->size());
    CHECK_EQUAL (101       , ev->get_actor_id());
    CHECK_EQUAL ((void*)200, ev->params());

    delete ev;
}
