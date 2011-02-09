#include "UniqueID.hpp"
#include "Exception.hpp"
#include <climits>
using namespace std;
using namespace erica;


UniqueID:: UniqueID (int s, int e) : start(0), end(0), next(0)
{
    if (s < 0 || s >= INT_MAX) {
        throw Exception (__FILE__, __func__, "Start ID is invalid. s=%d.", s);
    }
    if (e < 0 || e >= INT_MAX) {
        throw Exception (__FILE__, __func__, "End ID is invalid. e=%d.", e);
    }
    if (s > e) {
        throw Exception (__FILE__, __func__, "Range[%d,%d) is invalid.", s, e);
    }
    
    start = s;
    end   = e;
    next  = start;
}

UniqueID:: ~UniqueID ()
{
}

int UniqueID:: get () const
{
    int id = 0;
    int i  = 0;
    for (i = 0; i < (end-start); i++) {
        if (ids.find (next) == ids.end()) {
            id = next;
            ids.insert (next++);
            break;
        } else {
            next++;
        }
        next = start + (next - start) % (end - start);
    }
    if (i == (end-start)) {
        throw Exception (__FILE__, __func__, "UniqueID is exhausted.");
    }

    return id;
}


void UniqueID:: release (int id)
{
    if (id < start || id >= end) {
        throw Exception (__FILE__, __func__, "UniqueID is invalid, id=%d.", id);
    }
    ids.erase (id);
}



