#ifndef __ERICA_CONTROLLER_HPP__
#define __ERICA_CONTROLLER_HPP__

namespace erica {

class EventQueue;

/**
 * キー入力を処理するコントローラークラス.
 */
class Controller
{
public:
    /**
     * コンストラクタ.
     */
    Controller ();

    /**
     * デストラクタ.
     */
    virtual ~Controller ();

    /**
     * このコントローラーにイベントキュー(in,out)を関連付ける.
     * @param[in] in   関連付けるイベントキュー(in).
     * @param[in] out  関連付けるイベントキュー(out).
     */
    void set_event_queue (EventQueue* in, EventQueue* out);

    /**
     * このコントローラーに関連付けられているイベントキューを取得する.
     * @param[in] dir  このイベントキューの方向.
     * @return アクターID.
     */
    EventQueue* get_evnet_queue (int dir) const;

    /**
     * このコントローラーに関連付けられているアクターIDを取得する.
     * @return アクターID.
     */
    int get_actor_id () const;

    /**
     * このコントローラーにアクターを関連付ける.
     * @param[in] id 関連付けるアクターID.
     */
    void set_actor_id (int id);

    /**
     * このコントローラーを更新する.
     * @param[in]  msec  秒数をmsecで指定する.
     */

    void update (int msec);


protected:

    /**
     * update()の実装関数。コントローラーの派生クラスはこの関数を再実装しなければならない.
     * @param[in]  msec  秒数をmsecで指定する.
     */
    virtual void update_impl (int msec) = 0;

    /**
     * set_event_queue()の実装関数。コントローラーの派生クラスはこの関数を再実装しなければならない.
     */
    virtual void set_event_queue_impl (EventQueue* in, EventQueue* out) = 0;

    /**
     * このコントローラーが監視するイベントキュー(in).
     */
    EventQueue* in;

    /**
     * このコントローラーが出力するイベントキュー(out).
     */
    EventQueue* out;

private:
    /**
     * このコントローラーが関連付けられているアクターID.
     */
    int actor_id;
};


} // namespace erica {

#endif
