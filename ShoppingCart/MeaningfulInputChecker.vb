Imports Fhi.Fw
Imports System.Text.RegularExpressions
Public Class MeaningfulInputChecker

    ''' <summary>
    ''' 入力されたコマンドがlistである
    ''' </summary>
    Public IsList As Boolean = False
    ''' <summary>
    ''' 入力されたコマンドが--name 商品名である
    ''' </summary>
    Public IsSearchName As Boolean = False

    ''' <summary>
    ''' 意味のある入力がされるまで待機する
    ''' </summary>
    ''' <returns>入力されたコマンド</returns>
    Public Function WaitForMeaningfulInput() As String
        Dim listShowCommand As String
        Do
            listShowCommand = Console.ReadLine()
        Loop While Not CheckInputCommand(listShowCommand)
        Return listShowCommand
    End Function

    ''' <summary>
    ''' 入力された内容がコマンドとして正しいかチェックする
    ''' </summary>
    ''' <param name="inputCommand">入力された文字列</param>
    ''' <returns>コマンドとして正しければTrue、そうでなければFalse</returns>
    Public Function CheckInputCommand(inputCommand As String) As Boolean
        If StringUtil.IsNotEmpty(inputCommand) AndAlso "list".Equals(inputCommand) Then
            IsList = True
            Return IsList
        End If
        If StringUtil.IsNotEmpty(inputCommand) AndAlso Regex.IsMatch(inputCommand, "^(--name .*)") Then
            IsSearchName = True
            Return IsSearchName
        End If
        Console.WriteLine("入力内容が正しくありません。")
        Return False
    End Function
End Class
