#include <unittest++/UnitTest++.h>
#include <iostream>
#include "UniqueID.hpp"
using namespace std;
using namespace erica;

TEST (ActorIDManger_get_unique_actor_id)
{
    UniqueID unique_id (100, 200);

    int id1 = unique_id.get ();
    int id2 = unique_id.get ();

    //　自動発行だが連番.
    CHECK_EQUAL (100, id1);
    CHECK_EQUAL (101, id2);
}

TEST (UniqueID_release_unique_actor_id)
{
    UniqueID unique_id (100, 200);
    
    int id1 = unique_id.get ();
    int id2 = unique_id.get ();

    // とりあえずエラーが起きなければそれでいい.
    unique_id.release (id1);
    unique_id.
release (id2);


}
