#include "TestGameView.hpp"
using namespace erica;
using namespace std;


TestGameView:: TestGameView (GameLogic* logic) : GameView(logic)
{
}

TestGameView:: ~TestGameView ()
{
}
 
void TestGameView:: update_impl (int msec)
{

}

bool TestGameView:: handle_impl (const Event* ev)
{
    // 今のところ
    // nothing to do.
    return false;
}

void TestGameView:: render_impl () const
{
}

