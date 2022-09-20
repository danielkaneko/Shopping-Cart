Imports NUnit.Framework
Imports ShoppingCart
Public Class ReturningListInResponseToCommandTest
    Private ReadOnly rlirtc As New ReturningListInResponseToCommand
    Private ReadOnly plm As New ProductsListMaker
    Private ReadOnly productsList As List(Of Products) = plm.MakeProductsList

    <Test>
    Public Sub 商品名検索で入力した文字列を含む商品が全て取得できていればTrueを返す()
        Dim actual As List(Of Products) = rlirtc.ReturnSearchNameList("--name 鉛筆", productsList)
        Assert.IsTrue(actual.Count = 2 AndAlso "色鉛筆セット".Equals(actual(0).ProductsName) AndAlso "鉛筆".Equals(actual(1).ProductsName))
    End Sub

    <Test>
    Public Sub 商品価格検索で入力した金額の範囲内の商品が全て取得できていればTrueを返す()
        Dim actual As List(Of Products) = rlirtc.ReturnSearchPriceList("--price 100-500", productsList)
        Assert.IsTrue(actual.Count = 3 AndAlso "消しゴム".Equals(actual(0).ProductsName) AndAlso "ノート".Equals(actual(1).ProductsName) AndAlso "鉛筆".Equals(actual(2).ProductsName))
    End Sub
    <Test>
    Public Sub 商品名で並び替えされたリストの並び替えが正しく行われていればTrueを返す()
        Dim actual As List(Of Products) = rlirtc.ReturnSortNameList(productsList)
        Assert.IsTrue(actual.Count = productsList.Count AndAlso "applewatch".Equals(actual(0).ProductsName) AndAlso "掃除機".Equals(actual(actual.Count - 1).ProductsName))
    End Sub
    <Test>
    Public Sub 商品価格検索で入力した金額の範囲内の商品が取得できていればTrueを返す()
        Dim actual As List(Of Products) = rlirtc.ReturnSortPriceList(productsList)
        Assert.IsTrue(actual.Count = productsList.Count AndAlso "鉛筆".Equals(actual(0).ProductsName) AndAlso "携帯電話".Equals(actual(actual.Count - 1).ProductsName))
    End Sub

End Class
