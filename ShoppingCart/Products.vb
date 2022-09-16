﻿''' <summary>
''' 所品リストのプロパティ設定
''' </summary>
Public Class Products
    ''' <summary>
    ''' 商品名
    ''' </summary>
    Public ProductsName As String
    ''' <summary>
    ''' 商品説明
    ''' </summary>
    Public ProductsDescription As String
    ''' <summary>
    ''' 商品価格
    ''' </summary>
    Public ProductsPrice As Integer
    ''' <summary>
    ''' 商品在庫
    ''' </summary>
    Public ProductsInventory As Integer
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="productsName">商品名</param>
    ''' <param name="productsDescription">商品説明</param>
    ''' <param name="productsPrice">商品価格</param>
    ''' <param name="productsInventory">商品在庫</param>
    Public Sub New(productsName As String, productsDescription As String, productsPrice As Integer, productsInventory As Integer)
        Me.ProductsName = productsName
        Me.ProductsDescription = productsDescription
        Me.ProductsPrice = productsPrice
        Me.ProductsInventory = productsInventory
    End Sub
End Class