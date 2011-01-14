#include "BasicActor.hpp"
#include "EventQueue.hpp"

using namespace erica;
using namespace std;


BasicActor:: BasicActor (EventQueue* out) : Actor(out)
{
}

BasicActor:: ~BasicActor()
{
}

void BasicActor:: update (int msec)
{

}

bool BasicActor:: handle (const Event* event)
{
    return false;
}

