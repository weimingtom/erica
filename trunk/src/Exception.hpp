#ifndef __ERICA_EXCEPTION_HPP__
#define __ERICA_EXCEPTION_HPP__

#include <exception>

namespace erica {

/**
 * 例外クラス.
 */
class Exception : public std::exception
{
public:
    /**
     * コンストラクタ.
     * @param[in] file  ファイル名を指定する. 通常は__FILE__.
     * @param[in] func  関数名を指定する.通常は__func__.
     */
    Exception (const char* file, const char* func);

    /**
     * デストラクタ.
     */
    virtual ~Exception () throw();
    
    /**
     * エラーメッセージの表示.
     * @return エラーメッセージ.
     */
    virtual const char* what () throw();
};


} // namespace erica {


#endif
