''' <summary>
''' 商品リストを生成する
''' </summary>
Public Class ProductsListMaker

    ''' <summary>
    ''' 商品リストに各プロパティの値を設定し返す
    ''' </summary>
    ''' <returns>商品リスト</returns>
    Public Function MakeProductsList() As List(Of Products)
        Dim productsList As New List(Of Products) From {
            New Products("消しゴム", "モノけし", 120, 30),
            New Products("ノート", "自由帳", 200, 100),
            New Products("椅子", "座椅子", 3000, 20),
            New Products("ソファー", "リクライニング機能付き", 15000, 5),
            New Products("机", "四つ足の机", 5000, 5),
            New Products("携帯電話", "SIMカード無", 30000, 10),
            New Products("充電器", "タイプCの充電器", 2000, 10),
            New Products("ゴミ箱", "そこは浅め", 1500, 20),
            New Products("掃除機", "コードレス", 23000, 8),
            New Products("パソコン", "ノートPC", 22000, 5),
            New Products("色鉛筆セット", "12色の色鉛筆", 900, 20),
            New Products("鉛筆", "六角形の鉛筆", 100, 50),
            New Products("applewatch", "中古", 11000, 2)
        }
        Return productsList
    End Function

End Class
