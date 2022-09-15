Module Main

    Sub Main()
        Dim plm As New GoodsListMaker
        Dim goodsList As List(Of GoodsListMaker.GoodsProperty) = plm.RegisterGoodsList
        Dim ls As New ListShower
        Dim mic As New MeaningfulInputChecker
        Dim inputCommand As String = mic.WaitForMeaningfulInput
        If mic.IsList Then
            ls.ShowList(goodsList)
        End If
        If mic.IsSearchName Then
            ls.ShowList(goodsList.FindAll(Function(name) name.goodsName.Contains(inputCommand.Substring(7))))
        End If
    End Sub

End Module
