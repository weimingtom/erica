#ifndef __ERICA_EVENT_HPP__
#define __ERICA_EVENT_HPP__

namespace erica {

/**
 * 
 */
class Event
{
    struct Entity {
        const char* name;
        void*       params;
        int         size;
    };

public:
    Event (const char* name, const void* params, int size);
    ~Event ();

    const char* name () const;

    const void* params () const;

    int size () const;

private:
    Entity e;
};


} // namespace erica {


#endif
