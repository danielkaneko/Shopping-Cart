Imports NUnit.Framework
Imports Fhi.Fw.Domain

Namespace TestUtil.DebugString.Test
    Public MustInherit Class DebugStringMakerTest
#Region "Nested classes..."
        Private Class HogeVo
            Public Property Id As Integer?
            Public Property Name As String
            Public Property Money As Decimal?
        End Class
        Public Enum MyEnum
            ONE = 1
            TWO
            THREE
        End Enum
        Private Class Fuga
            Private ReadOnly keyValues As New Dictionary(Of String, String)
            Public Function GetValue(ByVal key As String) As String
                Return keyValues(key)
            End Function
            Public Sub SetValue(ByVal key As String, ByVal value As String)
                If keyValues.ContainsKey(key) Then
                    keyValues.Remove(key)
                End If
                keyValues.Add(key, value)
            End Sub
        End Class
        Private Class Boolean2Vo
            Public Property Bool1 As Boolean?
            Public Property Bool2 As Boolean?
            Public Property Name As String
        End Class
        Private Class Boolean3Vo
            Public Property Bool1 As Boolean?
            Public Property Bool2 As Boolean?
            Public Property Bool3 As Boolean?
            Public Property Name As String
        End Class

        Private Class LastVo
            Public Property Name As String
        End Class

        Private Class SecondVo
            Public Property Id As Integer
            Public Property LastVo As LastVo
        End Class

        Private Class FirstVo
            Public Property Id As Integer
            Public Property Name As String
            Public Property SecondVo As SecondVo
        End Class

        Private Class HeaderVo
            Public Property Id As Integer
            Public Property Name As String
            Public Property Details As ICollection(Of DetailVo)
        End Class
        Public Class DetailVo
            Public Property Name As String
            Public Property Middle As String
            Public Property Values As ICollection(Of ValueVo)
        End Class
        Public Class ValueVo
            Public Property Value As String
            Public Property Name As String
        End Class

        Private Class HeaderVoWithTwoDetails
            Public Property Name As String
            Public Property ADetails As ICollection(Of DetailVo)
            Public Property BDetails As ICollection(Of DetailVo)
        End Class
#End Region

        <SetUp()> Public Overridable Sub SetUp()

        End Sub

        Public Class ConvDebugValueTest : Inherits DebugStringMakerTest

            <Test(), Sequential()> Public Sub Int型_実値ならそのまま出力する(
                    <Values(0, 1, -1)> ByVal value As Integer,
                    <Values("0", "1", "-1")> ByVal expected As String)
                Assert.That(DebugStringMaker.ConvDebugValue(value), [Is].EqualTo(expected))
            End Sub

            <Test()> Public Sub Int型_nullならnullを出力する()
                Dim i As Integer? = Nothing
                Assert.That(DebugStringMaker.ConvDebugValue(i), [Is].EqualTo("null"))
            End Sub

            <Test(), Sequential()> Public Sub Decimal型_実値ならそのまま出力する(
                    <Values("0", "1", "-1", "0.00", "0.00001")> ByVal value As String)
                Dim d As Decimal = CDec(value)
                Assert.That(DebugStringMaker.ConvDebugValue(d), [Is].EqualTo(value))
            End Sub

            <Test(), Sequential()> Public Sub String型_シングルクォートで囲って出力する(
                    <Values("", "abc", "123", "あいう", "漢字")> ByVal value As String)
                Assert.That(DebugStringMaker.ConvDebugValue(value), [Is].EqualTo("'" & value & "'"))
            End Sub

            <Test()> Public Sub String型_nullならnullを出力する_シングルクォートで囲まない()
                Dim s As String = Nothing
                Assert.That(DebugStringMaker.ConvDebugValue(s), [Is].EqualTo("null"))
            End Sub

            <Test(), Sequential()> Public Sub DateTime型_シングルクォートで囲って出力する(
                    <Values("2016/1/1", "1:22:33")> ByVal dateStr As String,
                    <Values("'2016/01/01 0:00:00'", "'1:22:33'")> ByVal expected As String)
                Assert.That(DebugStringMaker.ConvDebugValue(CDate(dateStr)), [Is].EqualTo(expected))
            End Sub

            <Test(), Sequential()> Public Sub Enum型_シングルクォート抜きで_Enum名称で出力する(
                    <Values(MyEnum.ONE, MyEnum.TWO, MyEnum.THREE)> ByVal value As MyEnum,
                    <Values("ONE", "TWO", "THREE")> ByVal expected As String)
                Assert.That(DebugStringMaker.ConvDebugValue(value), [Is].EqualTo(expected))
            End Sub

            <TestCase("A" & vbCrLf & "B", "'A\nB'")>
            <TestCase("1" & vbCrLf & "2" & vbCrLf & "3", "'1\n2\n3'")>
            Public Sub 改行は_YenNになる(value As String, expected As String)
                Assert.That(DebugStringMaker.ConvDebugValue(value), [Is].EqualTo(expected))
            End Sub

        End Class

        Public Class MakeStringTest : Inherits DebugStringMakerTest

            <Test()> Public Sub タイトル未定ならプロパティ名がタイトルになる()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.Bind(vo.Id, vo.Name, vo.Money))
                Assert.That(maker.MakeString(New HogeVo With {.Id = 123, .Name = "abc", .Money = 3.14D}), [Is].EqualTo(
                            "Id  Name  Money" & vbCrLf &
                            "123 'abc'  3.14"))
            End Sub

            <Test()> Public Sub タイトル指定なら_それがタイトルになる()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.BindWithTitle(vo.Id, "I").BindWithTitle(vo.Name, "NM").BindWithTitle(vo.Money, "MNY"))
                Assert.That(maker.MakeString(New HogeVo With {.Id = 12, .Name = "abc", .Money = 0D}), [Is].EqualTo(
                            "I  NM    MNY" & vbCrLf &
                            "12 'abc'   0"))
            End Sub

            <Test()> Public Sub タイトル指定なら_それがタイトルになる_タイトルは数値でも常に左寄せ()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.BindWithTitle(vo.Id, "1").BindWithTitle(vo.Name, "22").BindWithTitle(vo.Money, "300"))
                Assert.That(maker.MakeString(New HogeVo With {.Id = 12, .Name = "abc", .Money = 12D}), [Is].EqualTo(
                            "1  22    300" & vbCrLf &
                            "12 'abc'  12"))
            End Sub

            <Test()> Public Sub 数値型は右揃えになる_int型()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.Bind(vo.Id))
                Assert.That(maker.MakeString(New HogeVo With {.Id = 12},
                                             New HogeVo With {.Id = 234},
                                             New HogeVo With {.Id = 3456}), [Is].EqualTo(
                            "Id  " & vbCrLf &
                            "  12" & vbCrLf &
                            " 234" & vbCrLf &
                            "3456"))
            End Sub

            <Test()> Public Sub 数値型は右揃えになる_Decimal型_小数点が揃う()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.Bind(vo.Money))
                Assert.That(maker.MakeString(New HogeVo With {.Money = 1.23D},
                                             New HogeVo With {.Money = 23.4D},
                                             New HogeVo With {.Money = 345D}), [Is].EqualTo(
                            "Money " & vbCrLf &
                            "  1.23" & vbCrLf &
                            " 23.4 " & vbCrLf &
                            "345   "))
            End Sub

            <Test()> Public Sub 数値型は右揃えになる_けどAlignsLeftTheNumericがtrueなら左揃え_int型()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.Bind(vo.Id))
                maker.AlignsLeftTheNumeric = True
                Assert.That(maker.MakeString(New HogeVo With {.Id = 12},
                                             New HogeVo With {.Id = 234},
                                             New HogeVo With {.Id = 3456}), [Is].EqualTo(
                            "Id  " & vbCrLf &
                            "12  " & vbCrLf &
                            "234 " & vbCrLf &
                            "3456"))
            End Sub

            <Test()> Public Sub 数値型は右揃えになる_けどAlignsLeftTheNumericがtrueなら左揃え_Decimal型()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.Bind(vo.Money))
                maker.AlignsLeftTheNumeric = True
                Assert.That(maker.MakeString(New HogeVo With {.Money = 12.3D},
                                             New HogeVo With {.Money = 4D},
                                             New HogeVo With {.Money = 5.67D}), [Is].EqualTo(
                            "Money" & vbCrLf &
                            "12.3 " & vbCrLf &
                            "4    " & vbCrLf &
                            "5.67 "))
            End Sub

            <Test()> Public Sub 数値型は右揃えになる_けどnullは左揃え_int型()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.Bind(vo.Id))
                Assert.That(maker.MakeString(New HogeVo With {.Id = 12},
                                             New HogeVo With {.Id = Nothing},
                                             New HogeVo With {.Id = 34567}), [Is].EqualTo(
                            "Id   " & vbCrLf &
                            "   12" & vbCrLf &
                            "null " & vbCrLf &
                            "34567"))
            End Sub

            <Test()> Public Sub 数値型は右揃えになる_けどnullは左揃え_Decimal型_小数点が揃ってそれとは関係なくnull表示()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.Bind(vo.Money))
                Assert.That(maker.MakeString(New HogeVo With {.Money = 1.23D},
                                             New HogeVo With {.Money = Nothing},
                                             New HogeVo With {.Money = 45.6D}), [Is].EqualTo(
                            "Money" & vbCrLf &
                            " 1.23" & vbCrLf &
                            "null " & vbCrLf &
                            "45.6 "))
            End Sub

            <Test(), Sequential()> Public Sub 小数点以下桁数が多いと省略される_DecimalLengthを超えたら省略される()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.Bind(vo.Money)) With {.DecimalLength = 5}
                Assert.That(maker.MakeString(New HogeVo With {.Money = 1.234567D}), [Is].EqualTo(
                            "Money  " & vbCrLf &
                            "1.234…"), "小数点以下5桁(DecimalLength)以下じゃないから省略される")
            End Sub

            <Test(), Sequential()> Public Sub 小数点以下桁数が多いと省略される_DecimalLength以下なら省略しない()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.Bind(vo.Money)) With {.DecimalLength = 6}
                Assert.That(maker.MakeString(New HogeVo With {.Money = 1.234567D}), [Is].EqualTo(
                            "Money   " & vbCrLf &
                            "1.234567"), "小数点以下6桁(DecimalLength)以下だから省略しない")
            End Sub

            <Test(), Sequential()> Public Sub 不要な小数点以下の0を除いて出力できる()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.Bind(vo.Money))
                Assert.That(maker.MakeString(New HogeVo With {.Money = CDec("1.2300")}, New HogeVo With {.Money = CDec("0.00")}), [Is].EqualTo(
                            "Money" & vbCrLf &
                            " 1.23" & vbCrLf &
                            " 0   "))
            End Sub

            <Test()> Public Sub Bind_Boolean型三項目以上の場合_Boolean型のプロパティを指定するとエラーになる()
                Try
                    Dim maker As New DebugStringMaker(Of Boolean3Vo)(Function(defineBy As IDebugStringRuleBinder, vo As Boolean3Vo) _
                                                                         defineBy.Bind(vo.Bool1))
                    Assert.Fail()
                Catch expected As NotSupportedException
                    Assert.That(expected.Message, [Is].EqualTo("Boolean型が3項目以上の場合は、ラムダ式を利用してください"))
                End Try
            End Sub

            <Test()> Public Sub BindWithTitle_Boolean型三項目以上の場合_Boolean型のプロパティを指定するとエラーになる()
                Try
                    Dim maker As New DebugStringMaker(Of Boolean3Vo)(Function(defineBy As IDebugStringRuleBinder, vo As Boolean3Vo) _
                                                                      defineBy.BindWithTitle(vo.Bool1, "1"))
                    Assert.Fail()
                Catch expected As NotSupportedException
                    Assert.That(expected.Message, [Is].EqualTo("Boolean型が3項目以上の場合は、ラムダ式を利用してください"))
                End Try
            End Sub

            <Test()> Public Sub Bind_Boolean型三目以上でも_Boolean型のプロパティを指定しなければ出力できる()
                Dim maker As New DebugStringMaker(Of Boolean3Vo)(Function(defineBy As IDebugStringRuleBinder, vo As Boolean3Vo) _
                                                                  defineBy.Bind(vo.Name))
                Assert.That(maker.MakeString(New Boolean3Vo With {.Name = "N"}), [Is].EqualTo(
                            "Name" & vbCrLf &
                            "'N' "))
            End Sub

            <Test()> Public Sub BindWithTitle_Boolean型三項目以上でも_Boolean型のプロパティを指定しなければ出力できる()
                Dim maker As New DebugStringMaker(Of Boolean3Vo)(Function(defineBy As IDebugStringRuleBinder, vo As Boolean3Vo) _
                                                                  defineBy.BindWithTitle(vo.Name, "C"))
                Assert.That(maker.MakeString(New Boolean3Vo With {.Name = "N"}), [Is].EqualTo(
                            "C  " & vbCrLf &
                            "'N'"))
            End Sub

            <Test()> Public Sub Bind_Boolean型二項目なら_Boolean型のプロパティを指定して出力できる()
                Dim maker As New DebugStringMaker(Of Boolean2Vo)(Function(defineBy As IDebugStringRuleBinder, vo As Boolean2Vo) _
                                                                         defineBy.Bind(vo.Bool1).Bind(vo.Bool2).Bind(vo.Name))
                Assert.That(maker.MakeString(New Boolean2Vo() {New Boolean2Vo With {.Bool1 = True, .Bool2 = True, .Name = "N"}}), [Is].EqualTo(
                            "Bool1 Bool2 Name" & vbCrLf &
                            "True  True  'N' "))
            End Sub

            <Test()> Public Sub BindWithTitle_Boolean型二項目なら_Boolean型のプロパティを指定して出力できる()
                Dim maker As New DebugStringMaker(Of Boolean2Vo)(Function(defineBy As IDebugStringRuleBinder, vo As Boolean2Vo) _
                                                                         defineBy.BindWithTitle(vo.Bool1, "A").BindWithTitle(vo.Bool2, "B") _
                                                                         .BindWithTitle(vo.Name, "Nm"))
                Assert.That(maker.MakeString(New Boolean2Vo() {New Boolean2Vo With {.Bool1 = True, .Bool2 = True, .Name = "N"}}), [Is].EqualTo(
                            "A    B    Nm " & vbCrLf &
                            "True True 'N'"))
            End Sub

            <Test()> Public Sub ネストしたVoの値も出力できる_2階層()
                Dim maker As New DebugStringMaker(Of FirstVo)(Function(defineBy As IDebugStringRuleBinder, vo As FirstVo) _
                                                                 defineBy.Bind(vo.SecondVo.Id))
                Assert.That(maker.MakeString(New FirstVo With {.SecondVo = New SecondVo With {.Id = 8}}), [Is].EqualTo(
                            "Id" & vbCrLf &
                            " 8"))
            End Sub

            <Test()> Public Sub ネストしたVoの値も出力できる_3階層()
                Dim maker As New DebugStringMaker(Of FirstVo)(Function(defineBy As IDebugStringRuleBinder, vo As FirstVo) _
                                                                 defineBy.Bind(vo.SecondVo.LastVo.Name))
                Assert.That(maker.MakeString(New FirstVo With {.SecondVo = New SecondVo With {.LastVo = New LastVo With {.Name = "abc"}}}), [Is].EqualTo(
                            "Name " & vbCrLf &
                            "'abc'"))
            End Sub

        End Class

        Public Class BindDetailByMakeStringTest : Inherits DebugStringMakerTest

            <Test()> Public Sub 内包する配列の値を出力できる()
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.BindWithTitle(vo.Name, "Head").JoinDetails(vo.Details,
                                                                           Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                                               detailBy.Bind(detail.Name)))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details = {New DetailVo With {.Name = "1"},
                                                                                         New DetailVo With {.Name = "2"}}}), [Is].EqualTo(
                            "Head D.Name" & vbCrLf &
                            "'a'  '1'   " & vbCrLf &
                            "'a'  '2'   "))
            End Sub

            <Test()> Public Sub 内包する配列の値を出力できる_別名()
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.BindWithTitle(vo.Name, "Head").JoinDetails(vo.Details,
                                                                           Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                                               detailBy.BindWithTitle(detail.Name, "Hoge")))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details = {New DetailVo With {.Name = "1"},
                                                                                         New DetailVo With {.Name = "2"}}}), [Is].EqualTo(
                            "Head Hoge" & vbCrLf &
                            "'a'  '1' " & vbCrLf &
                            "'a'  '2' "))
            End Sub

            <Test()> Public Sub 内包する配列の値を出力できる_2段階()
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.Bind(vo.Name).JoinDetails(vo.Details,
                                                          Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                              detailBy.Bind(detail.Name) _
                                                              .JoinDetails(detail.Values,
                                                                          Function(valueBy As IDebugStringRuleBinder, value As ValueVo) _
                                                                              valueBy.Bind(value.Value))))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details =
                                             {New DetailVo With {.Name = "1", .Values = {New ValueVo With {.Value = "x"}}},
                                              New DetailVo With {.Name = "2", .Values = {New ValueVo With {.Value = "y"},
                                                                                         New ValueVo With {.Value = "z"}}}}}), [Is].EqualTo(
                            "Name D.Name D.V.Value" & vbCrLf &
                            "'a'  '1'    'x'      " & vbCrLf &
                            "'a'  '2'    'y'      " & vbCrLf &
                            "'a'  '2'    'z'      "))
            End Sub

            <Test()> Public Sub 内包するList値を出力できる()
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.Bind(vo.Name).JoinDetails(vo.Details,
                                                          Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                              detailBy.Bind(detail.Name) _
                                                              .JoinDetails(detail.Values,
                                                                          Function(valueBy As IDebugStringRuleBinder, value As ValueVo) _
                                                                              valueBy.Bind(value.Value))))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details =
                                             ({New DetailVo With {.Name = "1",
                                                                  .Values = ({New ValueVo With {.Value = "x"}}).ToList}}).ToList}), [Is].EqualTo(
                            "Name D.Name D.V.Value" & vbCrLf &
                            "'a'  '1'    'x'      "))
            End Sub

            <Test()> Public Sub 取得したDetailがnullなら_項目値はすべてnullになる()
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.BindWithTitle(vo.Name, "Head").JoinDetails(vo.Details,
                                                                           Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                                               detailBy.Bind(detail.Name, detail.Middle)))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a"}), [Is].EqualTo(
                            "Head D.Name D.Middle" & vbCrLf &
                            "'a'  null   null    "))
            End Sub

            <Test()> Public Sub 取得したDetailが長さ0なら_項目値はすべてnullになる()
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.BindWithTitle(vo.Name, "Head").JoinDetails(vo.Details,
                                                                           Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                                               detailBy.Bind(detail.Name, detail.Middle)))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details = New DetailVo() {}}), [Is].EqualTo(
                            "Head D.Name D.Middle" & vbCrLf &
                            "'a'  null   null    "))
            End Sub

            <Test()> Public Sub 異なる複数の明細をもってたとき_少ない明細値はnullになる()
                Dim maker As New DebugStringMaker(Of HeaderVoWithTwoDetails)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVoWithTwoDetails) _
                        defineBy.BindWithTitle(vo.Name, "Head") _
                        .JoinDetails(vo.ADetails, Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) detailBy.Bind(detail.Name)) _
                        .JoinDetails(vo.BDetails, Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) detailBy.Bind(detail.Name)))
                Assert.That(maker.MakeString(New HeaderVoWithTwoDetails With {.Name = "a", .ADetails = {New DetailVo With {.Name = "x"}},
                                                                 .BDetails = {New DetailVo With {.Name = "y"}, New DetailVo With {.Name = "z"}}}), [Is].EqualTo(
                            "Head A.Name B.Name" & vbCrLf &
                            "'a'  'x'    'y'   " & vbCrLf &
                            "'a'  null   'z'   "))
            End Sub

            <Test()> Public Sub いろいろ異なる複数の明細をもっていても表示できる()
                Dim maker As New DebugStringMaker(Of HeaderVoWithTwoDetails)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVoWithTwoDetails) _
                        defineBy.BindWithTitle(vo.Name, "Head") _
                        .JoinDetails(vo.ADetails, Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) detailBy.Bind(detail.Name)) _
                        .JoinDetails(vo.BDetails, Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) detailBy.Bind(detail.Name)))
                Assert.That(maker.MakeString(New HeaderVoWithTwoDetails With {.Name = "a", .ADetails = {New DetailVo With {.Name = "x"}},
                                                                              .BDetails = {New DetailVo With {.Name = "y"}, New DetailVo With {.Name = "z"}}},
                                             New HeaderVoWithTwoDetails With {.Name = "b", .ADetails = {New DetailVo With {.Name = "d"}, New DetailVo With {.Name = "e"}, New DetailVo With {.Name = "f"}},
                                                                              .BDetails = New DetailVo() {}}), [Is].EqualTo(
                            "Head A.Name B.Name" & vbCrLf &
                            "'a'  'x'    'y'   " & vbCrLf &
                            "'a'  null   'z'   " & vbCrLf &
                            "'b'  'd'    null  " & vbCrLf &
                            "'b'  'e'    null  " & vbCrLf &
                            "'b'  'f'    null  "))
            End Sub

        End Class

        Public Class BindSideBySideByMakeStringTest : Inherits DebugStringMakerTest

            <Test()> Public Sub 内包する配列の値を横並びで出力できる()
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.BindWithTitle(vo.Name, "Head").JoinFromSideBySide(vo.Details,
                                                                                   Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                                                       detailBy.Bind(detail.Name)))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details = {New DetailVo With {.Name = "1"},
                                                                                         New DetailVo With {.Name = "2"}}}), [Is].EqualTo(
                            "Head D.Name#0 D.Name#1" & vbCrLf &
                            "'a'  '1'      '2'     "))
            End Sub

            <Test()> Public Sub 内包する配列の値を横並びで出力できる2()
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.JoinFromSideBySide(vo.Details,
                                                    Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                        detailBy.Bind(detail.Name)))
                Assert.That(maker.MakeString(New HeaderVo With {.Details = {New DetailVo With {.Name = "3"},
                                                                            New DetailVo With {.Name = "4"}}}), [Is].EqualTo(
                            "D.Name#0 D.Name#1" & vbCrLf &
                            "'3'      '4'     "))
            End Sub

            <Test()> Public Sub 値を横並びで出力できる_MakeSideBySide()
                Dim maker As New DebugStringMaker(Of DetailVo)(
                                                    Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                        detailBy.Bind(detail.Name))
                Assert.That(maker.MakeSideBySide(New DetailVo With {.Name = "5"},
                                                 New DetailVo With {.Name = "6"}), [Is].EqualTo(
                            "Name#0 Name#1" & vbCrLf &
                            "'5'    '6'   "))
            End Sub

            <Test()> Public Sub 内包する配列の値を横並びで出力できる_複数項目を横並びできる()
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.BindWithTitle(vo.Name, "Head").JoinFromSideBySide(vo.Details,
                                                                                   Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                                                       detailBy.Bind(detail.Name, detail.Middle)))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details = {New DetailVo With {.Name = "1", .Middle = "x"},
                                                                                         New DetailVo With {.Name = "2", .Middle = "y"}}}), [Is].EqualTo(
                            "Head D.Name#0 D.Middle#0 D.Name#1 D.Middle#1" & vbCrLf &
                            "'a'  '1'      'x'        '2'      'y'       "))
            End Sub

            <Test()> Public Sub 値を横並びで出力できる_複数項目を横並びできる_MakeSideBySide()
                Dim maker As New DebugStringMaker(Of DetailVo)(
                    Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                        detailBy.Bind(detail.Name, detail.Middle))
                Assert.That(maker.MakeSideBySide(New DetailVo With {.Name = "1", .Middle = "x"},
                                                 New DetailVo With {.Name = "2", .Middle = "y"}), [Is].EqualTo(
                            "Name#0 Middle#0 Name#1 Middle#1" & vbCrLf &
                            "'1'    'x'      '2'    'y'     "))
            End Sub

            <TestCase(Nothing)>
            <TestCase(0)>
            Public Sub 内包する配列の値を横並びで出力できる_長さ0の配列なら_列そのものを出力しない(length As Object)
                Dim details As DetailVo() = If(IsNumeric(length), Enumerable.Range(0, CInt(length)).Select(Function(i) New DetailVo With {.Name = "NG"}).ToArray, Nothing)
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.BindWithTitle(vo.Name, "Head").JoinFromSideBySide(vo.Details,
                                                                                   Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                                                       detailBy.Bind(detail.Name, detail.Middle)))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details = details}), [Is].EqualTo(
                            "Head" & vbCrLf &
                            "'a' "))
            End Sub

            <Test()> Public Sub 内包する配列の値を横並びで出力できる_最大配列数に応じてnullで穴埋めできる()
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.Bind(vo.Name).JoinDetails(vo.Details,
                                                           Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                               detailBy.Bind(detail.Name).JoinFromSideBySide(detail.Values,
                                                                                                             Function(valueBy As IDebugStringRuleBinder, value As ValueVo) _
                                                                                                                 valueBy.Bind(value.Value))))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details =
                                             {New DetailVo With {.Name = "1", .Values = {New ValueVo With {.Value = "x"}}},
                                              New DetailVo With {.Name = "2", .Values = {New ValueVo With {.Value = "y"},
                                                                                         New ValueVo With {.Value = "z"}}},
                                              New DetailVo With {.Name = "3", .Values = {New ValueVo With {.Value = "d"},
                                                                                         New ValueVo With {.Value = "e"},
                                                                                         New ValueVo With {.Value = "f"}}}}}), [Is].EqualTo(
                            "Name D.Name D.V.Value#0 D.V.Value#1 D.V.Value#2" & vbCrLf &
                            "'a'  '1'    'x'         null        null       " & vbCrLf &
                            "'a'  '2'    'y'         'z'         null       " & vbCrLf &
                            "'a'  '3'    'd'         'e'         'f'        "))
            End Sub

            <Test()> Public Sub 値を横並びで出力できる_横並びの入れ子_MakeSideBySide()
                Dim maker As New DebugStringMaker(Of DetailVo)(
                    Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                        detailBy.Bind(detail.Name).JoinFromSideBySide(detail.Values,
                                                                      Function(valueBy As IDebugStringRuleBinder, value As ValueVo) _
                                                                          valueBy.Bind(value.Value)))
                Assert.That(maker.MakeSideBySide(New DetailVo With {.Name = "1", .Values = {New ValueVo With {.Value = "x"}}},
                                                 New DetailVo With {.Name = "2", .Values = {New ValueVo With {.Value = "y"},
                                                                                            New ValueVo With {.Value = "z"}}},
                                                 New DetailVo With {.Name = "3", .Values = {New ValueVo With {.Value = "d"},
                                                                                            New ValueVo With {.Value = "e"},
                                                                                            New ValueVo With {.Value = "f"}}}), [Is].EqualTo(
                "Name#0 V.Value#0#0 V.Value#1#0 V.Value#2#0 Name#1 V.Value#0#1 V.Value#1#1 V.Value#2#1 Name#2 V.Value#0#2 V.Value#1#2 V.Value#2#2" & vbCrLf &
                "'1'    'x'         null        null        '2'    'y'         'z'         null        '3'    'd'         'e'         'f'        "))
            End Sub

            <Test()> Public Sub 内包する配列の値を横並びで出力できる_最大配列数に応じてnullで穴埋めできる_複数項目を繰り返し表示できる()
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.Bind(vo.Name).JoinDetails(vo.Details,
                                                           Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                               detailBy.Bind(detail.Name).JoinFromSideBySide(detail.Values,
                                                                                                             Function(valueBy As IDebugStringRuleBinder, value As ValueVo) _
                                                                                                                 valueBy.Bind(value.Name, value.Value))))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details =
                                             {New DetailVo With {.Name = "1", .Values = {New ValueVo With {.Value = "x", .Name = "s"}}},
                                              New DetailVo With {.Name = "2", .Values = {New ValueVo With {.Value = "y"},
                                                                                         New ValueVo With {.Value = "z", .Name = "t"}}},
                                              New DetailVo With {.Name = "3", .Values = {New ValueVo With {.Value = "d"},
                                                                                         New ValueVo With {.Value = "e"},
                                                                                         New ValueVo With {.Value = "f", .Name = "u"}}}}}), [Is].EqualTo(
                            "Name D.Name D.V.Name#0 D.V.Value#0 D.V.Name#1 D.V.Value#1 D.V.Name#2 D.V.Value#2" & vbCrLf &
                            "'a'  '1'    's'        'x'         null       null        null       null       " & vbCrLf &
                            "'a'  '2'    null       'y'         't'        'z'         null       null       " & vbCrLf &
                            "'a'  '3'    null       'd'         null       'e'         'u'        'f'        "))
            End Sub

            <Test()> Public Sub 内包する配列の値を横並びで出力できる_最大配列数に応じてnullで穴埋めできる_Headerが異なってもok()
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.Bind(vo.Name).JoinDetails(vo.Details,
                                                           Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                               detailBy.Bind(detail.Name).JoinFromSideBySide(detail.Values,
                                                                                                             Function(valueBy As IDebugStringRuleBinder, value As ValueVo) _
                                                                                                                 valueBy.Bind(value.Value))))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details =
                                             {New DetailVo With {.Name = "1", .Values = {New ValueVo With {.Value = "x"}}}}},
                                             New HeaderVo With {.Name = "b", .Details =
                                             {New DetailVo With {.Name = "2", .Values = {New ValueVo With {.Value = "y"},
                                                                                         New ValueVo With {.Value = "z"}}}}}), [Is].EqualTo(
                            "Name D.Name D.V.Value#0 D.V.Value#1" & vbCrLf &
                            "'a'  '1'    'x'         null       " & vbCrLf &
                            "'b'  '2'    'y'         'z'        "))
            End Sub

            <Test()> Public Sub 内包する配列の値を横並びで出力できる_最大配列数に応じてnullで穴埋めできる_いろいろ()
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.Bind(vo.Name).JoinDetails(vo.Details,
                                                           Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                               detailBy.Bind(detail.Name).JoinFromSideBySide(detail.Values,
                                                                                                             Function(valueBy As IDebugStringRuleBinder, value As ValueVo) _
                                                                                                                 valueBy.Bind(value.Value))))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details =
                                             {New DetailVo With {.Name = "1", .Values = {New ValueVo With {.Value = "x"}}},
                                              New DetailVo With {.Name = "2", .Values = {New ValueVo With {.Value = "y"},
                                                                                         New ValueVo With {.Value = "z"}}}}},
                                             New HeaderVo With {.Name = "b", .Details =
                                             {New DetailVo With {.Name = "3", .Values = {New ValueVo With {.Value = "d"},
                                                                                         New ValueVo With {.Value = "e"},
                                                                                         New ValueVo With {.Value = "f"}}}}}), [Is].EqualTo(
                            "Name D.Name D.V.Value#0 D.V.Value#1 D.V.Value#2" & vbCrLf &
                            "'a'  '1'    'x'         null        null       " & vbCrLf &
                            "'a'  '2'    'y'         'z'         null       " & vbCrLf &
                            "'b'  '3'    'd'         'e'         'f'        "))
            End Sub

            <TestCase(Nothing)>
            <TestCase(0)>
            Public Sub 内包する配列の値を横並びで出力できる_最大配列数に応じてnullで穴埋めできる_Headerが異なっても全部null埋めできる(length As Object)
                Dim values As ValueVo() = If(IsNumeric(length), Enumerable.Range(0, CInt(length)).Select(Function(i) New ValueVo With {.Value = "NG"}).ToArray, Nothing)
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.Bind(vo.Name).JoinDetails(vo.Details,
                                                           Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                               detailBy.Bind(detail.Name).JoinFromSideBySide(detail.Values,
                                                                                                             Function(valueBy As IDebugStringRuleBinder, value As ValueVo) _
                                                                                                                 valueBy.Bind(value.Value))))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details =
                                             {New DetailVo With {.Name = "1", .Values = values}}},
                                             New HeaderVo With {.Name = "b", .Details =
                                             {New DetailVo With {.Name = "2", .Values = {New ValueVo With {.Value = "E"},
                                                                                         New ValueVo With {.Value = "d"},
                                                                                         New ValueVo With {.Value = "o"}}}}}), [Is].EqualTo(
                            "Name D.Name D.V.Value#0 D.V.Value#1 D.V.Value#2" & vbCrLf &
                            "'a'  '1'    null        null        null       " & vbCrLf &
                            "'b'  '2'    'E'         'd'         'o'        "))
            End Sub

            <TestCase(Nothing)>
            <TestCase(0)>
            Public Sub 内包する配列の値を横並びで出力できる_ValuesがEmptyでも_固定繰り返し数が2だから_2回までnull埋めする(length As Object)
                Dim values As ValueVo() = If(IsNumeric(length), Enumerable.Range(0, CInt(length)).Select(Function(i) New ValueVo With {.Value = "NG"}).ToArray, Nothing)
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.Bind(vo.Name).JoinDetails(vo.Details,
                                                           Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                               detailBy.Bind(detail.Name).JoinFromSideBySide(detail.Values, 2,
                                                                                                             Function(valueBy As IDebugStringRuleBinder, value As ValueVo) _
                                                                                                                 valueBy.Bind(value.Value))))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details =
                                             {New DetailVo With {.Name = "1", .Values = values}}}), [Is].EqualTo(
                            "Name D.Name D.V.Value#0 D.V.Value#1" & vbCrLf &
                            "'a'  '1'    null        null       "))
            End Sub

            <Test()> Public Sub 内包する配列の値を横並びで出力できる_Valuesが3つあっても_固定繰り返し数が2だから_2回まで()
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.Bind(vo.Name).JoinDetails(vo.Details,
                                                           Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                               detailBy.Bind(detail.Name).JoinFromSideBySide(detail.Values, 2,
                                                                                                             Function(valueBy As IDebugStringRuleBinder, value As ValueVo) _
                                                                                                                 valueBy.Bind(value.Value))))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details =
                                             {New DetailVo With {.Name = "1", .Values = {New ValueVo With {.Value = "E"},
                                                                                         New ValueVo With {.Value = "d"},
                                                                                         New ValueVo With {.Value = "o"}}}}}), [Is].EqualTo(
                            "Name D.Name D.V.Value#0 D.V.Value#1" & vbCrLf &
                            "'a'  '1'    'E'         'd'        "), "3レコード目'o'は出力しない")
            End Sub

            <TestCase(Nothing)>
            <TestCase(0)>
            Public Sub 内包する配列の値を横並びで出力できる_横並びの指定の親Voが_Emptyでも_null埋めできる(length As Object)
                Dim details As DetailVo() = If(IsNumeric(length), Enumerable.Range(0, CInt(length)).Select(Function(i) New DetailVo With {.Name = "NG"}).ToArray, Nothing)
                Dim maker As New DebugStringMaker(Of HeaderVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As HeaderVo) _
                        defineBy.Bind(vo.Name).JoinDetails(vo.Details,
                                                           Function(detailBy As IDebugStringRuleBinder, detail As DetailVo) _
                                                               detailBy.Bind(detail.Name).JoinFromSideBySide(detail.Values,
                                                                                                             Function(valueBy As IDebugStringRuleBinder, value As ValueVo) _
                                                                                                                 valueBy.Bind(value.Value))))
                Assert.That(maker.MakeString(New HeaderVo With {.Name = "a", .Details = details},
                                             New HeaderVo With {.Name = "b", .Details =
                                             {New DetailVo With {.Name = "2", .Values = {New ValueVo With {.Value = "L"},
                                                                                         New ValueVo With {.Value = "R"}}}}}), [Is].EqualTo(
                            "Name D.Name D.V.Value#0 D.V.Value#1" & vbCrLf &
                            "'a'  null   null        null       " & vbCrLf &
                            "'b'  '2'    'L'         'R'        "))
            End Sub

        End Class

        Public Class MakeString_BindLambdaWithTitleTest : Inherits DebugStringMakerTest

            <Test()> Public Sub 引数付きのメソッドを指定できる()
                Dim maker As New DebugStringMaker(Of Fuga)(Function(defineBy As IDebugStringRuleBinder, vo As Fuga) _
                                                                  defineBy.BindFuncWithTitle(Function() vo.GetValue("A"), "B"))
                Dim f As New Fuga
                f.SetValue(key:="A", value:="V")
                Assert.That(maker.MakeString(f), [Is].EqualTo(
                            "B  " & vbCrLf &
                            "'V'"))
            End Sub

            <Test()> Public Sub プロパティを指定できる_BindWithTitleメソッドと結果は同じ()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.BindFuncWithTitle(Function() vo.Name, "P"))
                Assert.That(maker.MakeString(New HogeVo With {.Name = "N"}), [Is].EqualTo(
                            "P  " & vbCrLf &
                            "'N'"))
            End Sub

            <Test()> Public Sub String定数を指定できる()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.BindFuncWithTitle(Function() "W", "C"))
                Assert.That(maker.MakeString(New HogeVo With {.Name = "N"}), [Is].EqualTo(
                            "C  " & vbCrLf &
                            "'W'"))
            End Sub

            <Test()> Public Sub 数値定数を指定できる()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.BindFuncWithTitle(Function() 1.234D, "D"))
                Assert.That(maker.MakeString(New HogeVo With {.Name = "N"}), [Is].EqualTo(
                            "D    " & vbCrLf &
                            "1.234"))
            End Sub

            <Test()> Public Sub 簡単な演算処理を指定できる()
                Dim maker As New DebugStringMaker(Of HogeVo)(Function(defineBy As IDebugStringRuleBinder, vo As HogeVo) _
                                                                  defineBy.BindFuncWithTitle(Function() "*" & vo.Name, "C"))
                Assert.That(maker.MakeString(New HogeVo With {.Name = "N"}), [Is].EqualTo(
                            "C   " & vbCrLf &
                            "'*N'"))
            End Sub

            <Test()> Public Sub Boolean型三項目以上でも_Boolean型のプロパティを出力できる()
                Dim maker As New DebugStringMaker(Of Boolean3Vo)(Function(defineBy As IDebugStringRuleBinder, vo As Boolean3Vo) _
                                                                  defineBy.BindFuncWithTitle(Function() vo.Bool1, "1"))
                Assert.That(maker.MakeString(New Boolean3Vo With {.Bool1 = True}), [Is].EqualTo(
                            "1   " & vbCrLf &
                            "True"))
            End Sub

            <Test()> Public Sub Int型配列でMakeStringできる()
                Dim values As String() = {"c", "b", "a"}
                Dim maker As New DebugStringMaker(Of Integer)(Function(defineBy As IDebugStringRuleBinder, index As Integer) _
                                                                  defineBy.BindFuncWithTitle(Function() values(index), "ttl"))
                Assert.That(maker.MakeString(Enumerable.Range(0, values.Length)), [Is].EqualTo(
                            "ttl" & vbCrLf &
                            "'c'" & vbCrLf &
                            "'b'" & vbCrLf &
                            "'a'"))
            End Sub

        End Class

        Public Class ValueObjectを出力Test : Inherits DebugStringMakerTest

            Private Class PrimitiveId : Inherits ValueObject
                Friend ReadOnly Value As Integer
                Public Sub New(value As Integer)
                    Me.Value = value
                End Sub
                Protected Overrides Function GetAtomicValues() As IEnumerable(Of Object)
                    Return New Object() {Value}
                End Function
            End Class
            Private Class PrimitiveName : Inherits ValueObject
                Private ReadOnly _value As String
                Friend ReadOnly Property Value As String
                    Get
                        Return _value
                    End Get
                End Property
                Public Sub New(value As String)
                    _value = value
                End Sub
                Protected Overrides Function GetAtomicValues() As IEnumerable(Of Object)
                    Return New Object() {Value}
                End Function
            End Class
            Private Class PrimitiveNameField : Inherits ValueObject
                Friend ReadOnly Value As String
                Public Sub New(value As String)
                    Me.Value = value
                End Sub
                Protected Overrides Function GetAtomicValues() As IEnumerable(Of Object)
                    Return New Object() {Value}
                End Function
            End Class
            Private Class Parent : Inherits ValueObject
                Public ReadOnly Id As PrimitiveId
                Public ReadOnly Name As PrimitiveName
                Public Sub New(Id As Integer, Name As String)
                    Me.New(New PrimitiveId(Id), New PrimitiveName(Name))
                End Sub
                Public Sub New(Id As PrimitiveId, Name As PrimitiveName)
                    Me.Id = Id
                    Me.Name = Name
                End Sub
                Protected Overrides Function GetAtomicValues() As IEnumerable(Of Object)
                    Return New Object() {Id, Name}
                End Function
            End Class
            Private Class Grand : Inherits ValueObject
                Public ReadOnly Parent As Parent
                Public ReadOnly FamilyName As PrimitiveName
                Public Sub New(Id As PrimitiveId, Name As PrimitiveName, familyName As PrimitiveName)
                    Me.New(New Parent(Id, Name), familyName)
                End Sub
                Public Sub New(parent As Parent, familyName As PrimitiveName)
                    Me.Parent = parent
                    Me.FamilyName = familyName
                End Sub
                Protected Overrides Function GetAtomicValues() As IEnumerable(Of Object)
                    Return New Object() {Parent, FamilyName}
                End Function
            End Class
            Private Class GrandEntity
                Public Property Parent As Parent
                Public Property FamilyName As PrimitiveName
                Public Sub New()
                End Sub
                Public Sub New(parent As Parent, familyName As PrimitiveName)
                    Me.Parent = parent
                    Me.FamilyName = familyName
                End Sub
            End Class
            Private Class ParentGrand : Inherits ValueObject
                Public ReadOnly Grand As Grand
                Public ReadOnly MiddleName As String
                Public Sub New(grand As Grand, middleName As String)
                    Me.Grand = grand
                    Me.MiddleName = middleName
                End Sub
                Protected Overrides Function GetAtomicValues() As IEnumerable(Of Object)
                    Return New Object() {Grand, MiddleName}
                End Function
            End Class
            Private Class ParentCollection : Inherits CollectionObject(Of Parent)
                Public Sub New()
                End Sub
                Public Sub New(ByVal initialList As IEnumerable(Of Parent))
                    MyBase.New(initialList)
                End Sub
                Public Sub New(ByVal src As CollectionObject(Of Parent))
                    MyBase.New(src)
                End Sub
            End Class
            Private Class GrandVoIncludedCollection
                Public FamilyName As PrimitiveName
                Private _parents As ParentCollection
                Public ReadOnly Property Parents As ParentCollection
                    Get
                        Return _parents
                    End Get
                End Property
                Public Sub New()
                    Me.New(DirectCast(Nothing, ParentCollection))
                End Sub
                Public Sub New(initialList As IEnumerable(Of Parent))
                    Me.New(New ParentCollection(initialList))
                End Sub
                Public Sub New(Parents As ParentCollection)
                    Me._parents = If(Parents, New ParentCollection)
                End Sub
            End Class
            Private Class GrandVoCollection : Inherits CollectionObject(Of GrandVoIncludedCollection)
                Public Sub New()
                End Sub
                Public Sub New(ByVal initialList As IEnumerable(Of GrandVoIncludedCollection))
                    MyBase.New(initialList)
                End Sub
                Public Sub New(ByVal src As CollectionObject(Of GrandVoIncludedCollection))
                    MyBase.New(src)
                End Sub
            End Class
            Private Class FamilyVoIncludedCollection
                Public Name As PrimitiveName
                Private _grands As GrandVoCollection
                Public ReadOnly Property Grands As GrandVoCollection
                    Get
                        Return _grands
                    End Get
                End Property
                Public Sub New()
                    Me.New(DirectCast(Nothing, GrandVoCollection))
                End Sub
                Public Sub New(ByVal initialList As IEnumerable(Of GrandVoIncludedCollection))
                    Me.New(New GrandVoCollection(initialList))
                End Sub
                Public Sub New(Grands As GrandVoCollection)
                    Me._grands = If(Grands, New GrandVoCollection)
                End Sub
            End Class
            Private Class VoIncludedROProperty : Inherits ValueObject
                Public ReadOnly Name As PrimitiveName
                Private _parents As New CollectionValueObject(Of Parent)
                Public ReadOnly Property Parents As CollectionValueObject(Of Parent)
                    Get
                        Return _parents
                    End Get
                End Property
                Public Sub New(name As PrimitiveName, parents As IEnumerable(Of Parent))
                    Me.Name = name
                    Me._parents = New CollectionValueObject(Of Parent)(parents)
                End Sub
                Protected Overrides Function GetAtomicValues() As IEnumerable(Of Object)
                    Return New Object() {Name, _parents}
                End Function
            End Class
            Private Class VoPrimitiveNamesProperty
                Public Property Names As PrimitiveName()
            End Class
            Private Class VoPrimitiveNamesField
                Public Names As PrimitiveName()
            End Class
            Private Class NamesVo
                Public Property Names As NameVo()
            End Class
            Private Class NameVo
                Public Property Name As String
                Public Sub New()
                End Sub
                Public Sub New(name As String)
                    Me.Name = name
                End Sub
            End Class
            Private Class VoNamesProperty
                Public Property Names As String()
            End Class
            Private Class VoNamesField
                Public Names As String()
            End Class
            Private Class Pvo : Inherits PrimitiveValueObject(Of String)
                Public Sub New(value As String)
                    MyBase.New(value)
                End Sub
            End Class
            Private Class PvoContainer : Inherits ValueObject
                Public Property PvoA As Pvo
                Public Property PvoB As Pvo
                Protected Overrides Function GetAtomicValues() As IEnumerable(Of Object)
                    Return New Object() {PvoA, PvoB}
                End Function
                Public Overrides Function ToString() As String
                    Return PvoA.ToString() & PvoB.ToString()
                End Function
            End Class
            Private Class ContainerParent : Inherits ValueObject
                Public Property InnerVo As PvoContainer
                Protected Overrides Function GetAtomicValues() As IEnumerable(Of Object)
                    Return New Object() {InnerVo}
                End Function
            End Class


            <Test()> Public Sub String型Field値をもつValueObjectで出力できる()
                Dim maker As New DebugStringMaker(Of PrimitiveNameField)(Function(defineBy As IDebugStringRuleBinder, vo As PrimitiveNameField) _
                                                                  defineBy.Bind(vo.Value))
                Assert.That(maker.MakeString(New PrimitiveNameField("X")), [Is].EqualTo(
                            "Value" & vbCrLf &
                            "'X'  "))
            End Sub

            <Test()> Public Sub String型Property値をもつValueObjectで出力できる()
                Dim maker As New DebugStringMaker(Of PrimitiveName)(Function(defineBy As IDebugStringRuleBinder, vo As PrimitiveName) _
                                                                  defineBy.Bind(vo.Value))
                Assert.That(maker.MakeString(New PrimitiveName("Y")), [Is].EqualTo(
                            "Value" & vbCrLf &
                            "'Y'  "))
            End Sub

            <Test()> Public Sub StringやIntのValueObjectを集約しても出力できる()
                Dim maker As New DebugStringMaker(Of Parent)(Function(defineBy As IDebugStringRuleBinder, vo As Parent) _
                                                                  defineBy.Bind(vo.Id, vo.Name))
                Assert.That(maker.MakeString(New Parent(3, "A")), [Is].EqualTo(
                            "Id Name" & vbCrLf &
                            " 3 'A' "))
            End Sub

            <Test()> Public Sub StringやIntのValueObjectを集約しても出力できる_Lambdaでもok()
                Dim maker As New DebugStringMaker(Of Parent)(Function(defineBy As IDebugStringRuleBinder, vo As Parent) _
                                                                  defineBy.BindFuncWithTitle(Function() vo.Id.Value, "ID").BindFuncWithTitle(Function() vo.Name.Value, "Nm"))
                Assert.That(maker.MakeString(New Parent(4, "B")), [Is].EqualTo(
                            "ID Nm " & vbCrLf &
                            " 4 'B'"))
            End Sub

            <Test()> Public Sub 多階層のValueObjectでも出力できる_3階層()
                Dim maker As New DebugStringMaker(Of Grand)(Function(defineBy As IDebugStringRuleBinder, vo As Grand) _
                                                                  defineBy.Bind(vo.Parent.Id, vo.Parent.Name, vo.FamilyName))
                Assert.That(maker.MakeString(New Grand(New Parent(5, "C"), New PrimitiveName("Mike"))), [Is].EqualTo(
                            "Id Name FamilyName" & vbCrLf &
                            " 5 'C'  'Mike'    "))
            End Sub

            <Test()> Public Sub 多階層のValueObjectでも出力できる_4階層()
                Dim maker As New DebugStringMaker(Of ParentGrand)(Function(defineBy As IDebugStringRuleBinder, vo As ParentGrand) _
                                                                  defineBy.Bind(vo.Grand.Parent.Name, vo.MiddleName))
                Assert.That(maker.MakeString(New ParentGrand(New Grand(New Parent(6, "D"), New PrimitiveName("Bob")), "Alice")), [Is].EqualTo(
                            "Name MiddleName" & vbCrLf &
                            "'D'  'Alice'   "))
            End Sub

            <Test()> Public Sub CollectionObjectを出力できる()
                Dim maker As New DebugStringMaker(Of GrandVoIncludedCollection)(
                    Function(defineBy As IDebugStringRuleBinder, vo As GrandVoIncludedCollection) _
                        defineBy.Bind(vo.FamilyName).JoinDetails(vo.Parents, Function(parentBy As IDebugStringRuleBinder, vo2 As Parent) _
                                                                                parentBy.Bind(vo2.Id, vo2.Name)))
                Assert.That(maker.MakeString(New GrandVoIncludedCollection(New ParentCollection({New Parent(12, "Bob")})) With {.FamilyName = New PrimitiveName("Frank")}), [Is].EqualTo(
                            "FamilyName P.Id P.Name" & vbCrLf &
                            "'Frank'      12 'Bob' "), "ParentsがCollectionObject")
            End Sub

            <Test()> Public Sub CollectionObjectを出力できる_3明細()
                Dim maker As New DebugStringMaker(Of GrandVoIncludedCollection)(
                    Function(defineBy As IDebugStringRuleBinder, vo As GrandVoIncludedCollection) _
                        defineBy.Bind(vo.FamilyName).JoinDetails(vo.Parents, Function(parentBy As IDebugStringRuleBinder, vo2 As Parent) _
                                                                                parentBy.Bind(vo2.Id, vo2.Name)))
                Assert.That(maker.MakeString(New GrandVoIncludedCollection(New ParentCollection({New Parent(12, "Bob"),
                                                                                                 New Parent(13, "Chris"),
                                                                                                 New Parent(14, "Don")})) With {.FamilyName = New PrimitiveName("Ed")}), [Is].EqualTo(
                            "FamilyName P.Id P.Name " & vbCrLf &
                            "'Ed'         12 'Bob'  " & vbCrLf &
                            "'Ed'         13 'Chris'" & vbCrLf &
                            "'Ed'         14 'Don'  "), "ParentsがCollectionObject")
            End Sub

            <Test()> Public Sub CollectionObjectの中身が空っぽなら_nullで出力する()
                Dim maker As New DebugStringMaker(Of GrandVoIncludedCollection)(
                    Function(defineBy As IDebugStringRuleBinder, vo As GrandVoIncludedCollection) _
                        defineBy.Bind(vo.FamilyName).JoinDetails(vo.Parents, Function(parentBy As IDebugStringRuleBinder, vo2 As Parent) _
                                                                                parentBy.Bind(vo2.Id, vo2.Name)))
                Assert.That(maker.MakeString(New GrandVoIncludedCollection() With {.FamilyName = New PrimitiveName("Jack")}), [Is].EqualTo(
                            "FamilyName P.Id P.Name" & vbCrLf &
                            "'Jack'     null null  "))
            End Sub

            <Test()> Public Sub CollectionObjectの中のCollectionObjectも出力できる()
                Dim maker As New DebugStringMaker(Of FamilyVoIncludedCollection)(
                    Function(defineBy As IDebugStringRuleBinder, vo As FamilyVoIncludedCollection) _
                        defineBy.Bind(vo.Name) _
                        .JoinDetails(vo.Grands,
                                    Function(grandBy As IDebugStringRuleBinder, grand As GrandVoIncludedCollection) _
                                        grandBy.Bind(grand.FamilyName) _
                                        .JoinDetails(grand.Parents, Function(parentBy As IDebugStringRuleBinder, vo2 As Parent) _
                                                                       parentBy.Bind(vo2.Id, vo2.Name))))
                Assert.That(maker.MakeString(New FamilyVoIncludedCollection(
                                             {New GrandVoIncludedCollection({New Parent(9, "Alice"), New Parent(8, "Bob")}) With {.FamilyName = New PrimitiveName("Steve")},
                                              New GrandVoIncludedCollection({New Parent(7, "Gary"), New Parent(6, "Hank")}) With {.FamilyName = New PrimitiveName("Tony")}
                                             }) With {.Name = New PrimitiveName("Ed")}),
                            [Is].EqualTo(
                            "Name G.FamilyName G.P.Id G.P.Name" & vbCrLf &
                            "'Ed' 'Steve'           9 'Alice' " & vbCrLf &
                            "'Ed' 'Steve'           8 'Bob'   " & vbCrLf &
                            "'Ed' 'Tony'            7 'Gary'  " & vbCrLf &
                            "'Ed' 'Tony'            6 'Hank'  "))
            End Sub

            <Test()> Public Sub ValueObjectのなかにReadonlyPropertyがあっても処理できる()
                Dim maker As New DebugStringMaker(Of VoIncludedROProperty)(
                    Function(defineBy As IDebugStringRuleBinder, vo As VoIncludedROProperty) _
                        defineBy.Bind(vo.Name).JoinDetails(vo.Parents, Function(parentBy As IDebugStringRuleBinder, vo2 As Parent) _
                                                                                parentBy.Bind(vo2.Id, vo2.Name)))
                Assert.That(maker.MakeString(New VoIncludedROProperty(New PrimitiveName("Jack"), {New Parent(1, "Ron"), New Parent(2, "Pete")})), [Is].EqualTo(
                            "Name   P.Id P.Name" & vbCrLf &
                            "'Jack'    1 'Ron' " & vbCrLf &
                            "'Jack'    2 'Pete'"))
            End Sub

            <Test()> Public Sub 内包するPrimitiveName配列の値を横並びで出力できる_1項目だけだからBindを省略できる_Property()
                Dim maker As New DebugStringMaker(Of VoPrimitiveNamesProperty)(
                    Function(defineBy As IDebugStringRuleBinder, vo As VoPrimitiveNamesProperty) _
                        defineBy.JoinFromSideBySide(vo.Names))
                Assert.That(maker.MakeString({New VoPrimitiveNamesProperty With {.Names = {New PrimitiveName("a"), New PrimitiveName("b"), New PrimitiveName("c")}},
                                              New VoPrimitiveNamesProperty With {.Names = {New PrimitiveName("z")}}}), [Is].EqualTo(
                            "Names#0 Names#1 Names#2" & vbCrLf &
                            "'a'     'b'     'c'    " & vbCrLf &
                            "'z'     null    null   "))
            End Sub

            <Test()> Public Sub 内包するPrimitiveName配列の値を横並びで出力できる_1項目だけだからBindを省略できる_Field()
                Dim maker As New DebugStringMaker(Of VoPrimitiveNamesField)(
                    Function(defineBy As IDebugStringRuleBinder, vo As VoPrimitiveNamesField) _
                        defineBy.JoinFromSideBySide(vo.Names))
                Assert.That(maker.MakeString({New VoPrimitiveNamesField With {.Names = {New PrimitiveName("a"), New PrimitiveName("b"), New PrimitiveName("c")}},
                                              New VoPrimitiveNamesField With {.Names = {New PrimitiveName("z")}}}), [Is].EqualTo(
                            "Names#0 Names#1 Names#2" & vbCrLf &
                            "'a'     'b'     'c'    " & vbCrLf &
                            "'z'     null    null   "))
            End Sub

            <Test()> Public Sub 内包するPrimitiveName配列の値を横並びで出力できる_Value属性指定なら列名がValueになる()
                Dim maker As New DebugStringMaker(Of VoPrimitiveNamesProperty)(
                    Function(defineBy As IDebugStringRuleBinder, vo As VoPrimitiveNamesProperty) _
                        defineBy.JoinFromSideBySide(vo.Names,
                                                    Function(namesBy As IDebugStringRuleBinder, name As PrimitiveName) namesBy.Bind(name.Value)))
                Assert.That(maker.MakeString({New VoPrimitiveNamesProperty With {.Names = {New PrimitiveName("a"), New PrimitiveName("b"), New PrimitiveName("c")}},
                                              New VoPrimitiveNamesProperty With {.Names = {New PrimitiveName("z")}}}), [Is].EqualTo(
                            "N.Value#0 N.Value#1 N.Value#2" & vbCrLf &
                            "'a'       'b'       'c'      " & vbCrLf &
                            "'z'       null      null     "))
            End Sub

            <Test()> Public Sub 内包するVo配列の値を横並びで出力できる()
                Dim maker As New DebugStringMaker(Of NamesVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As NamesVo) _
                        defineBy.JoinFromSideBySide(vo.Names,
                                                    Function(namesBy As IDebugStringRuleBinder, name As NameVo) namesBy.Bind(name.Name)))
                Assert.That(maker.MakeString({New NamesVo With {.Names = {New NameVo("a"), New NameVo("b"), New NameVo("c")}},
                                              New NamesVo With {.Names = {New NameVo("z")}}}), [Is].EqualTo(
                            "N.Name#0 N.Name#1 N.Name#2" & vbCrLf &
                            "'a'      'b'      'c'     " & vbCrLf &
                            "'z'      null     null    "))
            End Sub

            <Test()> Public Sub 内包するString配列の値を横並びで出力できる_1項目だけだからBindを省略できる_Property()
                Dim maker As New DebugStringMaker(Of VoNamesProperty)(
                    Function(defineBy As IDebugStringRuleBinder, vo As VoNamesProperty) _
                        defineBy.JoinFromSideBySide(vo.Names))
                Assert.That(maker.MakeString({New VoNamesProperty With {.Names = {"a", "b", "c"}},
                                              New VoNamesProperty With {.Names = {"z"}}}), [Is].EqualTo(
                            "Names#0 Names#1 Names#2" & vbCrLf &
                            "'a'     'b'     'c'    " & vbCrLf &
                            "'z'     null    null   "))
            End Sub

            <Test()> Public Sub 内包するString配列の値を横並びで出力できる_1項目だけだからBindを省略できる_Field()
                Dim maker As New DebugStringMaker(Of VoNamesField)(
                    Function(defineBy As IDebugStringRuleBinder, vo As VoNamesField) _
                        defineBy.JoinFromSideBySide(vo.Names))
                Assert.That(maker.MakeString({New VoNamesField With {.Names = {"a", "b", "c"}},
                                              New VoNamesField With {.Names = {"z"}}}), [Is].EqualTo(
                            "Names#0 Names#1 Names#2" & vbCrLf &
                            "'a'     'b'     'c'    " & vbCrLf &
                            "'z'     null    null   "))
            End Sub

            <Test()> Public Sub 内包するString配列の値を横並びで出力できる_1項目だけだからBindを省略できる_ValueObject()
                Dim maker As New DebugStringMaker(Of Grand)(
                    Function(defineBy As IDebugStringRuleBinder, vo As Grand) _
                        defineBy.Bind(vo.FamilyName, vo.Parent.Id, vo.Parent.Name))
                Assert.That(maker.MakeSideBySide({New Grand(New Parent(1, "pn1"), New PrimitiveName("gn1")),
                                                  New Grand(New Parent(2, "pn2"), New PrimitiveName("gn2"))}), [Is].EqualTo(
                            "FamilyName#0 Id#0 Name#0 FamilyName#1 Id#1 Name#1" & vbCrLf &
                            "'gn1'           1 'pn1'  'gn2'           2 'pn2' "))
            End Sub

            <Test()> Public Sub 内包するString配列の値を横並びで出力できる_1項目だけだからBindを省略できる_Entity()
                Dim maker As New DebugStringMaker(Of GrandEntity)(
                    Function(defineBy As IDebugStringRuleBinder, vo As GrandEntity) _
                        defineBy.Bind(vo.FamilyName, vo.Parent.Id, vo.Parent.Name))
                Assert.That(maker.MakeSideBySide({New GrandEntity With {.Parent = New Parent(1, "pn1"), .FamilyName = New PrimitiveName("gn1")},
                                                  New GrandEntity With {.Parent = New Parent(2, "pn2"), .FamilyName = New PrimitiveName("gn2")}}), [Is].EqualTo(
                            "FamilyName#0 Id#0 Name#0 FamilyName#1 Id#1 Name#1" & vbCrLf &
                            "'gn1'           1 'pn1'  'gn2'           2 'pn2' "))
            End Sub

            <Test> Public Sub ValueObjectでToStringに記述した内容をシングルクォートで囲んで出力できる()
                Dim maker As New DebugStringMaker(Of ContainerParent)(Function(defineBy, vo) defineBy.Bind(vo.InnerVo))
                Assert.That(maker.MakeString(New ContainerParent With {.InnerVo = New PvoContainer With {.PvoA = New Pvo("ho"), .PvoB = New Pvo("ge")}}),
                            [Is].EqualTo(
                                "InnerVo" & vbCrLf &
                                "'hoge' "))
            End Sub
        End Class

        Public Class PrimitiveValueObjectを出力Test : Inherits DebugStringMakerTest

            Private Class Sheet : Inherits PrimitiveValueObject(Of String)
                Public Sub New(ByVal value As String)
                    MyBase.New(value)
                End Sub
            End Class
            Private Class Weapon : Inherits PrimitiveValueObject(Of String)
                Public Count As Integer ' 消さないで（これが出力されないことを確認する）
                Public Sub New(ByVal value As String)
                    MyBase.New(value)
                End Sub
            End Class
            Private Class SheetEntity
                Public Name As String
                Public Title As Sheet
                Public Addr As Weapon
            End Class

            <Test()> Public Sub PrimitiveValueObject内にPublicなFieldがあっても_デフォルト出力は_PVOValue()
                Dim maker As New DebugStringMaker(Of SheetEntity)(
                    Function(defineBy As IDebugStringRuleBinder, vo As SheetEntity) _
                        defineBy.Bind(vo.Name, vo.Title, vo.Addr))
                Assert.That(maker.MakeString({New SheetEntity With {.Name = "aaa", .Title = New Sheet("bbb"), .Addr = New Weapon("cc")}
                                             }), [Is].EqualTo(
                            "Name  Title Addr" & vbCrLf &
                            "'aaa' 'bbb' 'cc'"))
            End Sub

            <Test()> Public Sub 指定すれば_PrimitiveValueObject内のPublicFieldを出力できる()
                Dim maker As New DebugStringMaker(Of SheetEntity)(
                    Function(defineBy As IDebugStringRuleBinder, vo As SheetEntity) _
                        defineBy.Bind(vo.Name, vo.Title, vo.Addr.Count))
                Assert.That(maker.MakeString({New SheetEntity With {.Name = "aaa", .Title = New Sheet("bbb"), .Addr = New Weapon("cc") With {.Count = 789}}
                                             }), [Is].EqualTo(
                            "Name  Title Count" & vbCrLf &
                            "'aaa' 'bbb'   789"))
            End Sub

        End Class

        Public Class 非PublicコンストラクタなVoを出力Test_Debug用なんだからPrivateだろうがなんだろうが出力させたい : Inherits DebugStringMakerTest

            Private Class PublicVo
                Public Property Id As Integer
                Public Property Name As String
            End Class
            Private Class PrivateVo : Inherits PublicVo
                Public Shared ReadOnly Instance As New PrivateVo
                Private Sub New()
                End Sub
            End Class
            Private Class FriendVo : Inherits PublicVo
                Friend Sub New()
                End Sub
            End Class

            <Test()> Public Sub Friendコンストラクタでも出力できる()
                Dim maker As New DebugStringMaker(Of FriendVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As FriendVo) _
                        defineBy.Bind(vo.Id, vo.Name))
                Assert.That(maker.MakeString({New FriendVo With {.Id = 8, .Name = "Bob"}}), [Is].EqualTo(
                            "Id Name " & vbCrLf &
                            " 8 'Bob'"))
            End Sub

            <Test()> Public Sub Privateコンストラクタでも出力できる()
                Dim maker As New DebugStringMaker(Of PrivateVo)(
                    Function(defineBy As IDebugStringRuleBinder, vo As PrivateVo) _
                        defineBy.Bind(vo.Id, vo.Name))
                PrivateVo.Instance.Id = 10
                PrivateVo.Instance.Name = "Jeff"
                Assert.That(maker.MakeString({PrivateVo.Instance}), [Is].EqualTo(
                            "Id Name  " & vbCrLf &
                            "10 'Jeff'"))
            End Sub

        End Class

    End Class
End Namespace
