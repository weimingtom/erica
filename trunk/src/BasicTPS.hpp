#ifndef __ERICA_BASIC_TPS_HPP__
#define __ERICA_BASIC_TPS_HPP__

#include <vector>
#include "GameLogic.hpp"
#include "IEventListener.hpp"


namespace erica {

    class BasicTPSData;
    class Event;
    class Actor;

/**
 * 
 */
class BasicTPS : public GameLogic, public IEventListener
{
public:
    BasicTPS ();
    virtual ~BasicTPS ();

protected:
    virtual void create_game (const char* init_file);

    virtual void update (int msec);

    virtual bool handle (const Event* event);    
   
private:
    std::vector<Actor*> actors;
    BasicTPSData*        data;
};


} // namespace erica {

#endif
