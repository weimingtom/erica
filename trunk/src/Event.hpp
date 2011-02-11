#ifndef __ERICA_EVENT_HPP__
#define __ERICA_EVENT_HPP__

#include <map>
#include <iosfwd>

namespace erica {

/**
 * イベントを表現するクラス.
 */
class Event
{
    struct Entity {
        Entity (const char* n, void* p, int s) : name(n), params(p), size(s), id(0) {};
        const char* name;
        void*       params;
        int         size;
        unsigned long long id;
    };

public:
    /**
     * コンストラクタ.<BR>
     * 1. イベント名はコピーされない。<BR>
     * 2. イベントパラメーターはコピーされる。<BR>
     * 従ってパラメーターはmemcpy()でコピー可能なPODでなければならない。
     * イベント名は大文字小文字を区別しない。IDは文字列からハッシュ関数を使って自動で決定される。
     * @param[in] name   このイベントの名前. 
     * @param[in] params イベントパラメーター.
     * @param[in] size   イベントパラメーターのサイズ[0,65535].
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
    unsigned long long id () const;

    /**
     * このクラスの情報を内部表示する。デバッグ用。
     * param[in]  out  出力先ストリーム.
     */
    std::ostream& print (std::ostream& out) const;

private:
    /**
     * イベントデータ.
     */
    Entity e;

};


} // namespace erica {

/**
 * このクラスの情報を内部表示する。デバッグ用。
 * @param[in]  out   出力先ストリーム.
 * @param[in]  event イベント
 */
std::ostream& operator<< (std::ostream& out, const erica::Event& ev);


/**
 * イベントの種類（名前)が等しければtrueを返す比較演算子. 大文字小文字は区別しない.
 * @param[in]  event  イベント
 * @param[in]  name   名前文字列
 */
bool operator== (const erica::Event& ev, const char* name);


/**
 * イベントの種類（名前)が等しければtrueを返す比較演算子. 大文字小文字は区別しない.
 * @param[in]  event  イベント
 * @param[in]  name   名前文字列
 */
bool operator== (const char* name, const erica::Event& ev);

#endif
