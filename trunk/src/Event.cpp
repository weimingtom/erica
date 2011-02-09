#include "Event.hpp"
#include "Exception.hpp"
#include "EventManager.hpp"
#include <iostream>
#include <cstring>
using namespace erica;
using namespace std;


Event:: Event (const char* name, const void* params, int size) : e(NULL,NULL,0)
{
    if (name == NULL) {
        throw Exception (__FILE__, __func__, "Event name is NULL.");
    }
    if (params == NULL) {
        throw Exception (__FILE__, __func__, "Event params is NULL.");
    }
    if (size < 0 || size > 65535) {
        throw Exception (__FILE__, __func__, "Event size is invalid size=%d.", size);
    }

    e.name   = name;
    e.params = new char [size];
    e.size   = size;

    memcpy (e.params, params, size);
}

Event:: ~Event ()
{
    delete (char*)e.params;
}

const char* Event:: name () const
{
    return e.name;
}

const void* Event:: params () const
{
    return e.params;
}


int Event:: size () const
{
    return e.size;
}


int Event:: id () const
{
    return EventManager:: find(e.name);
}



