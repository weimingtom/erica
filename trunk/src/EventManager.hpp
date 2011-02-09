#ifndef __ERICA_EVENT_MANAGER_HPP__
#define __ERICA_EVENT_MANAGER_HPP__

#include <map>

namespace erica {

/**
 * イベントの名前を管理するクラス.インスタンス化はできない。
 */
class EventManager
{
public:

    /**
     * イベントを登録する。有効なイベントはあらかじめこの関数を使って登録済みでなければならない.
     * @param[in] id    一意なID.
     * @param[in] name  一意なイベント名.
     */
    static void regist (int id, const char* name);

    /**
     * 登録されているイベントのリストを取得する。
     * @return イベントリスト.
     */
    static const std::map<const char*, int> get_registered_events ();

    /**
     * イベント名が登録済みかどうか検索する.
     * @param[in] name  イベント名.
     * @return イベントID. 登録されていない場合は0が返る.
     */
    static int find (const char* name);

private:
    EventManager ();

    /**
     * 登録済みのイベントのリスト.
     */
    static std::map<const char*, int> registered;

};


} // namesapce erica {

#endif
