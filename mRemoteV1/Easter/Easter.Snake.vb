Imports System.Threading.Thread
Imports System.IO

Namespace Easter
    Public Class Snake
        Public Class Game
            Private Shared _Mode As GameMode
            Public Shared Property Mode() As GameMode
                Get
                    Return _Mode
                End Get
                Set(ByVal value As GameMode)
                    Dim prevMode As GameMode = _Mode
                    _Mode = value
                    GameModeChanged(value, prevMode)
                End Set
            End Property

            Public Shared Player As Snake
            Public Shared Apple As Food
            Public Shared HighScores() As String
            Public Shared Difficulty As Integer = 3
            Public Shared GridLength As Long
            Public Shared GridHeight As Long
            Public Shared TileWidth As Long
            Public Shared TileHeight As Long
            Public Shared WithEvents imgField As PictureBox

            Public Shared Sub CreatePicBox(ByVal Parent As Control)
                'setup the picture box
                Game.imgField = New PictureBox
                Game.imgField.Parent = Parent
                Game.imgField.Size = New Size(100, 100)
                Game.imgField.Location = New Point((Parent.Width / 2) - (imgField.Width / 2), (Parent.Height / 2) - (imgField.Height / 2))
                Game.imgField.BackColor = Color.Black
            End Sub

            Public Shared Sub SetupGame()
                'setup the gamefield
                GridLength = 20
                GridHeight = 20

                'setup the tiles
                TileWidth = 5
                TileHeight = 5

                'create the player (snake)
                Player = New Snake
                Player.CreateSnake(3, 7, 10, Direction.Up)

                'place some food to begin with
                Apple = Food.PlaceFood()
            End Sub

            Private Shared Sub GameModeChanged(ByVal NewMode As GameMode, ByVal PreviousMode As GameMode)
                Select Case NewMode
                    Case GameMode.Welcome
                        SetupGame()
                        RefreshGraphics()
                    Case GameMode.Playing
                        If PreviousMode = GameMode.Pause Or PreviousMode = GameMode.Welcome Then
                            MainLoop()
                        End If
                    Case GameMode.Pause
                        RefreshGraphics()
                    Case GameMode.GameOver
                        RefreshGraphics()
                    Case GameMode.Highscore
                        Highscore.CreateScoreFile()
                        HighScores = Highscore.GetScores()
                        Dim fsc() As String = Highscore.PutPlayerInHighScore(HighScores)
                        HighScores = fsc
                        RefreshGraphics()
                End Select
            End Sub

            Public Shared Sub MainLoop()
                Do While Mode = GameMode.Playing
                    CheckEat()
                    RefreshGraphics()
                    CheckLoose()

                    Select Case Game.Difficulty
                        Case 1
                            Sleep(200)
                        Case 2
                            Sleep(140)
                        Case 3
                            Sleep(80)
                        Case 4
                            Sleep(70)
                        Case 5
                            Sleep(60)
                        Case 6
                            Sleep(50)
                    End Select

                    Application.DoEvents()

                    If Mode = GameMode.Playing Then
                        Player.ChangeDirection()
                        Player.Move()
                    End If
                Loop
            End Sub

            Public Shared Sub CheckEat()
                If Player.Tiles(0).PosX = Apple.PosX And Player.Tiles(0).PosY = Apple.PosY Then
                    Player.AddTile()
                    Apple = Food.PlaceFood()

                    Player.Score += Difficulty + 10
                End If
            End Sub

            Public Shared Sub CheckLoose()
                'check edges
                If Player.Tiles(0).PosX > GridLength - 1 _
                Or Player.Tiles(0).PosX < 0 _
                Or Player.Tiles(0).PosY > GridHeight - 1 _
                Or Player.Tiles(0).PosY < 0 Then
                    Mode = GameMode.GameOver
                End If

                'check eat itself
                For Each ti As Tile In Player.Tiles
                    If ti.PosX = Player.Tiles(0).PosX And ti.PosY = Player.Tiles(0).PosY And ti IsNot Player.Tiles(0) Then
                        Mode = GameMode.GameOver
                    End If
                Next
            End Sub

            Public Shared Sub CheckKeyPress(ByVal Key As KeyEventArgs)
                Select Case Key.KeyCode
                    Case Keys.Enter
                        If Mode = GameMode.Welcome Then
                            Mode = GameMode.Playing
                        ElseIf Mode = GameMode.Playing Then
                            Mode = GameMode.Pause
                        ElseIf Mode = GameMode.Pause Then
                            Mode = GameMode.Playing
                        ElseIf Mode = GameMode.GameOver Then
                            Mode = GameMode.Highscore
                        ElseIf Mode = GameMode.Highscore Then
                            Mode = GameMode.Welcome
                        End If



                    Case Keys.Escape
                        Game.Quit()



                    Case Keys.Up
                        If Mode = GameMode.Welcome Then
                            HigherDifficulty()
                            RefreshGraphics()
                        ElseIf Mode = GameMode.Playing Then
                            Player.ChangeDirectionRequest(Direction.Up)
                        End If
                    Case Keys.Right
                        If Mode = GameMode.Welcome Then
                            HigherDifficulty()
                            RefreshGraphics()
                        ElseIf Mode = GameMode.Playing Then
                            Player.ChangeDirectionRequest(Direction.Right)
                        End If
                    Case Keys.Down
                        If Mode = GameMode.Welcome Then
                            LowerDifficulty()
                            RefreshGraphics()
                        ElseIf Mode = GameMode.Playing Then
                            Player.ChangeDirectionRequest(Direction.Down)
                        End If
                    Case Keys.Left
                        If Mode = GameMode.Welcome Then
                            LowerDifficulty()
                            RefreshGraphics()
                        ElseIf Mode = GameMode.Playing Then
                            Player.ChangeDirectionRequest(Direction.Left)
                        End If
                End Select
            End Sub

            Public Shared Sub HigherDifficulty()
                If Difficulty < 6 Then
                    Difficulty += 1
                End If
            End Sub

            Public Shared Sub LowerDifficulty()
                If Difficulty > 1 Then
                    Difficulty -= 1
                End If
            End Sub

            Public Shared Sub RefreshGraphics()
                imgField.Refresh()
            End Sub

            Public Shared Sub SetBackgroundImage(ByVal Image As Image)
                imgField.Image = Image
            End Sub

            Public Shared Sub Quit()
                Mode = GameMode.Pause
                imgField.FindForm.Close()
            End Sub


            Public Class Highscore
                Public Shared Function PutPlayerInHighScore(ByVal scores() As String) As Array
                    Dim nSc(10) As String
                    Dim newScore As Boolean = False

                    For i As Integer = 0 To scores.Length - 1
                        Dim numSc As Integer = scores(i).Substring(scores(i).IndexOf("=") + 1)
                        If Player.Score > numSc Then
                            newScore = True
                            Player.Name = InputBox("Your name:", , Player.Name)

                            For j As Integer = 0 To 4
                                If j < i Then
                                    nSc(j) = scores(j)
                                ElseIf j = i Then
                                    nSc(j) = Player.Name & "=" & Player.Score
                                    nSc(j + 1) = scores(j)
                                ElseIf j > i Then
                                    nSc(j + 1) = scores(j)
                                End If
                            Next
                            Exit For
                        End If
                    Next

                    If newScore Then
                        Array.Resize(nSc, 5)

                        SaveScores(nSc)

                        Return nSc
                    Else
                        Return scores
                    End If
                End Function

                Public Shared Sub SaveScores(ByVal scores() As String)
                    Dim tW As TextWriter = New StreamWriter(My.Application.Info.DirectoryPath & "\Scores.fx")
                    Dim strSc As String = ""

                    For Each sc As String In scores
                        strSc &= sc & ";"
                    Next

                    tW.WriteLine(strSc)

                    tW.Close()
                End Sub

                Public Shared Function GetScores() As Array
                    Dim sc() As String

                    Dim tR As TextReader = New StreamReader(My.Application.Info.DirectoryPath & "\Scores.fx")
                    Dim strsc As String = tR.ReadLine
                    tR.Close()

                    sc = strsc.Split(";")

                    Array.Resize(sc, 5)

                    Return sc
                End Function

                Public Shared Sub CreateScoreFile()
                    If File.Exists(My.Application.Info.DirectoryPath & "\Scores.fx") = False Then
                        Dim tW As TextWriter = New StreamWriter(My.Application.Info.DirectoryPath & "\Scores.fx")
                        tW.WriteLine("FX=0;FX=0;FX=0;FX=0;FX=0")
                        tW.Close()
                    End If
                End Sub
            End Class

            Public Class Paint
                Private Shared g As Graphics
                Private Shared w As Long
                Private Shared h As Long
                Public Shared m As Decimal = 1.0

                Private Shared SmallFont As New Font("Verdana", 7 * m)
                Private Shared BigFont As New Font("Verdana", 8 * m, FontStyle.Bold)
                Private Shared CenterAlign As New StringFormat()

                Public Shared Sub Draw(ByVal graphics As Graphics, ByVal width As Long, ByVal height As Long)
                    CenterAlign.LineAlignment = StringAlignment.Center
                    CenterAlign.Alignment = StringAlignment.Center

                    g = graphics
                    w = width
                    h = height

                    Select Case Game.Mode
                        Case GameMode.Welcome
                            DrawWelcome()
                        Case GameMode.Playing
                            DrawPlaying()
                        Case GameMode.Pause
                            DrawPlaying()
                            DrawPause()
                        Case GameMode.GameOver
                            DrawPlaying()
                            DrawGameOver()
                        Case GameMode.Highscore
                            DrawHighscore()
                    End Select

                    g.DrawRectangle(Pens.White, New Rectangle(0, 0, w - 1, h - 1))
                End Sub

                Private Shared Sub DrawWelcome()
                    g.DrawString("SnakeFX Lite", BigFont, Brushes.White, w / 2, (h / 2) - 10, CenterAlign)

                    Dim stars As String = ""
                    For i As Integer = 0 To Difficulty - 1
                        stars &= Chr(149)
                    Next

                    g.DrawString("Difficulty: " & stars, SmallFont, Brushes.LightGray, w / 2, (h / 2) + 10, CenterAlign)
                End Sub

                Private Shared Sub DrawPlaying()
                    g.DrawString("Score: " & Game.Player.Score, SmallFont, Brushes.DarkGray, 5 * m, 5 * m)

                    g.FillRectangle(Brushes.DarkGoldenrod, New Rectangle(Game.Apple.PosX * TileWidth * m, _
                                                                         Game.Apple.PosY * TileHeight * m, _
                                                                         TileWidth * m, _
                                                                         TileHeight * m))

                    For Each ti As Tile In Game.Player.Tiles
                        g.FillRectangle(Brushes.GreenYellow, New Rectangle(ti.PosX * TileWidth * m, _
                                                                              ti.PosY * TileHeight * m, _
                                                                              TileWidth * m, _
                                                                              TileHeight * m))
                    Next
                End Sub

                Private Shared Sub DrawPause()
                    g.DrawString("Pause", BigFont, Brushes.White, w / 2, h / 2, CenterAlign)
                End Sub

                Private Shared Sub DrawGameOver()
                    g.DrawString("Game Over", BigFont, Brushes.White, w / 2, h / 2, CenterAlign)

                    g.DrawString("Score: " & Game.Player.Score, SmallFont, Brushes.DarkGray, 5 * m, 5 * m)
                End Sub

                Private Shared Sub DrawHighscore()
                    g.DrawString("High Score", BigFont, Brushes.White, w / 2, 25, CenterAlign)

                    For i As Integer = 0 To Game.HighScores.Length - 1
                        g.DrawString(Game.HighScores(i).Replace("=", ": "), SmallFont, Brushes.LightGray, w / 2, (10 * i + 40) * m, CenterAlign)
                    Next
                End Sub
            End Class


            Public Enum GameMode
                Welcome = 1
                Playing = 2
                Pause = 3
                GameOver = 4
                Highscore = 5
            End Enum

            Private Shared Sub imgField_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles imgField.Paint
                Game.Paint.Draw(e.Graphics, imgField.Width, imgField.Height)
            End Sub
        End Class

        Public Class Snake
            Public Tiles(-1) As Tile
            Public Score As Long = 0
            Public Name As String = ""

            Public Sub CreateSnake(ByVal Length As Long, ByVal PosX As Integer, ByVal PosY As Integer, ByVal Direction As Direction)
                Dim headTile As New Tile(PosX, PosY, Direction)

                Array.Resize(Tiles, Tiles.Length + 1)
                Tiles(Tiles.Length - 1) = headTile

                For i As Integer = 0 To Length - 2
                    AddTile()
                Next
            End Sub

            Public Sub DestroySnake()
                Tiles = Nothing
            End Sub

            Public Sub AddTile()
                Dim nTi As New Tile

                nTi.Direction = Tiles(Tiles.Length - 1).Direction

                Select Case Tiles(Tiles.Length - 1).Direction
                    Case Direction.Up
                        nTi.PosX = Tiles(Tiles.Length - 1).PosX
                        nTi.PosY = Tiles(Tiles.Length - 1).PosY + 1
                    Case Direction.Right
                        nTi.PosX = Tiles(Tiles.Length - 1).PosX - 1
                        nTi.PosY = Tiles(Tiles.Length - 1).PosY
                    Case Direction.Down
                        nTi.PosX = Tiles(Tiles.Length - 1).PosX
                        nTi.PosY = Tiles(Tiles.Length - 1).PosY - 1
                    Case Direction.Left
                        nTi.PosX = Tiles(Tiles.Length - 1).PosX + 1
                        nTi.PosY = Tiles(Tiles.Length - 1).PosY
                End Select

                Array.Resize(Tiles, Tiles.Length + 1)

                Tiles(Tiles.Length - 1) = nTi
            End Sub

            Private DirectionRequest As Direction
            Public Sub ChangeDirectionRequest(ByVal Direction As Direction)
                DirectionRequest = Direction
            End Sub

            Public Sub ChangeDirection()
                Dim direction As Direction = DirectionRequest

                Dim ok As Boolean = True

                Select Case Tiles(0).Direction
                    Case Easter.Snake.Direction.Up
                        If direction = Easter.Snake.Direction.Down Then
                            ok = False
                        End If
                    Case Easter.Snake.Direction.Right
                        If direction = Easter.Snake.Direction.Left Then
                            ok = False
                        End If
                    Case Easter.Snake.Direction.Down
                        If direction = Easter.Snake.Direction.Up Then
                            ok = False
                        End If
                    Case Easter.Snake.Direction.Left
                        If direction = Easter.Snake.Direction.Right Then
                            ok = False
                        End If
                End Select

                If ok Then
                    If direction <> 0 Then
                        Me.Tiles(0).Direction = direction
                    End If
                End If

                DirectionRequest = 0
            End Sub

            Public Sub Move()
                For i As Integer = Tiles.Length - 1 To 1 Step -1
                    Tiles(i).PosX = Tiles(i - 1).PosX
                    Tiles(i).PosY = Tiles(i - 1).PosY
                    Tiles(i).Direction = Tiles(i - 1).Direction
                Next

                Select Case Tiles(0).Direction
                    Case Direction.Up
                        Tiles(0).PosY -= 1
                    Case Direction.Right
                        Tiles(0).PosX += 1
                    Case Direction.Down
                        Tiles(0).PosY += 1
                    Case Direction.Left
                        Tiles(0).PosX -= 1
                End Select
            End Sub
        End Class

        Public Enum Direction
            Up = 1
            Right = 2
            Down = 3
            Left = 4
        End Enum

        Public Class Tile
            Public Sub New(Optional ByVal posx As Integer = 0, Optional ByVal posy As Integer = 0, Optional ByVal direction As Direction = Easter.Snake.Direction.Up)
                _PosX = posx
                _PosY = posy
                _Direction = direction
            End Sub

            Private _PosX As Integer
            Public Property PosX() As Integer
                Get
                    Return _PosX
                End Get
                Set(ByVal value As Integer)
                    _PosX = value
                End Set
            End Property

            Private _PosY As Integer
            Public Property PosY() As Integer
                Get
                    Return _PosY
                End Get
                Set(ByVal value As Integer)
                    _PosY = value
                End Set
            End Property

            Private _Direction As Direction
            Public Property Direction() As Direction
                Get
                    Return _Direction
                End Get
                Set(ByVal value As Direction)
                    _Direction = value
                End Set
            End Property
        End Class

        Public Class Food
            Public Sub New(ByVal posx As Integer, ByVal posy As Integer)
                _PosX = posx
                _PosY = posy
            End Sub

            Private _PosX As Integer
            Public Property PosX() As Integer
                Get
                    Return _PosX
                End Get
                Set(ByVal value As Integer)
                    _PosX = value
                End Set
            End Property

            Private _PosY As Integer
            Public Property PosY() As Integer
                Get
                    Return _PosY
                End Get
                Set(ByVal value As Integer)
                    _PosY = value
                End Set
            End Property


            Public Shared Function PlaceFood() As Food
                Dim ok As Boolean = False

                Dim PosX As Integer
                Dim PosY As Integer

                Do Until ok
                    PosX = Tools.RandomNumber(Game.GridLength, 0)
                    PosY = Tools.RandomNumber(Game.GridHeight, 0, Now.Millisecond + Now.Second)

                    Dim problem As Boolean = False

                    For Each ti As Tile In Game.Player.Tiles
                        If ti.PosX = PosX And ti.PosY = PosY Then
                            problem = True
                        End If
                    Next

                    If problem = False Then
                        ok = True
                    End If
                Loop

                Return New Food(PosX, PosY)
            End Function
        End Class

        Public Class Tools
            Public Shared Function RandomNumber(ByVal MaxNumber As Integer, Optional ByVal MinNumber As Integer = 0, Optional ByVal Seed As Long = 0) As Integer
                Dim r As New Random

                If Seed <> 0 Then
                    r = New Random(Seed)
                End If

                If MinNumber > MaxNumber Then
                    Dim t As Integer = MinNumber
                    MinNumber = MaxNumber
                    MaxNumber = t
                End If

                Return r.Next(MinNumber, MaxNumber)
            End Function
        End Class
    End Class
End Namespace

