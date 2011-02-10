#include <unittest++/UnitTest++.h>
#include <iostream>
#include "Exception.hpp"
using namespace std;
using namespace erica;


TEST (Exception_default_variables)
{
    Exception* e = new Exception ("FILE", "FUNCTION", "MESSAGE");
    
    CHECK_EQUAL ("FILE:FUNCTION MESSAGE", e.what());
}
