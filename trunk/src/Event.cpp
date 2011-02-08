#include "Event.hpp"
#include <iostream>
using namespace erica;
using namespace std;


Event:: Event (const char* name, const void* params, int size)
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



