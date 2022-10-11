Module Main

    Sub Main()
        Dim productListMaker As New ProductsListMaker
        Dim productsList As Dictionary(Of String, Products) = productListMaker.MakeProductsList
        Dim commandDetector As New CommandDetector
        Do
            Dim inputChecker As New MeaningfulInputChecker
            Dim inputCommand As String = inputChecker.WaitForMeaningfulInput
            If IsNothing(inputCommand) Then
                Throw New ArgumentException($"{NameOf(inputCommand)}に値が存在しません")
            End If
            DetectCommand(inputChecker).DoAction(inputCommand, productsList)
        Loop
    End Sub

    ''' <summary>
    ''' 入力されたコマンドに応じたクラスを返す
    ''' </summary>
    ''' <param name="inputChecker">判定する変数を持ったクラス</param>
    ''' <returns>適した処理を持つクラス</returns>
    Private Function DetectCommand(inputChecker As MeaningfulInputChecker) As InputCommandAction
        If inputChecker.IsListCommand Then
            Return New InputListCommand
        End If
        If inputChecker.IsSearchName Then
            Return New InputSearchName
        End If
        If inputChecker.IsSearchPrice Then
            Return New InputSearchPrice
        End If
        Return New InputThatNotNeedAction
    End Function

    Private Interface InputCommandAction
        Sub DoAction(inputCommand As String, productsList As Dictionary(Of String, Products))
    End Interface

    ''' <summary>
    ''' 全商品一覧表示が入力されたときの処理
    ''' </summary>
    Private Class InputListCommand : Implements InputCommandAction
        Private Sub DoAction(inputCommand As String, productsList As Dictionary(Of String, Products)) Implements InputCommandAction.DoAction
            Dim listShowwer As New ListShower
            listShowwer.ShowList(productsList.Values.ToList)
        End Sub
    End Class

    ''' <summary>
    ''' 商品名検索が入力されたときの処理
    ''' </summary>
    Private Class InputSearchName : Implements InputCommandAction
        Private Sub DoAction(inputCommand As String, productsList As Dictionary(Of String, Products)) Implements InputCommandAction.DoAction
            Dim listShowwer As New ListShower
            Dim commandDetector As New CommandDetector
            listShowwer.ShowList(commandDetector.FilterListByName(inputCommand, productsList.Values.ToList))
        End Sub
    End Class

    ''' <summary>
    ''' 商品価格検索が入力されたときの処理
    ''' </summary>
    Private Class InputSearchPrice : Implements InputCommandAction
        Private Sub DoAction(inputCommand As String, productsList As Dictionary(Of String, Products)) Implements InputCommandAction.DoAction
            Dim listShowwer As New ListShower
            Dim commandDetector As New CommandDetector
            listShowwer.ShowList(commandDetector.FilterListByPrice(inputCommand, productsList.Values.ToList))
        End Sub
    End Class
    ''' <summary>
    ''' 処理する必要がないコマンドが入力されたときの処理
    ''' </summary>
    Private Class InputThatNotNeedAction : Implements InputCommandAction
        Private Sub DoAction(inputCommand As String, productsList As Dictionary(Of String, Products)) Implements InputCommandAction.DoAction
            Console.WriteLine("入力したコマンドが適切ではありません")
        End Sub
    End Class
End Module
