#ifndef __ERICA_BASIC_EVENT_PARAMS_HPP__
#define __ERICA_BASIC_EVENT_PARAMS_HPP__

namespace erica {

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

struct ActorSpawnParams {
    const char* name;        // インスタンス名
    const char* class_name;  // クラス名
};

struct ActorDieParams {
    int actor_id;
};

struct ActorShootParams {
    int actor_id;
};

struct SceneNodeCreateParams {
    int         actor_id;
    const char* controller;        // コントローラークラスの名前
    const char* representation;    // 描画に使うファイルの名前
};

struct SceneNodeDeleteParams {
    int actor_id;

};

struct SceneNodeMoveParams {
    int   actor_id;
    float x;         // ワールド座標、絶対値
    float y;         // ワールド座標、絶対値
    float z;         // ワールド座標、絶対値
};



} // namespace erica {


#endif
