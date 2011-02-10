#include "Exception.hpp"
#include <cstdio>
#include <cstdarg>
using namespace erica;
using namespace std;

const int size = 1024;

Exception:: Exception (const char* file, const char* func, const char* format, ...)
{
    char buf[size];
    va_list args;
    va_start (args, format);
    vsnprintf (buf, size, format, args);
    va_end (args);
    msg = string(file) + ":" + string(func) + " " + string(buf);
}

Exception:: ~Exception () throw()
{
}
    
const char* Exception:: what () const throw()
{
    return msg.c_str();
}

