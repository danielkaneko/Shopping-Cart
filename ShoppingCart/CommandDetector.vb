''' <summary>
''' コマンドを検出するクラス
''' </summary>
Public Class CommandDetector
    Private Class ProductsNameCompare : Implements IComparer(Of String)
        Public Function Compare(x As String, y As String) As Integer Implements IComparer(Of String).Compare
            Dim ci As System.Globalization.CompareInfo = System.Globalization.CompareInfo.GetCompareInfo("ja-JP")
            Return ci.Compare(x, y)
        End Function
    End Class

    ''' <summary>
    ''' 商品名で検索されたリストを返す
    ''' </summary>
    ''' <param name="inputCommand">入力されたコマンド</param>
    ''' <param name="productsList">商品リスト</param>
    ''' <returns>表示するリスト</returns>
    Public Function FilterListByName(inputCommand As String, productsList As List(Of Products)) As List(Of Products)
        Dim stringToSearch As String = SplitCommand(inputCommand)(1)
        Return productsList.FindAll(Function(product) product.Name.Contains(stringToSearch))
    End Function

    ''' <summary>
    ''' 商品価格で検索されたリストを返す
    ''' </summary>
    ''' <param name="inputCommand">入力されたコマンド</param>
    ''' <param name="productsList">商品リスト</param>
    ''' <returns>表示するリスト</returns>
    Public Function FilterListByPrice(inputCommand As String, productsList As List(Of Products)) As List(Of Products)
        Dim amountString As String() = SplitCommandRange(SplitCommand(inputCommand)(1))
        Return productsList.FindAll(Function(product) Integer.Parse(amountString(0)) <= product.Price AndAlso product.Price <= Integer.Parse(amountString(1)))
    End Function

    ''' <summary>
    ''' 商品名で並び替えされたリストを返す
    ''' </summary>
    ''' <param name="productsList">商品リスト</param>
    ''' <returns>表示するリスト</returns>
    Public Function SortListByName(productsList As List(Of Products)) As List(Of Products)
        Dim compare As New ProductsNameCompare
        Return productsList.OrderBy(Function(product) product.Name, compare).ThenBy(Function(product) product.Id).ThenBy(Function(product) product.Description).ThenBy(Function(product) product.Price).ThenBy(Function(product) product.Inventory).ToList()
    End Function

    ''' <summary>
    ''' 商品価格で並び替えされたリストを返す
    ''' </summary>
    ''' <param name="productsList">商品リスト</param>
    ''' <returns>表示するリスト</returns>
    Public Function SortListByPrice(productsList As List(Of Products)) As List(Of Products)
        Dim compare As New ProductsNameCompare
        Return productsList.OrderBy(Function(product) product.Price).ThenBy(Function(product) product.Id).ThenBy(Function(product) product.Name, compare).ThenBy(Function(product) product.Description).ThenBy(Function(product) product.Inventory).ToList()
    End Function

    ''' <summary>
    ''' コマンドを分割する
    ''' </summary>
    ''' <param name="inputCommand">コマンド</param>
    ''' <returns>分割されたコマンド</returns>
    Private Function SplitCommand(inputCommand As String) As String()
        Return inputCommand.Split(" "c)
    End Function

    ''' <summary>
    ''' コマンドを範囲として分割する
    ''' </summary>
    ''' <param name="inputCommand">コマンド</param>
    ''' <returns>分割されたコマンド</returns>
    Private Function SplitCommandRange(inputCommand As String) As String()
        Return inputCommand.Split("-"c)
    End Function
End Class
