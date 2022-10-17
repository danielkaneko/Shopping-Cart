Imports NUnit.Framework
Imports ShoppingCart
Imports Fhi.Fw.TestUtil.DebugString

Public Class ProductsListMakerTest
    Private Overloads Function ToString(products As IEnumerable(Of Products)) As String
        Dim maker As New DebugStringMaker(Of Products)(Function(defineBy As IDebugStringRuleBinder, vo As Products) _
                                                           defineBy.BindFuncWithTitle(Function() vo.Id, NameOf(vo.Id)).
                                                           BindFuncWithTitle(Function() vo.Name, NameOf(vo.Name)).
                                                           BindFuncWithTitle(Function() vo.Description, NameOf(vo.Description)).
                                                           BindFuncWithTitle(Function() vo.Price, NameOf(vo.Price)).
                                                           BindFuncWithTitle(Function() vo.Inventory, NameOf(vo.Inventory)))
        Return maker.MakeString(products)
    End Function
    <Test>
    Public Sub 商品が格納されたリストを返すこと()

        Dim productListMaker As New ProductsListMaker
        Dim actual As Dictionary(Of String, Products) = productListMaker.MakeProductsList
        Assert.That(ToString(actual.Values),
                            [Is].EqualTo("Id       Name           Description              Price Inventory" & vbCrLf &
                                         "'MPN-1'  '消しゴム'     'モノけし'                 120        30" & vbCrLf &
                                         "'MPN-2'  'ノート'       '自由帳'                   200       100" & vbCrLf &
                                         "'MPN-3'  '椅子'         '座椅子'                  3000        20" & vbCrLf &
                                         "'MPN-4'  'ソファー'     'リクライニング機能付き' 15000         5" & vbCrLf &
                                         "'MPN-5'  '机'           '四つ足の机'              5000         5" & vbCrLf &
                                         "'MPN-6'  '携帯電話'     'SIMカード無'            30000        10" & vbCrLf &
                                         "'MPN-7'  '充電器'       'タイプCの充電器'         2000        10" & vbCrLf &
                                         "'MPN-8'  'ゴミ箱'       'そこは浅め'              1500        20" & vbCrLf &
                                         "'MPN-9'  '掃除機'       'コードレス'             23000         8" & vbCrLf &
                                         "'MPN-10' 'パソコン'     'ノートPC'               22000         5" & vbCrLf &
                                         "'MPN-11' '色鉛筆セット' '12色の色鉛筆'             900        20" & vbCrLf &
                                         "'MPN-12' '鉛筆'         '六角形の鉛筆'             100        50" & vbCrLf &
                                         "'MPN-13' 'applewatch'   '中古'                   11000         2"))

    End Sub
End Class
