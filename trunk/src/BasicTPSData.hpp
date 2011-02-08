#ifndef __BASIC_TPS_DATA_HPP__
#define __BASIC_TPS_DATA_HPP__

namespace erica {

/**
 * 基本的なTPS(サード・パーソン・シューティング)のデータ管理を行うロジッククラス.
 */
class BasicTPSData
{
    static const int RUNNING     = 0;
    static const int END_OF_GAME = 1;


public:
    /**
     * コンストラクタ.
     */
    BasicTPSData ();

    /**
     * デストラクタ.
     */
    ~BasicTPSData ();

    /**
     * 終了処理を行う.
     */
    void quit ();

    /**
     * 終了状態を取得する.
     * @return 終了していればtrue, そうでなければfalse.
     */
    bool end_of_game () const;

    /**
     * 状態を取得する.
     * @return 状態. RUNNINGまたはEND_OF_GAME.
     */
    int get_state () const;

private:
    /**
     * 現在のゲームの状態.
     */
    int state;

};


} // namespace erica {

#endif
