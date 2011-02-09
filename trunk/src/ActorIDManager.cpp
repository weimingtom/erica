#include "ActorIDManager.hpp"
#include "Definitions.hpp"
#include "Exception.hpp"
using namespace std;
using namespace erica;

set<int> ActorIDManager:: ids;
int      ActorIDManager:: next = ACTOR_ID_AUTO_MIN;

int ActorIDManager:: get_unique_actor_id ()
{
    int id = 0;
    int i  = 0;
    for (i = 0; i < ACTOR_ID_AUTO_NUM; i++) {
        if (ids.find (next) == ids.end()) {
            id = next;
            ids.insert (next++);
            break;
        } else {
            next++;
        }
        next = ACTOR_ID_AUTO_MIN + (next - ACTOR_ID_AUTO_MIN) % ACTOR_ID_AUTO_NUM;
    }
    if (i == ACTOR_ID_AUTO_NUM) {
        throw Exception (__FILE__, __func__, "Unique ActorID is exhausted.");
    }

    return id;
}


void ActorIDManager:: release_unique_actor_id (int id)
{
    if (id < ACTOR_ID_AUTO_MIN || id >= ACTOR_ID_AUTO_MAX) {
        throw Exception (__FILE__, __func__, "ActorID is invalid, id=%d.", id);
    }
    ids.erase (id);
}



