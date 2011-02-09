#ifndef __ERICA_UNIQUE_HPP__
#define __ERICA_UNIQUE_HPP__

#include <set>

namespace erica {


/**
 * アクターIDを管理するクラス. インスタンス化はできない.
 * （メモ）これは単なるユニークIDの管理クラスなのでUniqueIDの方がいいか？
 */
class UniqueID
{
public:

    /**
     * ユニークIDを生成するコンストラクタ.
     * IDの範囲は[start,end). (startを含みendを含まない)
     * @param[in] start  開始ID
     * @param[in] end    終了ID
     */
    UniqueID (int start, int end);

    /**
     * デストラクタ.
     */
    ~UniqueID ();

    /**
     * ユニークなアクターIDを取得する.
     * 取得したアクターIDはrelease_unique_actor_id()を使って開放しなければならない。
     * @return アクターID.
     */
    int get () const;

    /**
     * ユニークなアクターIDを開放する.
     * @param[in] id  アクターID.
     */
    void release (int id);

private:

    /**
     * 開始ID.
     */
    int start;

    /**
     * 終了ID.
     */
    int end;

    /**
     * 次の空きID.
     */
    mutable int next;

    /**
     * 使用中のIDのリスト.
     */
    mutable std::set<int> ids;
};


} // namesapce erica {

#endif




