#include <unittest++/UnitTest++.h>
#include <iostream>
#include "Event.hpp"
#include "TestGameLogic.hpp"
#include "TestGameView.hpp"
#include "Actor.hpp"
using namespace std;
using namespace erica;


TEST (GameLogic_default_variables) 
{
    GameLogic* logic = new GameLogic;
    // 外からactrs, viewsの値を知る手段はない。
    // 何か作ろうか...
    
    delete logic;
}

TEST (GameLogic_set_variables) 
{
    GameLogic* logic = new GameLogic;
    Actor*     actr  = new Actor;
    GameView*  view  = new GameView (logic);
    
    // 外からactrs, viewsの値を知る手段はない。
    // セグ落ちしなければOK.

    logic->add_view (view);
    logic->remove_view (view);

    logic->add_actor (actr);
    logic->remove_actor (actr);

    delete logic;
    delete actr;
    delete view;
}


