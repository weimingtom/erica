#ifndef __ERICA_ACTOR_HPP__
#define __ERICA_ACTOR_HPP__

namespace erica {

class EventQueue;


/**
 * ゲームの構成要素を表現するアクタークラス.
 */
class Actor
{
public:
    /**
     * コンストラクタ.
     * @param[in] in   このアクターから出力されるイベントの入力キュー.
     * @param[in] out  このアクターから出力されるイベントの出力キュー.
     */
    Actor (EventQueue* in, EventQueue* out);

    /**
     * デストラクタ.
     */
    virtual ~Actor ();

    /**
     * アクターIDの取得。アクターIDはユニークIDが自動で発番される.
     * @return アクターID.
     */
    int get_actor_id () const;

    /**
     * このアクターを更新する。アクターの派生クラスはこの関数を再実装しなければならない.
     * @param[in]  msec  秒数をmsecで指定する.
     */
    void update (int msec);

private:

    /**
     * update()の実装関数。アクターの派生クラスはこの関数を再実装しなければならない.
     * @param[in]  msec  秒数をmsecで指定する.
     */
    virtual void update_impl (int msec) = 0;

    /**
     * 一意なIDを取得する.
     */
    static int get_unique_id ();


protected:

    /**
     * このアクターからイベントを出力するときの出力先(in).
     */
    EventQueue* in;

    /**
     * このアクターからイベントを出力するときの出力先(out).
     */
    EventQueue* out;


    
    /**
     * このアクターの一意な識別子.
     */
    int actor_id;

    /**
     * このアクターの名前(基本的に使用しない。デバッグ用)
     */
    const char* name;
};


} // namespace erica {

#endif
