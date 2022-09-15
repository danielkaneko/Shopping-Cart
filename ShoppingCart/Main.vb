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
    End Sub

End Module
