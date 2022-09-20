Module Main

    Sub Main()
        Dim plm As New ProductsListMaker
        Dim productsList As List(Of Products) = plm.MakeProductsList
        Dim ls As New ListShower
        Dim rlirtc As New ReturningListInResponseToCommand
        Do
            Dim mic As New MeaningfulInputChecker
            Dim inputCommand As String = mic.WaitForMeaningfulInput
            If mic.IsListCommand Then
                ls.ShowList(productsList)
            End If
            If mic.IsSearchName Then
                ls.ShowList(rlirtc.ReturnSearchNameList(inputCommand, productsList))
            End If
            If mic.IsSearchPrice Then
                ls.ShowList(rlirtc.ReturnSearchPriceList(inputCommand, productsList))
            End If
            If mic.IsSortName Then
                ls.ShowList(rlirtc.ReturnSortNameList(productsList))
            End If
            If mic.IsSortPrice Then
                ls.ShowList(rlirtc.ReturnSortPriceList(productsList))
            End If
            If mic.IsExit Then
                Exit Do
            End If
        Loop
    End Sub

End Module
