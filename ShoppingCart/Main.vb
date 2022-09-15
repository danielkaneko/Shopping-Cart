Module Main

    Sub Main()
        Dim plm As New GoodsListMaker
        Dim goodsList As List(Of GoodsListMaker.GoodsProperty) = plm.RegisterGoodsList
        Dim ls As New ListShower
        ls.ShowList(goodsList)
    End Sub

End Module
