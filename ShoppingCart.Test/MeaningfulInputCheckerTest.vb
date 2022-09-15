﻿Imports NUnit.Framework
Imports ShoppingCart
<TestFixture>
Public Class MeaningfulInputCheckerTest
    Private mic As MeaningfulInputChecker
    <SetUp>
    Public Sub Setup()
        mic = New MeaningfulInputChecker
    End Sub

    Public Class CheckInputCommandTest : Inherits MeaningfulInputCheckerTest
        <TestCase("list")>
        Public Sub コマンド入力で渡された値がコマンドとして適切であればTrueを返す(inputCommand As String)
            Dim actual As Boolean = mic.CheckInputCommand(inputCommand)
            Assert.IsTrue(actual)
        End Sub

        <TestCase("1ist")>
        <TestCase("")>
        <TestCase(Nothing)>
        Public Sub コマンド入力で渡された値がコマンドとして適切でなければFalseを返す(inputCommand As String)
            Dim actual As Boolean = mic.CheckInputCommand(inputCommand)
            Assert.IsFalse(actual)
        End Sub
    End Class
End Class
