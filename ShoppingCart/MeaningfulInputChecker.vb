Imports Fhi.Fw
Imports System.Text.RegularExpressions
''' <summary>
''' 意味のある入力かどうかチェックする
''' </summary>
Public Class MeaningfulInputChecker

    ''' <summary>
    ''' 入力されたコマンドがlistである
    ''' </summary>
    Public IsListCommand As Boolean = False
    ''' <summary>
    ''' 入力されたコマンドが--name 商品名である
    ''' </summary>
    Public IsSearchName As Boolean = False
    ''' <summary>
    ''' 入力されたコマンドが--price 価格-価格である
    ''' </summary>
    Public IsSearchPrice As Boolean = False
    ''' <summary>
    ''' 入力されたコマンドが--sort nameである
    ''' </summary>
    Public IsSortName As Boolean = False
    ''' <summary>
    ''' 入力されたコマンドが--sort priceである
    ''' </summary>
    Public IsSortPrice As Boolean = False
    ''' <summary>
    ''' 入力されたコマンドがExitである
    ''' </summary>
    Public IsExit As Boolean = False

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
        If StringUtil.IsNotEmpty(inputCommand) AndAlso Regex.IsMatch(inputCommand, "^(--name .*)") Then
            IsSearchName = True
            Return IsSearchName
        End If
        If StringUtil.IsNotEmpty(inputCommand) AndAlso Regex.IsMatch(inputCommand, "--price [0-9]{1,}\-[0-9]{1,}$") Then
            IsSearchPrice = True
            Return IsSearchPrice
        End If
        If StringUtil.IsNotEmpty(inputCommand) AndAlso "--sort name".Equals(inputCommand) Then
            IsSortName = True
            Return IsSortName
        End If
        If StringUtil.IsNotEmpty(inputCommand) AndAlso "--sort price".Equals(inputCommand) Then
            IsSortPrice = True
            Return IsSortPrice
        End If
        If StringUtil.IsNotEmpty(inputCommand) AndAlso "exit".Equals(inputCommand) Then
            IsExit = True
            Return IsExit
        End If
        Console.WriteLine("入力内容が正しくありません。")
        Return False
    End Function
End Class
