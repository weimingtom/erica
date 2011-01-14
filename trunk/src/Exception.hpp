#ifndef __ERICA_EXCEPTION_HPP__
#define __ERICA_EXCEPTION_HPP__

#include <exception>

namespace erica {

/**
 *
 */
class Exception : public std::exception
{
public:
    Exception (const char* file, const char* func);
    virtual ~Exception () throw();
    
    virtual const char* what () throw();
};


} // namespace erica {


#endif
