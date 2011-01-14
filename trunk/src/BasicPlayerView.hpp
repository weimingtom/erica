#ifndef __ERICA_BASIC_PLAYER_VIEW_HPP__
#define __ERICA_BASIC_PLAYER_VIEW_HPP__

#include "GameView.hpp"
#include "IEventListener.hpp"

namespace erica {

    class Event;

/**
 *
 */
    class BasicPlayerView : public GameView, public IEventListener
{
public:
    BasicPlayerView ();
    virtual ~BasicPlayerView ();

private:
    virtual void update (int msec);

    virtual bool handle (const Event* event);

    virtual void render () const;
};

} // namespace erica {


#endif
