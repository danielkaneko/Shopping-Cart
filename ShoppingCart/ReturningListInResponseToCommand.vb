''' <summary>
''' 入力されたコマンドごとにリストを返すクラス
''' </summary>
Public Class ReturningListInResponseToCommand
    ''' <summary>
    ''' 商品名で検索されたリストを返す
    ''' </summary>
    ''' <param name="inputCommand">入力されたコマンド</param>
    ''' <param name="productsList">商品リスト</param>
    ''' <returns>表示するリスト</returns>
    Public Function ReturnSearchNameList(inputCommand As String, productsList As List(Of Products)) As List(Of Products)
        Return productsList.FindAll(Function(name) name.ProductsName.Contains(SplittingString.SplitIntoArrays(inputCommand, " ")(1)))
    End Function

    ''' <summary>
    ''' 商品価格で検索されたリストを返す
    ''' </summary>
    ''' <param name="inputCommand">入力されたコマンド</param>
    ''' <param name="productsList">商品リスト</param>
    ''' <returns>表示するリスト</returns>
    Public Function ReturnSearchPriceList(inputCommand As String, productsList As List(Of Products)) As List(Of Products)
        Dim amountString As String() = SplittingString.SplitIntoArrays(SplittingString.SplitIntoArrays(inputCommand, " ")(1), "-")
        Return productsList.FindAll(Function(price) Integer.Parse(amountString(0)) <= price.ProductsPrice AndAlso price.ProductsPrice <= Integer.Parse(amountString(1)))
    End Function

    ''' <summary>
    ''' 商品名で並び替えされたリストを返す
    ''' </summary>
    ''' <param name="productsList">商品リスト</param>
    ''' <returns>表示するリスト</returns>
    Public Function ReturnSortNameList(productsList As List(Of Products)) As List(Of Products)
        Return productsList.OrderBy(Function(products) products.ProductsName).ThenBy(Function(products) products.ProductsDescription).ThenBy(Function(products) products.ProductsPrice).ThenBy(Function(products) products.ProductsInventory).ToList()
        'Return (From products In productsList Order By products.ProductsName, products.ProductsDescription, products.ProductsPrice, products.ProductsInventory).ToList()
    End Function

    ''' <summary>
    ''' 商品価格で並び替えされたリストを返す
    ''' </summary>
    ''' <param name="productsList">商品リスト</param>
    ''' <returns>表示するリスト</returns>
    Public Function ReturnSortPriceList(productsList As List(Of Products)) As List(Of Products)
        Return (From products In productsList Order By products.ProductsPrice, products.ProductsName, products.ProductsDescription, products.ProductsInventory).ToList()
    End Function
End Class
