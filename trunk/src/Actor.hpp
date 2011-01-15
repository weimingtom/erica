#ifndef __ERICA_ACTOR_HPP__
#define __ERICA_ACTOR_HPP__

namespace erica {

class EventQueue;


/**
 * ゲームの構成要素を表現するアクタークラス.
 */
class Actor
{
public:
    /**
     * コンストラクタ.
     * @param[in] out  このアクターから出力されるイベントの出力先キュー.
     */
    Actor (EventQueue* out);

    /**
     * デストラクタ.
     */
    virtual ~Actor ();

    /**
     * このアクターにクロックを供給する.
     * @param[in] msec  秒数をmsecで指定する.
     * @see update()
     */
    void tick (int msec);

    /**
     * アクターIDの取得。アクターIDはユニークIDが自動で発番される.
     * @return アクターID.
     */
    int get_actor_id () const;

private:
    /**
     * このアクターを更新する。アクターの派生クラスはこの関数を再実装しなければならない.
     * @param[in]  msec  秒数をmsecで指定する.
     */
    virtual void update (int msec) = 0;

    /**
     * 一意なIDを取得する.
     */
    static int get_unique_id ();

protected:
    /**
     * このアクターからイベントを出力するときの出力先.
     */
    EventQueue* out;

    
    /**
     * このアクターの一意な識別子.
     */
    int actor_id;

    /**
     * このアクターの名前(基本的に使用しない。デバッグ用)
     */
    const char* name;
};


} // namespace erica {

#endif
