#include <unittest++/UnitTest++.h>
#include <iostream>
#include "EventManager.hpp"
#include "TestEventParams.hpp"
using namespace std;
using namespace erica;


TEST (EventManager_regist)
{
    /**
     * このイベントはテスト全体で使用される.
     */
    EventManager::regist (1, "Hartmann");

    map<int, const char*> events = EventManager:: get_registered_events ();
    CHECK_EQUAL (1, events.size());
    CHECK_EQUAL ("Hartmann", events.find(1)->second);
}
