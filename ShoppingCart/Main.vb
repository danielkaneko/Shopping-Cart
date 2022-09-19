Module Main

    Sub Main()
        Dim plm As New ProductsListMaker
        Dim productsList As List(Of Products) = plm.MakeProductsList
        Dim ls As New ListShower
        Dim mic As New MeaningfulInputChecker
        Do
            Dim inputCommand As String = mic.WaitForMeaningfulInput
            If mic.IsList Then
                ls.ShowList(productsList)
            End If
            If mic.IsSearchName Then
                ls.ShowList(productsList.FindAll(Function(name) name.ProductsName.Contains(SplittingString.SplitIntoArrays(inputCommand, " ")(1))))
            End If
            If mic.IsSearchPrice Then
                Dim amountString As String() = SplittingString.SplitIntoArrays(SplittingString.SplitIntoArrays(inputCommand, " ")(1), "-")
                ls.ShowList(productsList.FindAll(Function(price) Integer.Parse(amountString(0)) < price.ProductsPrice AndAlso price.ProductsPrice < Integer.Parse(amountString(1))))
            End If
            If mic.IsSortName Then
                ls.ShowList((From products In productsList Order By products.ProductsName, products.ProductsDescription, products.ProductsPrice, products.ProductsInventory).ToList())
            End If
            If mic.IsSortPrice Then
                ls.ShowList((From products In productsList Order By products.ProductsPrice, products.ProductsName, products.ProductsDescription, products.ProductsInventory).ToList())
            End If
        Loop While Not mic.IsExit
    End Sub

End Module
