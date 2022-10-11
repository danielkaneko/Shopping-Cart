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

    <Test>
    Public Sub 商品価格検索で入力した金額の範囲内の商品が全て取得できていること()
        Dim actual As List(Of Products) = commandDetector.FilterListByPrice("--price 100-500", productsList.Values.ToList)
        Assert.That(actual.Count, [Is].EqualTo(3))
        Assert.That(actual(0).Name, [Is].EqualTo("消しゴム"))
        Assert.That(actual(1).Name, [Is].EqualTo("ノート"))
        Assert.That(actual(2).Name, [Is].EqualTo("鉛筆"))
    End Sub
    <Test>
    Public Sub 商品名で並び替えされたリストの並び替えが正しく行われていること()
        Dim actual As List(Of Products) = commandDetector.SortListByName(productsList.Values.ToList)
        Assert.That(actual.Count, [Is].EqualTo(productsList.Count))
        Assert.That(actual(0).Name, [Is].EqualTo("applewatch"))
        Assert.That(actual(12).Name, [Is].EqualTo("掃除機"))
    End Sub
End Class
