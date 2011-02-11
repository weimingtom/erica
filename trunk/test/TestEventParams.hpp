

#ifndef __TEST_EVENT_PARAMS__
#define __TEST_EVENT_PARAMS__

/**
 *
 */
struct TestEventParams
{
    TestEventParams (int i) : n(i) {};
    int n;
};

struct KeyPressedParams {
    unsigned char key;
};

struct ActorWalkParams {
    static const int LEFT    = 4;
    static const int RIGHT   = 6;
    static const int FORWARD = 8;
    static const int BACK    = 2;
    int           actor_id;
    unsigned char dir;    
};


#endif

