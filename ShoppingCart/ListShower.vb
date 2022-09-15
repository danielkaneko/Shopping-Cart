Public Class ListShower
    ''' <summary>
    ''' 渡されたリストを表示する
    ''' </summary>
    ''' <param name="goodsList">表示するリスト</param>
    Public Sub ShowList(goodsList As List(Of GoodsListMaker.GoodsProperty))
        For Each list In goodsList
            Console.WriteLine("商品名：" & list.goodsName & vbCrLf & "商品説明：" & list.goodsDescription & vbCrLf &
                          "価格：" & list.goodsPrice & "円" & vbCrLf & "在庫数：" & list.goodsInventory)
            Console.WriteLine()
        Next
    End Sub

End Class
