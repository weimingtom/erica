#ifndef __ERICA_IEVENT_LISTENER_HPP__
#define __ERICA_IEVENT_LISTENER_HPP__

namespace erica {

    class Event;

/**
 * イベントを受け取るリスナーための純粋インターフェース・クラス。
 * すべてのイベントリスナーはこのインターフェースを継承する.
 */
class IEventListener
{
public:
    /**
     * コンストラクタ.
     */
    IEventListener ();

    /**
     * デストラクタ.
     */
    virtual ~IEventListener ();

    /**
     * イベントを処理するコールバック関数.
     * ユーザーはこの関数を実装することでイベントに応じた処理が可能になる。
     * @param[in] event イベント
     * @return 処理したらtrue,処理しなかったらfalseを返す.
     */
    bool handle (const Event* event);

protected:

    /**
     * handle()関数の実装。リスナーの派生クラスはこの関数を再実装しなければならない.
     * この関数でtrueを返す場合はeventをdeleteする責任がある。
     * @param[in] event イベント
     * @return 処理したらtrue,処理しなかったらfalseを返す.
     */
    virtual bool handle_impl (const Event* event) = 0;

};


} // namespace erica {


#endif
