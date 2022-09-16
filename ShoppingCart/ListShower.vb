Imports Fhi.Fw
''' <summary>
''' リスト表示
''' </summary>
Public Class ListShower
    ''' <summary>
    ''' 渡されたリストを表示する
    ''' </summary>
    ''' <param name="goodsList">表示するリスト</param>
    Public Sub ShowList(goodsList As List(Of GoodsListMaker.GoodsProperty))
        If StringUtil.IsNotEmpty(goodsList) Then
            For Each list In goodsList
                Console.WriteLine("商品名：" & list.goodsName & vbCrLf & "商品説明：" & list.goodsDescription & vbCrLf &
                              "価格：" & list.goodsPrice & "円" & vbCrLf & "在庫数：" & list.goodsInventory)
                Console.WriteLine()
            Next
        Else
            Console.WriteLine("検索条件に一致する商品が見つかりませんでした")
        End If
    End Sub

End Class
