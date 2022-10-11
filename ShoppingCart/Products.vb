''' <summary>
''' 商品リストのプロパティ設定
''' </summary>
Public Class Products
    ''' <summary>
    ''' 商品ID
    ''' </summary>
    Public ReadOnly Property Id As String
    ''' <summary>
    ''' 商品名
    ''' </summary>
    Public ReadOnly Property Name As String
    ''' <summary>
    ''' 商品説明
    ''' </summary>
    Public ReadOnly Property Description As String
    ''' <summary>
    ''' 商品価格
    ''' </summary>
    Public ReadOnly Property Price As Integer
    ''' <summary>
    ''' 商品在庫
    ''' </summary>
    Public ReadOnly Property Inventory As Integer
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="productsName">商品名</param>
    ''' <param name="productsDescription">商品説明</param>
    ''' <param name="productsPrice">商品価格</param>
    ''' <param name="productsInventory">商品在庫</param>
    Public Sub New(productsId As String, productsName As String, productsDescription As String, productsPrice As Integer, productsInventory As Integer)
        Me.Id = productsId
        Me.Name = productsName
        Me.Description = productsDescription
        Me.Price = productsPrice
        Me.Inventory = productsInventory
    End Sub

    ''' <summary>
    ''' 引数なしコンストラクタ
    ''' </summary>
    Public Sub New()

    End Sub
End Class
