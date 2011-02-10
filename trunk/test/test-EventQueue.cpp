#include <unittest++/UnitTest++.h>
#include <iostream>
#include "Event.hpp"
#include "EventQueue.hpp"
#include "TestEventListener.hpp"
using namespace std;
using namespace erica;


TEST (EvnetQueue_default_variables) 
{
    EventQueue* in  = new EventQueue;

    CHECK_EQUAL (0, in->size());
    
    delete in;
}

TEST (EvnetQueue_destruct)
{
    // 未処理のイベントはEventQueueクラスが責任を持ってdeleteする.
    EventQueue* in  = new EventQueue;
    in->enqueue (new Event ("Hartmann", NULL, 0));
    delete in;
}

TEST (EventQueue_size_and_clear)
{
    EventQueue* in  = new EventQueue;
    Event*      ev = new Event ("Hartmann", NULL, 0);
    
    in->enqueue (ev);
    CHECK_EQUAL (1, in->size());

    in->clear ();
    CHECK_EQUAL (0, in->size());

    delete in;
    delete ev;
}


TEST (EventQueue_enqueue)
{
    EventQueue* in  = new EventQueue;
    Event*      ev1 = new Event ("Hartmann", NULL, 0);
    Event*      ev2 = new Event ("Barkhorn", NULL, 0);

    in->enqueue (ev1);
    in->enqueue (ev2);
    CHECK_EQUAL (2, in->size());
    
    const Event* ev3 = in->dequeue ();
    CHECK_EQUAL (ev1, ev3);
    CHECK_EQUAL (1, in->size());

    const Event* ev4 = in->dequeue ();
    CHECK_EQUAL (ev2, ev4);
    CHECK_EQUAL (0, in->size());

    const Event* ev5 = in->dequeue ();
    CHECK_EQUAL ((const Event*)NULL, ev5);
    CHECK_EQUAL (0, in->size());

    delete in;
    delete ev1;
    delete ev2;
}



TEST (EventQueue_add_and_remove_event_listener)
{
    EventQueue* in  = new EventQueue;
    TestEventListener* listener = new TestEventListener;

    in->add_listener (listener, "Hartmann");
    in->add_listener (listener, "Barkhorn");

    in->remove_listener (listener);

    // 実際にイベントを投げてみないと確認できない...

    delete in;
    delete listener;
}

TEST (EventQueue_trigger)
{
   EventQueue*        in       = new EventQueue;
   TestEventListener* listener = new TestEventListener;
   Event*             ev1      = new Event("Barkhorn", NULL, 0);
   Event*             ev2      = new Event("Hartmann", NULL, 0);
   Event*             ev3      = new Event("Wilcke"  , NULL, 0);


   in->enqueue (ev1);
   in->enqueue (ev2);
   in->enqueue (ev3);
   CHECK_EQUAL (3 , in->size());

   // イベント"Hartmann"をリスン
   in->add_listener (listener, "Hartmann");

   // トリガーするとイベント"Hartmann"がリスナーに送られる。
   in->trigger ();
   CHECK_EQUAL (2 , in->size());
   CHECK_EQUAL (1 , listener->events.size());
   CHECK_EQUAL (ev2, listener->events[0]);

   listener->events.clear();

   // もう一回トリガーしてもイベント"Hartmann"はない
   in->trigger ();
   CHECK_EQUAL (2 , in->size());
   CHECK_EQUAL (0 , listener->events.size());

   // リスナーを削除するとトリガーしても送られない
   in->remove_listener (listener);
   in->trigger ();
   CHECK_EQUAL (2, in->size());
   CHECK_EQUAL (0 , listener->events.size());
   
   // もう一度リスナーに登録してイベント"Barkhorn","Wilcke"をリスン.
   // トリガーすると"Barkhorn","Wilcke"がリスナーに送られる
   in->add_listener (listener, "Wilcke");
   in->add_listener (listener, "Barkhorn");
   in->trigger ();
   CHECK_EQUAL (0  , in->size());
   CHECK_EQUAL (2  , listener->events.size());
   CHECK_EQUAL (ev1, listener->events[0]);
   CHECK_EQUAL (ev3, listener->events[1]);

   delete in;
   delete listener;
   delete ev1;
   delete ev2;
   delete ev3;
}
