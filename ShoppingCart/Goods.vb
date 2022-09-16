''' <summary>
''' 所品リストのプロパティ設定
''' </summary>
Public Class Goods
    ''' <summary>
    ''' 商品名
    ''' </summary>
    Public GoodsName As String
    ''' <summary>
    ''' 商品説明
    ''' </summary>
    Public GoodsDescription As String
    ''' <summary>
    ''' 商品価格
    ''' </summary>
    Public GoodsPrice As Integer
    ''' <summary>
    ''' 商品在庫
    ''' </summary>
    Public GoodsInventory As Integer
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="goodsName">商品名</param>
    ''' <param name="goodsDescription">商品説明</param>
    ''' <param name="goodsPrice">商品価格</param>
    ''' <param name="goodsInventory">商品在庫</param>
    Public Sub New(goodsName As String, goodsDescription As String, goodsPrice As Integer, goodsInventory As Integer)
        Me.GoodsName = goodsName
        Me.GoodsDescription = goodsDescription
        Me.GoodsPrice = goodsPrice
        Me.GoodsInventory = goodsInventory
    End Sub
End Class
