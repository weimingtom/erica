#include <unittest++/UnitTest++.h>
#include <iostream>
#include "Event.hpp"
#include "TestGameLogic.hpp"
#include "TestGameView.hpp"
#include "Controller.hpp"
using namespace std;
using namespace erica;


TEST (GameView_default_variables)
{
    GameLogic* logic = new GameLogic;
    GameView* view = new GameView (logic);

    delete view;
}

TEST (GameView_set_variables)
{
    GameLogic* logic = new GameLogic;
    GameView* view = new GameView (logic);
    Controller* ctrl = new Controller;

    view->add_controller (ctrl);
    view->remove_controller (ctrl);

    delete ctrl;
    delete view;
}


