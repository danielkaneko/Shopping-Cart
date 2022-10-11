Imports NUnit.Framework
Imports ShoppingCart
<TestFixture>
Public Class MeaningfulInputCheckerTest
    Private meaningfulInputChecker As MeaningfulInputChecker
    <SetUp>
    Public Sub Setup()
        meaningfulInputChecker = New MeaningfulInputChecker
    End Sub

    Public Class IsValidInputCommandTest : Inherits MeaningfulInputCheckerTest
        <TestCase("list")>
        <TestCase("--name abc")>
        <TestCase("--price 100-1000")>
        <TestCase("--sort name")>
        <TestCase("--sort price")>
        Public Sub コマンド入力で渡された値がコマンドとして適切であればTrueを返す(inputCommand As String)
            Dim actual As Boolean = meaningfulInputChecker.ValidateInputCommand(inputCommand)
            Assert.IsTrue(actual)
        End Sub

        <TestCase("1ist")>
        <TestCase("--naMe abc")>
        <TestCase("--prIce 100-1000")>
        <TestCase("--sort namae")>
        <TestCase("--soRt name")>
        <TestCase("--sort nAme")>
        <TestCase("--soRt price")>
        <TestCase("--sort prICe")>
        <TestCase("")>
        <TestCase(Nothing)>
        Public Sub コマンド入力で渡された値がコマンドとして適切でなければFalseを返す(inputCommand As String)
            Dim actual As Boolean = meaningfulInputChecker.ValidateInputCommand(inputCommand)
            Assert.IsFalse(actual)
        End Sub
    End Class
End Class
