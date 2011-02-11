#include <unittest++/UnitTest++.h>
#include <iostream>
#include "Event.hpp"
#include "Hash.hpp"
using namespace std;
using namespace erica;


TEST (Event_defualt_variables)
{
    int params = 200;
    Event* ev = new Event("Hartmann", &params, sizeof(params));

    CHECK_EQUAL ("Hartmann", ev->name());
    CHECK_EQUAL (251134809 , ev->id());
    CHECK_EQUAL (4         , ev->size());
    CHECK_EQUAL (200       , *(int*)ev->params());

    delete ev;
}

TEST (Event_case_insensitive)
{
    Event* ev1 = new Event("Hartmann", NULL, 0);
    Event* ev2 = new Event("HARTMANN", NULL, 0);
    
    // iベント名は大文字小文字を区別しない
    CHECK_EQUAL (ev1->id(), ev2->id());

    delete ev1;
    delete ev2;
}

TEST (Event_operator_equal)
{
    Event* ev = new Event("Hartmann", NULL, 0);

    CHECK_EQUAL (*ev, "Hartmann");

    delete ev;
}

