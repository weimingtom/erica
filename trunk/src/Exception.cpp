#include "Exception.hpp"
using namespace erica;
using namespace std;

Exception:: Exception (const char* file, const char* func)
{
}

Exception:: ~Exception () throw()
{
}
    
const char* Exception:: what () throw()
{
    return 0;
}

