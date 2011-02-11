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

ostream& Event:: print (std::ostream& out) const
{
    out << "Event: name=" << e.name << ", id=0x" << hex << e.id << dec;
    return out;
}


std::ostream& operator<< (std::ostream& out, const Event& ev)
{
    return ev.print (out);
}


bool operator== (const Event& ev, const char* name)
{
    return ev.id() == hash(name);
}

bool operator== (const char* name, const Event& ev)
{
    return hash(name) == ev.id();
}



