

#ifndef __TEST_EVENT_PARAMS__
#define __TEST_EVENT_PARAMS__

/**
 * イベント一覧
 * "ACTOR_STORE"     , 自分がアクターなら保存し、そうでないなら転送する。
 * "CONTROLLER_STORE", 自分がコントローラーなら保存し、そうでないなら転送する。
 */

struct ActorStoreParams {
    int id;
};

struct ControllerStoreParams {
    int id;
};




#endif

