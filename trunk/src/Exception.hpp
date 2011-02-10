#ifndef __ERICA_EXCEPTION_HPP__
#define __ERICA_EXCEPTION_HPP__

#include <exception>
#include <string>

namespace erica {

/**
 * 例外クラス.
 */
class Exception : public std::exception
{
public:
    /**
     * コンストラクタ.
     * @param[in] file    ファイル名を指定する. 通常は__FILE__.
     * @param[in] func    関数名を指定する.通常は__func__.
     * @param[in] format  printfと同じ文字列フォーマット.
     * @param[in] ...     任意引数.
     */
    explicit Exception (const char* file, const char* func, const char* format, ...);

    /**
     * デストラクタ.
     */
    virtual ~Exception () throw();
    
    /**
     * エラーメッセージの表示.
     * @return エラーメッセージ.
     */
    virtual const char* what () const throw();

private:
    std::string msg;
};


} // namespace erica {


#endif
