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
     * イベントを処理する。
     * @param[in] event イベント
     * @return 処理したらtrue,処理しなかったらfalseを返す.
     */
    bool handle (const Event* event);

protected:

    /**
     * handle()関数の再実装。リスナーの派生クラスはこの関数を再実装しなければならない.
     * @param[in] event イベント
     * @return 処理したらtrue,処理しなかったらfalseを返す.
     */
    virtual bool handle_impl (const Event* event) = 0;


};


} // namespace erica {


#endif
