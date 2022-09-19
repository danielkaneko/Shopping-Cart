Imports NUnit.Framework
Imports ShoppingCart
Public Class SplittingStringTest
    <Test>
    Public Sub 引数に値が存在しなかった場合エラーと返す()
        Dim inputContent As String = Nothing
        Dim delimiter As String = " "
        Assert.That(Sub() SplittingString.SplitIntoArrays(inputContent, delimiter), Throws.TypeOf(Of ArgumentException).And.Message.EqualTo("inputContentに値が存在しません"))
    End Sub
End Class
