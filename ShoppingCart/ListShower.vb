Imports Fhi.Fw
''' <summary>
''' リスト表示
''' </summary>
Public Class ListShower
    ''' <summary>
    ''' 渡されたリストを表示する
    ''' </summary>
    ''' <param name="goodsList">表示するリスト</param>
    Public Sub ShowList(goodsList As List(Of Goods))
        If StringUtil.IsNotEmpty(goodsList) Then
            For Each oneGoods As Goods In goodsList
                Console.WriteLine("商品名：" & oneGoods.GoodsName & vbCrLf & "商品説明：" & oneGoods.GoodsDescription & vbCrLf &
                              "価格：" & oneGoods.GoodsPrice & "円" & vbCrLf & "在庫数：" & oneGoods.GoodsInventory)
                Console.WriteLine()
            Next
        Else
            Console.WriteLine("検索条件に一致する商品が見つかりませんでした")
        End If
    End Sub

End Class
