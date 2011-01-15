#ifndef __ERICA_GAME_VIEW_HPP__
#define __ERICA_GAME_VIEW_HPP__

#include <map>
#include "IEventListener.hpp"


namespace erica {

    class GameLogic;
    class EventQueue;
    class Controller;

/**
 * 表示とコントロールを行う「ゲームビュー」クラス.
 */
    class GameView
{
public:
    /**
     * コンストラクタ。１つのビューにつき１つのロジックが必ず関連付けられる.
     * @param[in] logic 関連付けるゲームロジック.
     */
    GameView (GameLogic* logic);

    /**
     * デストラクタ.
     */
    virtual ~GameView ();

    /**
     * このビューにクロックを供給する.
     * @param[in] msec 秒数をmsecで指定する.
     */
    void tick (int msec);

    /**
     * このビューのイベントキュー(in)にイベントを入れる.
     * イベントは次のtick()が呼ばれたタイミングで処理される。
     * @param[in] event イベント
     */
    void enqueue (const Event* event);



private:

    /**
     * このビューを更新する。ビューの派生クラスはこの関数を再実装しなければならない.
     * @param[in]  msec  秒数をmsecで指定する.
     */
    virtual void update (int msec) = 0;

    /**
     * このビューを描画する。ビューの派生クラスはこの関数を再実装しなければならない.
     */
    virtual void render () const = 0;

protected:

    /**
     * このビューがイベントを受け取るときのキュー.
     */
    EventQueue* in;

    /**
     * このビューがイベントを出力するときのキュー.
     */
    EventQueue* out;

    /**
     * このビューが関連付けられているロジック.
     */
    GameLogic*  logic;


protected:

    /**
     * このビューが保持しているコントローラー.
     */
    std::map<int, Controller*> ctrls;
};


} // namespace erica {

#endif




