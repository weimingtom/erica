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
     * @param[in] file  ファイル名を指定する.
     * @param[in] func  関数名を指定する.
     */
    Exception (const char* file, const char* func);

    /**
     * デストラクタ.
     */
    virtual ~Exception () throw();
    
    /**
     * エラーメッセージの表示.
     */
    virtual const char* what () throw();
};


} // namespace erica {


#endif
