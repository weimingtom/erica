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
     * このビューのイベントキュー(in)にイベントを入れる.
     * イベントは次のupdate()が呼ばれたタイミングで処理される。
     * @param[in] event イベント
     */
    void enqueue (const Event* event);

    /**
     * このビューを更新する.
     * @param[in]  msec  秒数をmsecで指定する.
     */
    void update (int msec);

    /**
     * このビューを描画する.
     */
    void render () const;

    /**
     * コントローラを追加する.
     * @param[in] ctrl  コントローラー.
     */
    void add_controller (Controller* ctrl);

    /**
     * コントローラを削除する.
     * @param[in] ctrl  コントローラー.
     */
    void remove_controller (const Controller* ctrl);
    

protected:

    /**
     * update()関数の実装。ビューの派生クラスはこの関数を再実装しなければならない.
     * @param[in]  msec  秒数をmsecで指定する.
     */
    virtual void update_impl (int msec) = 0;

    /**
     * render()関数の実装。ビューの派生クラスはこの関数を再実装しなければならない.
     */
    virtual void render_impl () const = 0;

protected:

    /**
     * このビューがイベントを受け取るときのキュー(in).
     */
    EventQueue* in;

    /**
     * このビューがイベントを出力するときのキュー(out).
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




