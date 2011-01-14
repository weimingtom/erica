#include "BasicController.hpp"
#include "EventQueue.hpp"
using namespace erica;
using namespace std;

BasicController:: BasicController (EventQueue* out) : Controller(out)
{
}

BasicController:: ~BasicController ()
{
}

bool BasicController:: handle (const Event* event)
{
    return false;
}


