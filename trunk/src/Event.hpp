#ifndef __ERICA_EVENT_HPP__
#define __ERICA_EVENT_HPP__

#include <map>

namespace erica {

/**
 * イベントを表現するクラス.
 */
class Event
{
    struct Entity {
        const char* name;
        void*       params;
        int         size;
    };

public:
    /**
     * コンストラクタ.
     * 1. イベント名はコピーされない。
     * 2. イベントパラメーターはコピーされる。
     * 従ってパラメーターはmemcpy()でコピー可能なPODでなければならない。
     * イベント名は大文字小文字を区別しない。また予め登録されたものでなければならない。
     * @param[in] name   このイベントの名前. 
     * @param[in] params イベントパラメーター.
     * @param[in] size   イベントパラメーターのサイズ.
     * @see regist()
     */
    Event (const char* name, const void* params, int size, int actor_id);

    /**
     * デストラクタ.
     */
    ~Event ();

    /**
     * このイベントの名前を取得する.
     * @return 名前.
     */
    const char* name () const;

    /**
     * このイベントのパラメーターを取得する.
     * @return パラメーター.
     */
    const void* params () const;

    /**
     * このイベントのパラメーターサイズを取得する.
     * @return パラメーターサイズ.
     */
    int size () const;

    /**
     * このイベントの一意なIDを取得する.
     * @return 一意なID.
     */
    int id () const;

    /**
     * このイベントを発行したアクターIDを取得する.
     * @return アクターID.
     */
    int get_actor_id () const;

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
    /**
     * イベントデータ.
     */
    Entity e;

    /**
     * 登録済みのイベントのリスト.
     */
    static std::map<int, const char*> registered;
};


} // namespace erica {


#endif
