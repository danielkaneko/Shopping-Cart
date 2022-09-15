﻿Module Main

    Sub Main()
        Dim plm As New GoodsListMaker
        Dim goodsList As List(Of GoodsListMaker.GoodsProperty) = plm.RegisterGoodsList
        Dim ls As New ListShower
        Dim mic As New MeaningfulInputChecker
        Dim inputCommand As String = mic.WaitForMeaningfulInput
        If mic.IsList Then
            ls.ShowList(goodsList)
        End If
        If mic.IsSearchName Then
            ls.ShowList(goodsList.FindAll(Function(name) name.goodsName.Contains(inputCommand.Substring(7))))
        End If
        If mic.IsSearchPrice Then
            Dim se As New StringExtraction
            Dim amountString As String() = se.ExtractionString(se.ExtractionString(inputCommand, " ")(1), "-")
            ls.ShowList(goodsList.FindAll(Function(price) Integer.Parse(amountString(0)) < price.goodsPrice AndAlso price.goodsPrice < Integer.Parse(amountString(1))))
        End If
        If mic.IsSortName Then
            ls.ShowList((From goods In goodsList Order By goods.goodsName, goods.goodsDescription, goods.goodsPrice, goods.goodsInventory).ToList())
        End If
    End Sub

End Module
