#ifndef __ERICA_ACTOR_ID_MANAGER_HPP__
#define __ERICA_ACTOR_ID_MANAGER_HPP__

#include <set>

namespace erica {


/**
 * アクターIDを管理するクラス. インスタンス化はできない.
 * （メモ）これは単なるユニークIDの管理クラスなのでUniqueIDの方がいいか？
 */
class ActorIDManager
{
public:

    /**
     * ユニークなアクターIDを取得する.
     * 取得したアクターIDはrelease_unique_actor_id()を使って開放しなければならない。
     * @return アクターID.
     */
    static int get_unique_actor_id ();

    /**
     * ユニークなアクターIDを開放する.
     * @param[in] id  アクターID.
     */
    static void release_unique_actor_id (int id);

private:
    ActorIDManager ();

    /**
     * 次の空きアクターID.
     */
    static int           next;

    /**
     * 使用中のアクターIDのリスト.
     */
    static std::set<int> ids;
};


} // namesapce erica {

#endif




