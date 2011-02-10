#include "Event.hpp"
#include "Hash.hpp"
#include "Exception.hpp"
#include <iostream>
#include <cstring>
using namespace erica;
using namespace std;


Event:: Event (const char* name, const void* params, int size) : e(NULL,NULL,0)
{
    if (name == NULL) {
        throw Exception (__FILE__, __func__, "Event name is NULL.");
    }
    if (size < 0 || size > 65535) {
        throw Exception (__FILE__, __func__, "Event size is invalid size=%d.", size);
    }

    e.name   = name;
    e.params = new char [size];
    memcpy (e.params, params, size);
    e.size   = size;
    e.id     = hash(name);
}

Event:: ~Event ()
{
    delete [] (char*)e.params;
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


unsigned long long Event:: id () const
{
    return e.id;
}



