

#ifndef __TEST_EVENT_PARAMS__
#define __TEST_EVENT_PARAMS__

/**
 * イベント一覧
 * || 名前               || 処理する人 ||
 * || "ACTOR_STORE"      || Actor     ||
 * || "CONTROLLER_STORE" || GameLogic ||
 * ||                    || Controller||
 */

struct LogicStoreParams {
    int id;
};

struct ActorStoreParams {
    int id;
};

struct ViewStoreParams {
    int id;
};

struct ControllerStoreParams {
    int id;
};




#endif

