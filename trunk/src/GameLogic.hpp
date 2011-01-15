#ifndef __ERICA_GAME_LOGIC_HPP__
#define __ERICA_GAME_LOGIC_HPP__

#include <vector>
namespace erica {

class GameView;
class EventQueue;
    class Event;


/**
 * ゲーム本体の進行管理を行う「ゲームロジック」クラス.
 */
class GameLogic
{
public:
    /**
     * コンストラクタ.
     */
    GameLogic ();

    /**
     * デストラクタ.
     */
    virtual ~GameLogic ();

    /**
     * ゲームのロード.
     * この関数の呼び出しは必須。
     * @param[in] ini_file ロードする初期化ファイルを指定する.
     */
    void load_game (const char* ini_file);

    /**
     * このロジックにクロックを供給する.
     * @param[in] msec 秒数をmsecで指定する.
     */
    void tick (int msec);

    /**
     * このロジックの持つビューを取得する.
     * @param[in] id  ビュー番号。通常は0がプレイヤービュー.
     */
    GameView* get_game_view (int id) const;

    /**
     * このロジックのイベントキュー(in)にイベントを入れる.
     * イベントは次のtick()が呼ばれたタイミングで処理される。
     * @param[in] event イベント
     */
    void enqueue (const Event* event);

private:
    /**
     * このロジックを更新する。ロジックの派生クラスはこの関数を再実装しなければならない.
     * @param[in]  msec  秒数をmsecで指定する.
     */
    virtual void update (int msec) = 0;
    
    /**
     * このロジックを構築する。ロジックの派生クラスはこの関数を再実装しなければならない.
     * @param[in]  ini_file 初期化ファイル.
     * @see load_game()
     */
    virtual void create_game (const char* ini_file) = 0;

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
     * このロジックが関連付けられているビュー.
     * 0個以上。
     */
    std::vector<GameView*> views;

    // この他にYAML形式で書いたゲーム情報をロードした結果を
    // まるごと全部保管しておく必要がある。
};


} // namespace erica {

#endif
