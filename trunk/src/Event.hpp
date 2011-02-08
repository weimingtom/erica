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
    Event (const char* name, const void* params, int size);

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

private:
    /**
     * イベントデータ.
     */
    Entity e;

};


} // namespace erica {


#endif
