Imports NUnit.Framework
Imports ShoppingCart
Public Class CommandDetectorTest
    Private ReadOnly commandDetector As New CommandDetector
    Private ReadOnly productsListMaker As New ProductsListMaker
    Private ReadOnly productsList As Dictionary(Of String, Products) = productsListMaker.MakeProductsList

    <Test>
    Public Sub 商品名検索で入力した文字列を含む商品が全て取得できていること()
        Dim actual As List(Of Products) = commandDetector.FilterListByName("--name 鉛筆", productsList.Values.ToList)
        Assert.That(actual.Count, [Is].EqualTo(2))
        '値が取得できているかどうかのテストなのでKeyを取得するだけでよい
        Assert.That(actual(0).Name, [Is].EqualTo("色鉛筆セット"))
        Assert.That(actual(1).Name, [Is].EqualTo("鉛筆"))
    End Sub
End Class
