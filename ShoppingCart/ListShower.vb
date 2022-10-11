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
            For Each oneProduct As Products In productsList
                Console.WriteLine("商品ID：" & oneProduct.Id)
                Console.WriteLine("商品名：" & oneProduct.Name)
                Console.WriteLine("商品説明：" & oneProduct.Description)
                Console.WriteLine("価格：" & oneProduct.Price & "円")
                Console.WriteLine("在庫数：" & oneProduct.Inventory)
                Console.WriteLine()
            Next
        Else
            Console.WriteLine("検索条件に一致する商品が見つかりませんでした")
        End If
    End Sub

End Class
