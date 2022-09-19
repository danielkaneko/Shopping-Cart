''' <summary>
''' 文字列を分割する
''' </summary>
Public Class StringExtraction
    ''' <summary>
    ''' 文字列を配列に分ける
    ''' </summary>
    ''' <param name="inputContent">分ける文字列</param>
    ''' <param name="delimiter">区切り文字</param>
    ''' <returns>配列に分けられた文字</returns>
    Public Shared Function ExtractionString(inputContent As String, delimiter As String) As String()
        Return inputContent.Split(CChar(delimiter))
    End Function
End Class
