#ifndef __ERICA_DEFINITIONS_HPP__
#define __ERICA_DEFINITIONS_HPP__


namespace erica {

/**
 * Ericaエンジンで使用する基本的な定数の定義.
 * 値の定義は[MIN,MAX). （MINを含みMAXを含まない）
 * 従ってEVENT_ID_MAXは有効なイベントIDではない。
 */


/**
 * ID一覧。すべてのIDはエンジン内部でユニークでなければならない。
 */
const int ACTOR_ID_UNDEFINED = 0;
const int ACTOR_ID_USER_MIN  = 1;
const int ACTOR_ID_USER_MAX  = 65536*1-1;
const int ACTOR_ID_USER_NUM  = ACTOR_ID_USER_MAX - ACTOR_ID_USER_MIN;
const int ACTOR_ID_AUTO_MIN  = 65536*1;
const int ACTOR_ID_AUTO_MAX  = 65536*16-1;
const int ACTOR_ID_AUTO_NUM  = ACTOR_ID_AUTO_MAX - ACTOR_ID_AUTO_MIN;
const int EVENT_ID_MIN       = 65536*16;
const int EVENT_ID_MAX       = 65536*32-1;



} // namespace erica {


#endif




