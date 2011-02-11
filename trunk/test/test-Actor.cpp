#include <unittest++/UnitTest++.h>
#include <iostream>
#include "TestActor.hpp"
using namespace std;
using namespace erica;

TEST (TestActor_default_variables)
{
    TestActor* actr1 = new TestActor;
    TestActor* actr2 = new TestActor;

    // アクターIDはデフォルトで自動発番のがつく
    // 多分連番...
    CHECK_EQUAL (actr2->get_actor_id(), actr1->get_actor_id()+1);
}

TEST (TestActor_set_variables)
{
    TestActor* actr = new TestActor;

    // ユーザー指定もできる
    actr->set_actor_id (100);

    CHECK_EQUAL (100, actr->get_actor_id());

}

