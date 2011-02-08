#include "EventManager.hpp"
#include <iostream>
using namespace std;
using namespace erica;

std::map<int, const char*> EventManager::registered;


void EventManager:: regist (int id, const char* name)
{

}

map<int, const char*> EventManager:: get_registered_events ()
{
    return registered;
}




