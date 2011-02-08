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
     *
     */
    void quit ();

    /**
     *
     */
    bool end_of_game () const;

    /**
     *
     */
    int get_state () const;

private:
    int state;

};


} // namespace erica {

#endif
