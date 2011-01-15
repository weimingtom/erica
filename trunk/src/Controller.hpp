#ifndef __ERICA_CONTROLLER_HPP__
#define __ERICA_CONTROLLER_HPP__

namespace erica {

class EventQueue;

/**
 * キー入力を処理するコントローラークラス.
 */
class Controller
{
public:
    /**
     * コンストラクタ.
     * @param[in] out  このコントローラーから出るイベントの出力先キュー.
     */
    Controller (EventQueue* out);

    /**
     * デストラクタ.
     */
    virtual ~Controller ();

protected:

    /**
     * このコントローラーにアクターを関連付ける.
     * @param[in] id 関連付けるアクターID.
     */
    void attach_actor (int id);

    /**
     * このコントローラーからイベントを出力するときの出力先.
     */
    EventQueue* queue;

    /**
     * このコントローラーが関連付けられているアクターID.
     */
    int actor_id;
};


} // namespace erica {

#endif
