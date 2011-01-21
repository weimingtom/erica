#include "BasicActor.hpp"
#include "EventQueue.hpp"
using namespace erica;
using namespace std;


BasicActor:: BasicActor (EventQueue* in, EventQueue* out) : Actor(in, out)
{
}

BasicActor:: ~BasicActor()
{
}

bool BasicActor:: handle (const Event* event)
{
    return false;
}


void BasicActor:: update (int msec)
{

}
