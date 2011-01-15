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
     * イベントの受け取り（処理）.
     * @param[in] event イベント
     * @return 処理したらtrue,処理しなかったらfalseを返す.
     */
    bool accept (const Event* event);

private:

    /**
     * イベントを処理する。リスナーの派生クラスはこの関数を再実装しなければならない.
     * @param[in] event イベント
     * @return 処理したらtrue,処理しなかったらfalseを返す.
     */
    virtual bool handle (const Event* event) = 0;
};


} // namespace erica {


#endif
