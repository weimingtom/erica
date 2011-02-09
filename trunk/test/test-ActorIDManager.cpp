#include <unittest++/UnitTest++.h>
#include <iostream>
#include "ActorIDManager.hpp"
using namespace std;
using namespace erica;

TEST (ActorIDManger_get_unique_actor_id)
{
    int id1;
    int id2;

    id1 = ActorIDManager::get_unique_actor_id();
    id2 = ActorIDManager::get_unique_actor_id();

    //　自動発行だが連番.
    CHECK_EQUAL (id2, id1+1);
}

TEST (ActorIDManager_release_unique_actor_id)
{
    int id1;
    int id2;

    id1 = ActorIDManager::get_unique_actor_id();
    id2 = ActorIDManager::get_unique_actor_id();

    ActorIDManager:: release_unique_actor_id (id1);
    ActorIDManager:: release_unique_actor_id (id2);

    // エラーが起きなければそれでいい.
}
