﻿''' <summary>
''' 商品リストを生成する
''' </summary>
Public Class GoodsListMaker

    ''' <summary>
    ''' 商品リストに各プロパティの値を設定し返す
    ''' </summary>
    ''' <returns>商品リスト</returns>
    Public Function MakeGoodsList() As List(Of Goods)
        Dim goodsList As New List(Of Goods) From {
            New Goods("消しゴム", "モノけし", 120, 30),
            New Goods("ノート", "自由帳", 200, 100),
            New Goods("椅子", "座椅子", 3000, 20),
            New Goods("ソファー", "リクライニング機能付き", 15000, 5),
            New Goods("机", "四つ足の机", 5000, 5),
            New Goods("携帯電話", "SIMカード無", 30000, 10),
            New Goods("充電器", "タイプCの充電器", 2000, 10),
            New Goods("ゴミ箱", "そこは浅め", 1500, 20),
            New Goods("掃除機", "コードレス", 23000, 8),
            New Goods("パソコン", "ノートPC", 22000, 5),
            New Goods("色鉛筆セット", "12色の色鉛筆", 900, 20),
            New Goods("鉛筆", "六角形の鉛筆", 100, 50),
            New Goods("applewatch", "中古", 11000, 2)
        }
        Return goodsList
    End Function

End Class
