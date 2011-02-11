#include "TestGameLogic.hpp"
using namespace erica;
using namespace std;



TestGameLogic:: TestGameLogic ()
{
}

TestGameLogic:: ~TestGameLogic ()
{
}

void TestGameLogic:: load_game_impl (const char* ini_file)
{
}

void TestGameLogic:: update_impl (int msec)
{
}

bool TestGameLogic:: end_of_game_impl () const
{
    return false;
}

bool TestGameLogic:: handle_impl (const Event* event)
{
    // nothing to do
    return false;
}
