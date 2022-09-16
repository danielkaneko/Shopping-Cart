Imports Fhi.Fw
''' <summary>
''' リスト表示
''' </summary>
Public Class ListShower
    ''' <summary>
    ''' 渡されたリストを表示する
    ''' </summary>
    ''' <param name="productsList">表示するリスト</param>
    Public Sub ShowList(productsList As List(Of Products))
        If StringUtil.IsNotEmpty(productsList) Then
            For Each oneProducts As Products In productsList
                Console.WriteLine("商品名：" & oneProducts.ProductsName)
                Console.WriteLine("商品説明：" & oneProducts.ProductsDescription)
                Console.WriteLine("価格：" & oneProducts.ProductsPrice & "円")
                Console.WriteLine("在庫数：" & oneProducts.ProductsInventory)
                Console.WriteLine()
            Next
        Else
            Console.WriteLine("検索条件に一致する商品が見つかりませんでした")
        End If
    End Sub

End Class
