''' <summary>
''' 商品リストを生成する
''' </summary>
Public Class GoodsListMaker
    ''' <summary>
    ''' 所品リストのプロパティ設定
    ''' </summary>
    Public Class GoodsProperty
        ''' <summary>
        ''' 商品名
        ''' </summary>
        Public goodsName As String
        ''' <summary>
        ''' 商品説明
        ''' </summary>
        Public goodsDescription As String
        ''' <summary>
        ''' 商品価格
        ''' </summary>
        Public goodsPrice As Integer
        ''' <summary>
        ''' 商品在庫
        ''' </summary>
        Public goodsInventory As Integer
        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <param name="goodsName">商品名</param>
        ''' <param name="goodsDescription">商品説明</param>
        ''' <param name="goodsPrice">商品価格</param>
        ''' <param name="goodsInventory">商品在庫</param>
        Public Sub New(goodsName As String, goodsDescription As String, goodsPrice As Integer, goodsInventory As Integer)
            Me.goodsName = goodsName
            Me.goodsDescription = goodsDescription
            Me.goodsPrice = goodsPrice
            Me.goodsInventory = goodsInventory
        End Sub
    End Class

    ''' <summary>
    ''' 商品リストに各プロパティの値を設定し返す
    ''' </summary>
    ''' <returns>商品リスト</returns>
    Public Function RegisterGoodsList() As List(Of GoodsProperty)
        Dim goodsList As New List(Of GoodsProperty) From {
            New GoodsProperty("消しゴム", "モノけし", 120, 30),
            New GoodsProperty("ノート", "自由帳", 200, 100),
            New GoodsProperty("椅子", "座椅子", 3000, 20),
            New GoodsProperty("ソファー", "リクライニング機能付き", 15000, 5),
            New GoodsProperty("机", "四つ足の机", 5000, 5),
            New GoodsProperty("携帯電話", "SIMカード無", 30000, 10),
            New GoodsProperty("充電器", "タイプCの充電器", 2000, 10),
            New GoodsProperty("ゴミ箱", "そこは浅め", 1500, 20),
            New GoodsProperty("掃除機", "コードレス", 23000, 8),
            New GoodsProperty("パソコン", "ノートPC", 22000, 5),
            New GoodsProperty("色鉛筆セット", "12色の色鉛筆", 900, 20),
            New GoodsProperty("鉛筆", "六角形の鉛筆", 100, 50),
            New GoodsProperty("applewatch", "中古", 11000, 2)
        }
        Return goodsList
    End Function

End Class
