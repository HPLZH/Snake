Imports System.Net
Imports Snake.Core

Public Class Form1

    Dim game As New Game(80, 60)
    Dim ctrl As New GameController(80, 60, True)

    Dim myId As Integer
    Dim code As Integer

    Dim tailBrush = Brushes.Gray
    Dim myHeadBrush = Brushes.Blue
    Dim othersHeadBrush = Brushes.Red
    Dim foodBrush = Brushes.Green

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        game.Next()
        Draw(game)
    End Sub

    Private Sub Draw(info As IGameInformation)
        Dim tails = info.Tails()
        Dim heads = info.Heads()
        Dim food = info.Food()

        PictureBox1.Image = New Bitmap(1600, 1200)
        Dim g As Graphics = Graphics.FromImage(PictureBox1.Image)
        For Each food1 In food
            g.FillRectangle(foodBrush, food1.x * 20, food1.y * 20, 20, 20)
        Next
        For Each tail In tails
            g.FillRectangle(tailBrush, tail.x * 20, tail.y * 20, 20, 20)
        Next
        For Each headKvp In heads
            If headKvp.Key = myId Then
                g.FillRectangle(myHeadBrush, headKvp.Value.x * 20, headKvp.Value.y * 20, 20, 20)
            Else
                g.FillRectangle(othersHeadBrush, headKvp.Value.x * 20, headKvp.Value.y * 20, 20, 20)
            End If
        Next
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        'myId = game.NextFreeId()
        'game.AddSnake(myId)
        ctrl.Next = Sub(s) Draw(ctrl)
        myId = ctrl.NextFreeId()
        ctrl.AddPlayer(myId, "Tester", code)
        ctrl.Start()
    End Sub

    Private Sub Form1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        Select Case e.KeyChar
            Case "w", "W"
                ctrl.Input(myId, code, Game.Direction.Up)
            Case "s", "S"
                ctrl.Input(myId, code, Game.Direction.Down)
            Case "a", "A"
                ctrl.Input(myId, code, Game.Direction.Left)
            Case "d", "D"
                ctrl.Input(myId, code, Game.Direction.Right)
        End Select
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Shift Then
            ctrl.Input(myId, code, 1)
        End If
    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If Not e.Shift Then
            ctrl.Input(myId, code, 0)
        End If
    End Sub
End Class
