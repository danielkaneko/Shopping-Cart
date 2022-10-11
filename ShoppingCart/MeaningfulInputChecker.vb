Imports Fhi.Fw
''' <summary>
''' 意味のある入力かどうかチェックする
''' </summary>
Public Class MeaningfulInputChecker

    ''' <summary>
    ''' 入力されたコマンドがlistである
    ''' </summary>
    Public IsListCommand As Boolean = False
    ''' <summary>
    ''' 意味のある入力がされるまで待機する
    ''' </summary>
    ''' <returns>入力されたコマンド</returns>
    Public Function WaitForMeaningfulInput() As String
        Dim listShowCommand As String
        Do
            listShowCommand = Console.ReadLine()
        Loop While Not ValidateInputCommand(listShowCommand)
        Return listShowCommand
    End Function

    ''' <summary>
    ''' 入力された内容がコマンドとして正しいかチェックする
    ''' </summary>
    ''' <param name="inputCommand">入力された文字列</param>
    ''' <returns>コマンドとして正しければTrue、そうでなければFalse</returns>
    Public Function ValidateInputCommand(inputCommand As String) As Boolean
        If StringUtil.IsNotEmpty(inputCommand) AndAlso "list".Equals(inputCommand) Then
            IsListCommand = True
            Return IsListCommand
        End If
        Console.WriteLine("入力内容が正しくありません。")
        Return False
    End Function
End Class
