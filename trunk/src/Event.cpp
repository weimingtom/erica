#include "Event.hpp"
#include <iostream>
using namespace erica;
using namespace std;

Event:: Event (const char* name, const void* params, int size, int actor_id)
{
}

Event:: ~Event ()
{
}

const char* Event:: name () const
{
    return NULL;
}

const void* Event:: params () const
{
    return NULL;
}


int Event:: size () const
{
    return 0;
}


int Event:: id () const
{
    return 0;
}

void Event:: regist (const char* name, int id)
{
}




