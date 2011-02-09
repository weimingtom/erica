#include "EventManager.hpp"
#include "Definitions.hpp"
#include "Exception.hpp"
#include <iostream>
using namespace std;
using namespace erica;

std::map<const char*, int> EventManager:: registered;


void EventManager:: regist (int id, const char* name)
{
    if (name == NULL) {
        throw Exception (__FILE__, __func__, "Event name is NULL.");
    }
    if (id < EVENT_ID_MIN || id >= EVENT_ID_MAX) {
        throw Exception (__FILE__, __func__, "Event ID is invalid, id=%d.", id);
    }

    registered.insert (make_pair<const char*, int>(name, id));
}

const map<const char*, int> EventManager:: get_registered_events ()
{
    return registered;
}

int EventManager:: find (const char* name)
{
    if (name == NULL) {
        throw Exception (__FILE__, __func__, "Event name is NULL.");
    }
    map<const char*, int>::iterator it;
    it = registered.find(name);
    if (it != registered.end()) {
        return it->second;
    }

    return 0;
}



