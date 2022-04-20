Public Class Form1
    Dim Canvas As New Drawing.Bitmap(300, 300)
    Dim G As Graphics = Graphics.FromImage(Canvas)
    Dim MyPen As Pen
    Dim drawoptions As Integer
    Dim x1clip, y1clip As Integer
    Dim poly As TPolygonArr
    Dim convex() As Integer
    Dim edgex(), edgey() As Integer
    Dim a, counter, b, c, d As Integer
    Dim normalx(), normaly() As Integer
    Dim checkx1(), checky1(), result1() As Integer
    Dim checkx2(), checky2(), result2() As Integer


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Create Screen
        G.FillRectangle(Brushes.White, 0, 0, Canvas.Width, Canvas.Height)
        PictureBox1.Image = Canvas
        MyPen = New Pen(Color.Blue, Width = 2)
        poly.init()
    End Sub

    Private Sub btnDrawDot_Click(sender As Object, e As EventArgs) Handles btnDrawDot.Click
        'Draw Dot
        PictureBox1.Image = Canvas
        Canvas.SetPixel(txtx1dot.Text, txty1dot.Text, Color.Blue)
    End Sub

    Private Sub btnDrawLine_Click(sender As Object, e As EventArgs) Handles btnDrawLine.Click
        'Draw line using case 
        'drawoptions = 1
        PictureBox1.Image = Canvas
        G.DrawLine(Pens.Black, CInt(txtX1.Text), CInt(txtY1.Text), CInt(txtX2.Text), CInt(txtY2.Text))
    End Sub

    Private Sub btnInsertLast_Click(sender As Object, e As EventArgs) Handles btnInsertLast.Click
        'Insert Last Point for Polygon
        x1clip = txtX1Clip.Text
        y1clip = txtY1Clip.Text
        poly.InsertLast(x1clip, y1clip)
    End Sub

    Private Sub btnInsertFirst_Click(sender As Object, e As EventArgs) Handles btnInsertFirst.Click
        'Insert First Point for Polygon
        x1clip = txtX1Clip.Text
        y1clip = txtY1Clip.Text
        poly.InsertFirst(x1clip, y1clip)
    End Sub
    Private Sub btnClearPolygon_Click(sender As Object, e As EventArgs) Handles btnClearPolygon.Click
        poly.DeleteAll()
    End Sub
    Private Sub btnDeleteLast_Click(sender As Object, e As EventArgs) Handles btnDeleteLast.Click
        poly.DeleteLast()
    End Sub
    Private Sub btnDeleteFirst_Click(sender As Object, e As EventArgs) Handles btnDeleteFirst.Click
        poly.DeleteFirst()
    End Sub

    Private Sub btnFinalizedPolygon_Click(sender As Object, e As EventArgs) Handles btnFinalizedPolygon.Click

        drawoptions = 1
    End Sub
    Private Sub PictureBox1_Paint(sender As Object, e As PaintEventArgs) Handles PictureBox1.Paint
        PictureBox1.Image = Canvas

        'Draw the polygon using case
        Select Case drawoptions
            Case 1
                If poly.n < 3 Then
                    'MessageBox.Show("Please input atleast 3 point.", "Error", MessageBoxButtons.OKCancel)
                    Exit Sub
                ElseIf poly.n >= 3 Then
                    CheckConvex()
                    If a = True Or b = True Then
                        For i = 0 To poly.n - 2
                            e.Graphics.DrawLine(MyPen, poly.element(i).x, poly.element(i).y, poly.element(i + 1).x, poly.element(i + 1).y)
                        Next
                        e.Graphics.DrawLine(MyPen, poly.element(poly.n - 1).x, poly.element(poly.n - 1).y, poly.element(0).x, poly.element(0).y)
                    Else
                        Exit Sub
                    End If
                End If

            Case 2

        End Select
    End Sub
    Private Sub ClearLine()
        'Clear the Line
        G.DrawLine(Pens.White, CInt(txtX1.Text), CInt(txtY1.Text), CInt(txtX2.Text), CInt(txtY2.Text))
    End Sub

    Private Sub btnClearLine_Click(sender As Object, e As EventArgs) Handles btnClearLine.Click
        'Clear Line 
        PictureBox1.Image = Canvas
        ClearLine()
    End Sub

    Private Sub btnClearDot_Click(sender As Object, e As EventArgs) Handles btnClearDot.Click
        'Clear Dot
        PictureBox1.Image = Canvas
        Canvas.SetPixel(txtx1dot.Text, txty1dot.Text, Color.White)
    End Sub
    Private Sub btnClearLineAndDot_Click(sender As Object, e As EventArgs) Handles btnClearLineAndDot.Click
        'Clear All Dot And Line
        PictureBox1.Image = Canvas
        For i = 0 To 299
            For j = 0 To 299
                Canvas.SetPixel(j, i, Color.White)
            Next
        Next
    End Sub
    Private Sub CheckInsideOutsideDot()
        For i = 0 To poly.n - 1
            ReDim Preserve checkx1(i), checky1(i), result1(i)
            checkx1(i) = (txtx1dot.Text - poly.element(i).x)
            checky1(i) = (txty1dot.Text - poly.element(i).y)
            result1(i) = (checkx1(i) * normalx(i)) + (checky1(i) * normaly(i))
        Next
    End Sub

    Private Sub btnClipDot_Click(sender As Object, e As EventArgs) Handles btnClipDot.Click
        PictureBox1.Image = Canvas

        CheckConvex()
        CheckNormal()
        CheckInsideOutsideDot()

        'Check the dot is inside or outside.
        'if c = true , then the dot in inside
        c = True
        counter = 0
        While c = True And counter < poly.n
            If result1(counter) < 0 Then
                c = False
            End If
            counter = counter + 1
        End While

        For i = 0 To poly.n - 1
            'This Mean The Dot inside Clipping
            If c = True Then
                Canvas.SetPixel(txtx1dot.Text, txty1dot.Text, Color.Green)
                'This mean the dot is outside clipping
            Else
                Canvas.SetPixel(txtx1dot.Text, txty1dot.Text, Color.White)
            End If
        Next


    End Sub

    Private Sub Label11_Click(sender As Object, e As EventArgs) Handles Label11.Click

    End Sub

    Private Sub btnClearScreen_Click_1(sender As Object, e As EventArgs) Handles btnClearScreen.Click
        'Clear all the screen 
        PictureBox1.Image = Canvas
        For i = 0 To 299
            For j = 0 To 299
                Canvas.SetPixel(j, i, Color.White)
            Next
        Next
        poly.DeleteAll()
    End Sub

    'Check Convex or not
    Private Sub CheckConvex()

        'Calculate The Edge
        For i = 0 To poly.n - 2
            ReDim Preserve edgex(i), edgey(i)
            edgex(i) = poly.element(i + 1).x - poly.element(i).x
            edgey(i) = poly.element(i + 1).y - poly.element(i).y
        Next
        ReDim Preserve edgex(poly.n - 1), edgey(poly.n - 1)
        edgex(poly.n - 1) = poly.element(0).x - poly.element(poly.n - 1).x
        edgey(poly.n - 1) = poly.element(0).y - poly.element(poly.n - 1).y


        'Calculate the Convex Cross Product
        For i = 0 To poly.n - 2
            ReDim Preserve convex(i)
            convex(i) = (edgex(i) * edgey(i + 1)) - (edgex(i + 1) * edgey(i))
        Next
        ReDim Preserve convex(poly.n - 1)
        convex(poly.n - 1) = (edgex(poly.n - 1) * edgey(0)) - (edgex(0) * edgey(poly.n - 1))

        'For Check all the convex is positive
        'if a = true , then the crossproduct > 0 
        a = True
        counter = 0
        While a = True And counter < poly.n
            If convex(counter) < 0 Then
                a = False
            End If
            counter = counter + 1
        End While

        'For Check all the convex is negative
        'if b = true , then the crossproduct < 0 
        counter = 0
        b = True
        While b = True And counter < poly.n
            If convex(counter) > 0 Then
                b = False
            End If
            counter = counter + 1
        End While
    End Sub

    Private Sub CheckNormal()
        CheckConvex()
        'ClockWise Polygon (Right Normal)
        If b = True Then
            For i = 0 To poly.n - 1
                ReDim Preserve normalx(i), normaly(i)
                normalx(i) = edgey(i)
                normaly(i) = -edgex(i)

            Next
        End If

        'AntiClockWise Polygon (Left Normal
        If a = True Then
            For i = 0 To poly.n - 1
                ReDim Preserve normalx(i), normaly(i)
                normalx(i) = -edgey(i)
                normaly(i) = edgex(i)

            Next
        End If
    End Sub

    Private Sub CheckInsideOutside()
        For i = 0 To poly.n - 1
            ReDim Preserve checkx1(i), checky1(i), checkx2(i), checky2(i), result1(i), result2(i)
            checkx1(i) = (txtX1.Text - poly.element(i).x)
            checky1(i) = (txtY1.Text - poly.element(i).y)
            result1(i) = (checkx1(i) * normalx(i)) + (checky1(i) * normaly(i))

            checkx2(i) = (txtX2.Text - poly.element(i).x)
            checky2(i) = (txtY2.Text - poly.element(i).y)
            result2(i) = (checkx2(i) * normalx(i)) + (checky2(i) * normaly(i))
        Next

    End Sub

    Private Sub btnClipLine_Click(sender As Object, e As EventArgs) Handles btnClipLine.Click
        PictureBox1.Image = Canvas
        Dim tenter(), tleave() As Double
        Dim tentermax, tleavemin As Double
        Dim zx, zy, wx, wy As Double


        CheckConvex()
        CheckNormal()
        CheckInsideOutside()

        'Check  entering/leaving
        For i = 0 To poly.n - 1
            ReDim Preserve tenter(i), tleave(i)


            'This mean Entering
            If result1(i) < 0 And result2(i) > 0 Then
                tenter(i) = ((poly.element(i).x - txtX1.Text) * normalx(i) + (poly.element(i).y - txtY1.Text) * normaly(i)) / ((txtX2.Text - txtX1.Text) * normalx(i) + (txtY2.Text - txtY1.Text) * normaly(i))
                tleave(i) = 0
                'This mean Leaving
            ElseIf result1(i) > 0 And result2(i) < 0 Then
                tleave(i) = ((poly.element(i).x - txtX1.Text) * normalx(i) + (poly.element(i).y - txtY1.Text) * normaly(i)) / ((txtX2.Text - txtX1.Text) * normalx(i) + (txtY2.Text - txtY1.Text) * normaly(i))
                tenter(i) = 0
            Else
                tenter(i) = 0
                tleave(i) = 0
            End If
        Next

        counter = 0
        tentermax = 0
        'Check TEntering Max 
        While counter < poly.n
            If tenter(counter) > tentermax Then
                tentermax = tenter(counter)
            End If
            counter = counter + 1
        End While

        'Check TLeaving Min
        counter = 0
        tleavemin = 1
        While counter < poly.n
            If tleave(counter) > 0 And tleave(counter) < tleavemin Then
                tleavemin = tleave(counter)

            End If
            counter = counter + 1
        End While

        zx = txtX1.Text + tentermax * (txtX2.Text - txtX1.Text)
        zy = txtY1.Text + tentermax * (txtY2.Text - txtY1.Text)

        wx = txtX1.Text + tleavemin * (txtX2.Text - txtX1.Text)
        wy = txtY1.Text + tleavemin * (txtY2.Text - txtY1.Text)


        'G.DrawLine(Pens.White, CInt(txtX1.Text), CInt(txtY1.Text), CInt(zx), CInt(zy))
        'G.DrawLine(Pens.White, CInt(txtX2.Text), CInt(txtY2.Text), CInt(wx), CInt(wy))

        For i = 0 To poly.n - 1

            If result1(i) < 0 And result2(i) < 0 Then
                G.DrawLine(Pens.White, CInt(txtX1.Text), CInt(txtY1.Text), CInt(txtX2.Text), CInt(txtY2.Text))
            ElseIf tentermax < tleavemin Then
                G.DrawLine(Pens.White, CInt(txtX1.Text), CInt(txtY1.Text), CInt(zx), CInt(zy))
                G.DrawLine(Pens.White, CInt(txtX2.Text), CInt(txtY2.Text), CInt(wx), CInt(wy))
            End If
        Next
    End Sub

    Private Sub btnCheckLine_Click(sender As Object, e As EventArgs) Handles btnCheckLine.Click
        PictureBox1.Image = Canvas



        CheckConvex()
        CheckNormal()
        CheckInsideOutside()


        'For Check all the result in inside
        'if d = true , then the result in inside
        d = True
        counter = 0
        While d = True And counter < poly.n
            If result1(counter) < 0 Or result2(counter) < 0 Then
                d = False
            End If
            counter = counter + 1
        End While

        'Check Whether the line accepted/rejected.
        For i = 0 To poly.n - 1
            'This Mean All inside ( Trivially Accepted)
            If d = True Then
                G.DrawLine(Pens.Green, CInt(txtX1.Text), CInt(txtY1.Text), CInt(txtX2.Text), CInt(txtY2.Text))
                'This mean there is one row outside (Trivially Rejected)
            ElseIf result1(i) And result2(i) < 0 Then
                G.DrawLine(Pens.Red, CInt(txtX1.Text), CInt(txtY1.Text), CInt(txtX2.Text), CInt(txtY2.Text))

            End If
        Next



    End Sub

    Public Structure TPoint
        Dim x As Integer
        Dim y As Integer

        Public Sub SetPoint(ByVal px As Integer, ByVal py As Integer)
            x = px
            y = py
        End Sub
    End Structure



    Public Structure TPolygonArr
        Dim n As Integer
        Dim element() As TPoint

        Public Sub init()
            n = 0
            ReDim element(-1)
        End Sub

        Public Sub InsertLast(px As Integer, py As Integer)
            Dim p As TPoint
            p.SetPoint(px, py)
            n = n + 1
            ReDim Preserve element(n - 1)
            element(n - 1) = p

        End Sub

        Public Sub InsertFirst(px As Integer, py As Integer)
            Dim p As TPoint
            Dim i As Integer
            p.SetPoint(px, py)
            n = n + 1
            ReDim Preserve element(n - 1)

            For i = n - 1 To 1 Step -1
                element(i) = element(i - 1)
            Next
            element(0) = p
        End Sub

        Public Sub DeleteLast()
            n = n - 1
            ReDim Preserve element(n - 1)
        End Sub

        Public Sub DeleteFirst()
            n = n - 1

            For i = 0 To n - 1
                element(i) = element(i + 1)
            Next
            ReDim Preserve element(n - 1)

        End Sub

        Public Sub DeleteAll()
            n = 0
            ReDim Preserve element(n - 1)
        End Sub

    End Structure

    Private Sub PictureBox1_MouseClick(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseClick 'Make a dot 
        PictureBox1.Image = Canvas
        txtx1dot.Text = e.X
        txty1dot.Text = e.Y
        txtX1Clip.Text = e.X
        txtY1Clip.Text = e.Y


    End Sub



    Private Sub PictureBox1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        txtX1.Text = e.X
        txtY1.Text = e.Y
    End Sub



    Private Sub PictureBox1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseUp
        txtX2.Text = e.X
        txtY2.Text = e.Y
    End Sub




End Class

