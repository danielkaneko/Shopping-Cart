''' <summary>
''' 商品リストを生成する
''' </summary>
Public Class ProductsListMaker

    ''' <summary>
    ''' 商品リストに各プロパティの値を設定し返す
    ''' </summary>
    ''' <returns>商品リスト</returns>
    Public Function MakeProductsList() As Dictionary(Of String, Products)
        Dim productsList As New List(Of Products) From {
            New Products("MPN-1", "消しゴム", "モノけし", 120, 30),
            New Products("MPN-2", "ノート", "自由帳", 200, 100),
            New Products("MPN-3", "椅子", "座椅子", 3000, 20),
            New Products("MPN-4", "ソファー", "リクライニング機能付き", 15000, 5),
            New Products("MPN-5", "机", "四つ足の机", 5000, 5),
            New Products("MPN-6", "携帯電話", "SIMカード無", 30000, 10),
            New Products("MPN-7", "充電器", "タイプCの充電器", 2000, 10),
            New Products("MPN-8", "ゴミ箱", "そこは浅め", 1500, 20),
            New Products("MPN-9", "掃除機", "コードレス", 23000, 8),
            New Products("MPN-10", "パソコン", "ノートPC", 22000, 5),
            New Products("MPN-11", "色鉛筆セット", "12色の色鉛筆", 900, 20),
            New Products("MPN-12", "鉛筆", "六角形の鉛筆", 100, 50),
            New Products("MPN-13", "applewatch", "中古", 11000, 2)
        }
        Return productsList.ToDictionary(Of String, Products)(Function(keyProduct) keyProduct.Id, Function(valueProduct) valueProduct)
    End Function

End Class
