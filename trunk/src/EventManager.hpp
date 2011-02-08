#ifndef __ERICA_EVENT_MANAGER_HPP__
#define __ERICA_EVENT_MANAGER_HPP__

#include <map>

namespace erica {

/**
 *
 */
class EventManager
{
public:

    /**
     * イベントを登録する。有効なイベントはあらかじめこの関数を使って登録されていなければならない.
     * @param[in] name  イベント名.
     * @param[in] id    一意なID.
     * @return 一意なID.
     */
    static void regist (int id, const char* name);

    /**
     * 登録されているイベントのリストを取得する。
     * @return イベントリスト
     */
    static std::map<int, const char*> get_registered_events ();

private:
    EventManager ();

    /**
     * 登録済みのイベントのリスト.
     */
    static std::map<int, const char*> registered;

};


}

#endif
