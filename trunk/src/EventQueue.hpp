#ifndef __ERICA_EVENT_QUEUE_HPP__
#define __ERICA_EVENT_QUEUE_HPP__

#include <list>
#include <map>

namespace erica {
class Event;
class IEventListener;

/**
 * イベントキューを表すクラス.
 */
class EventQueue
{
public:
    /**
     * コンストラクタ.
     */

    EventQueue ();

    /**
     * デストラクタ.
     */
    ~EventQueue ();

    /**
     * このイベントキューにクロックを供給する.
     * @param[in] msec 秒数をmsecで指定する.
     */
    void tick (int msec);

    /**
     * このイベントキューにイベントを追加する.
     * 挿入されたイベントは処理された後、内部で削除される。
     * @param[in] event イベント.
     */
    void enqueue (const Event* event);

    /**
     * このイベントキューにリスナーを追加する.
     * @param[in] listener 追加したいリスナー.
     * @param[in] listener 監視するイベントの名前.
     */
    void add_listener (IEventListener* listner, const char* event_name);

    /**
     * このイベントキューからリスナーを削除する.
     * リスナーが複数のイベントを監視していた場合、すべて解除される。
     * @param[in] listener 削除したいリスナー.
     */
    void remove_listener (const IEventListener* listner);

private:
    std::list<const Event*>  events;
    std::multimap<int, IEventListener*> listners;
};


} // namespace erica {

#endif


