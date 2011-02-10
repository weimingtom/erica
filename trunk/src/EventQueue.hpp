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
     * IN方向のキューを表す定数値.
     */
    static const int IN  = 1;

    /**
     * OUT方向のキューを表す定数値.
     */
    static const int OUT = 2;

    /**
     * コンストラクタ.
     */

    EventQueue ();

    /**
     * デストラクタ.
     */
    ~EventQueue ();

    /**
     * このイベントキューに入っているイベントを処理する.
     */
    void trigger ();

    /**
     * このイベントキューの末尾にイベントを追加する.
     * 挿入されたイベントはtrigger()関数が呼ばれたタイミングで処理された後、内部で削除される。
     * @param[in] event イベント.
     */
    void enqueue (const Event* event);

    /**
     * このイベントキューの先頭からイベントを1つ取得して削除する.
     * @return イベント.
     */
    const Event* dequeue ();

    /**
     * イベントを監視するリスナーを追加する. 既に登録済みの場合は何もしない。
     * @param[in] listener   追加したいリスナー.
     * @param[in] event_name 監視するイベントの名前.
     */
    void add_listener (IEventListener* listner, const char* event_name);

    /**
     * このイベントキューからリスナーを削除する.
     * リスナーが複数のイベントを監視していた場合、すべて解除される。
     * @param[in] listener 削除したいリスナー.
     */
    void remove_listener (const IEventListener* listener);

    /**
     * キューに入っているイベントをすべて削除する.
     */
    void clear ();

    /**
     * キューに入っているイベントの数を取得する.
     * @return イベント数.
     */
    int size () const;


private:
    /**
     * 未処理のイベントのリスト.
     */
    std::list<const Event*>  events;

    /**
     * イベントのリスナー.
     */
    std::multimap<unsigned long long, IEventListener*> listeners;
};


} // namespace erica {

#endif


