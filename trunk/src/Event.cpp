#include "Event.hpp"
#include <iostream>
using namespace erica;
using namespace std;

std::map<int, const char*> Event:: registered;


Event:: Event (const char* name, const void* params, int size, int actor_id)
{
}

Event:: ~Event ()
{
}

const char* Event:: name () const
{
    return "DUMMY";
}

const void* Event:: params () const
{
    return "DUMMY";
}


int Event:: size () const
{
    return 0;
}


int Event:: id () const
{
    return 0;
}

int Event:: get_actor_id () const
{
    return 0;
}


void Event:: regist (int id, const char* name)
{
    registered.insert (make_pair(id, name));
}

std::map<int, const char*> Event:: get_registered_events ()
{
    return registered;
}




