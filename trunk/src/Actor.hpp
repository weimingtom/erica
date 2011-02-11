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
     */
    Actor ();

    /**
     * デストラクタ.
     */
    virtual ~Actor ();

    /**
     * このアクターにイベントキューを関連付ける.
     * @param[in] in   このアクターから出力されるイベントの入力キュー(in).
     * @param[in] out  このアクターから出力されるイベントの入力キュー(out).
     */
    void set_event_queue (EventQueue* in, EventQueue* out);

    /**
     * このアクターに関連付けられているイベントキューの取得.
     * @param[in] dir  このイベントキューの方向.
     * @return イベントキュー(out)
     */
    EventQueue* get_event_queue (int dir) const;

    /**
     * アクターIDの取得。アクターIDはユニークIDが自動で発番される.
     * @return アクターID.
     */
    int get_actor_id () const;

    
    /**
     * アクターIDを設定する。IDを自動発番でなくユーザーがセットする場合に使用する.
     * @param[in] id  アクターID.
     */
    void set_actor_id (int id);

    /**
     * このアクターを更新する.
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
     * set_event_queue()の実装関数。アクターの派生クラスはこの関数を再実装しなければならない.
     */
    virtual void set_event_listener_impl () = 0;


protected:

    /**
     * このアクターからイベントを出力するときの出力先(int).
     */
    EventQueue* in;

    /**
     * このアクターからイベントを出力するときの出力先(out).
     */
    EventQueue* out;
    
    /**
     * このアクターの一意な識別子.
     * 自動発番されるがユーザーが後から指定しても良い。
     */
    int actor_id;

    /**
     * このアクターの名前(基本的に使用しない。デバッグ用)
     */
    const char* name;
};


} // namespace erica {

#endif
