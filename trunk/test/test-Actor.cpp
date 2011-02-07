#include <unittest++/UnitTest++.h>
#include <iostream>
#include "BasicActor.hpp"
using namespace std;
using namespace erica;

TEST (BasicActor_default_variables)
{
    BasicActor* actr1 = new BasicActor;
    BasicActor* actr2 = new BasicActor;

    // アクターIDはデフォルトで自動発番のがつく
    // 多分連番...
    CHECK_EQUAL (actr2->get_actor_id(), actr1->get_actor_id()+1);
}

TEST (BasicActor_set_variables)
{
    BasicActor* actr = new BasicActor;

    // ユーザー指定もできる
    actr->set_actor_id (100);

    CHECK_EQUAL (100, actr->get_actor_id());

}

