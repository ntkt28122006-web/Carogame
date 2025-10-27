using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace caro_game
{
    public class ChessManager
    {
        #region properties

        private Panel chessBoard;

        public Panel ChessBoard
        {
            get { return chessBoard; }
            set { chessBoard = value; }
        }

        private List<player> player;

        public List<player> Player
        {
            get { return player; }
            set { player = value; }
        }

        private int currentPlayer;
        public int CurrentPlayer
        {
            get { return currentPlayer; }
            set { currentPlayer = value; }
        }

        private TextBox playerName;
        public TextBox PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }

        private PictureBox playerMark;
        public PictureBox PlayerMark
        {
            get { return playerMark; }
            set { playerMark = value; }
        }

        private List<List<Button>> matrix;
        public List<List<Button>> Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }

        private event EventHandler playerMarked;
        public event EventHandler PlayerMarked
        {
            add
            {
                playerMarked += value;

            }

            remove
            {
                playerMarked -= value;
            }
        }

        private event EventHandler endedGame;
        public event EventHandler EndedGame
        {
            add
            {
                endedGame += value;
            }
            remove
            {
                endedGame -= value;
            }
        }

        private Stack<PlayInfo> playTimeLine;
        public Stack<PlayInfo> PlayTimeLine
        {
            get { return playTimeLine; }
            set { playTimeLine = value; }
        }


        #endregion

        #region Initialize
        public ChessManager(Panel chessBoard, TextBox playerName, PictureBox mark,string player1Name, string player2Name)
        {
            this.ChessBoard = chessBoard;
            this.PlayerName = playerName;
            this.PlayerMark = mark;
            this.Player = new List<player>()
            {
                new player(player1Name, Image.FromFile(Application.StartupPath + "\\Resources\\Ảnh X.jpg")),
                new player (player2Name, Image.FromFile(Application.StartupPath + "\\Resources\\Ảnh O.jpg"))
            };

           


        }


        #endregion

        #region Methods
        public void DrawChessBoard()
        {
            ChessBoard.Enabled = true;
            chessBoard.Controls.Clear();
            playTimeLine = new Stack<PlayInfo>();

            CurrentPlayer = 0;

            changePlayer();

            matrix = new List<List<Button>>();
            Button oldButton = new Button() { Width = 0, Location = new Point(0, 0) };
            for (int i = 0; i < cons.CHESS_BOARD_HEIGHT; i++)
            {
                Matrix.Add(new List<Button>());
                for (int j = 0; j < cons.CHESS_BOARD_WIDTH; j++)
                {
                    Button btn = new Button()
                    {
                        Width = cons.CHESS_WIDTH,
                        Height = cons.CHESS_HEIGHT,
                        Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString() //
                    };

                    btn.Click += Btn_Click;
                    ChessBoard.Controls.Add(btn);

                    Matrix[i].Add(btn);

                    oldButton = btn;
                }

                oldButton.Location = new Point(0, oldButton.Location.Y + cons.CHESS_HEIGHT);
                oldButton.Width = 0;
                oldButton.Height = 0;
            }

        }
        private void Btn_Click(object sender, EventArgs e)
        {

            Button btn = sender as Button;

            if (btn.BackgroundImage != null)
            {
                return; // khong thay doi hinh anh da ton tai
            }

            Mark(btn);

            playTimeLine.Push(new PlayInfo(GetChessPoint(btn),CurrentPlayer));

            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1; // thay doi gia tri

            changePlayer();

            if (playerMarked != null) // có người đánh chưa
            {
                playerMarked(this, new EventArgs());
            }

            if (isEndGame(btn))
            {
                EndGame();
            }

        }

        public void EndGame()
        {
            if (endedGame != null)
            {
                endedGame(this, new EventArgs());
            }
        }

        public bool Undo()
        {
            
            PlayInfo oldPoint=PlayTimeLine.Pop();
            Button btn = Matrix[oldPoint.Point.Y][oldPoint.Point.X];

            btn.BackgroundImage = null;

            if (PlayTimeLine.Count <= 0)
            {
                currentPlayer = 0;
            }
            else
            {
                CurrentPlayer = PlayTimeLine.Peek().CurrentPlayer == 1 ? 0 : 1;
            }

           
            changePlayer();
            return true;
        }
        private bool isEndGame(Button btn)
        {
            return isHangngang(btn) || isHangdoc(btn) || isCheochinh(btn) || isCheophu(btn);
        }

        private Point GetChessPoint(Button btn)
        {


            int hangdoc = Convert.ToInt32(btn.Tag); //   hang doc (Y)
            int hangngang = Matrix[hangdoc].IndexOf(btn); // hangngang (X)

            Point point = new Point(hangngang, hangdoc);  // X la hang ngang, Y la hang doc

            return point;
        }
        private bool isHangngang(Button btn)
        {
            Point point = GetChessPoint(btn);

            int CountLeft = 0; // tạo biến đếm bên trái
            for (int i = point.X; i >= 0; i--) //i la Point.x
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    CountLeft++;
                }
                else
                {
                    break;
                }
            }
            int CountRight = 0; // tạo biến đếm bên phải
            for (int i = point.X + 1; i < cons.CHESS_BOARD_WIDTH; i++)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    CountRight++;
                }
                else
                {
                    break;
                }
            }

            return (CountLeft + CountRight) >= 5;


        }
        private bool isHangdoc(Button btn)
        {
            Point point = GetChessPoint(btn);

            int CountTop = 0; 
            for (int i = point.Y; i >= 0; i--)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    CountTop++;
                }
                else
                {
                    break;
                }
            }
            // Luon Luon Y:X
            int CountBot = 0;
            for (int i = point.Y + 1; i < cons.CHESS_BOARD_HEIGHT; i++)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    CountBot++;
                }
                else
                {
                    break;
                }
            }

            return (CountTop + CountBot) >= 5;
        }
        private bool isCheochinh(Button btn)
        {
            Point point = GetChessPoint(btn);

            int CountTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0)
                {
                    break;
                }
                if (Matrix[point.Y - i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    CountTop++;
                }
                else
                {
                    break;
                }
            }

            int CountBot = 0;
            for (int i = 1; i < cons.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.Y + i >= cons.CHESS_BOARD_WIDTH || point.X + i >= cons.CHESS_BOARD_HEIGHT)
                {
                    break;
                }
                if (Matrix[point.Y + i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    CountBot++;
                }
                else
                {
                    break;
                }
            }

            return (CountTop + CountBot) >= 5;
        }

        private bool isCheophu(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X + i > cons.CHESS_BOARD_WIDTH || point.Y - i < 0)
                {
                    break;
                }

                if (Matrix[point.Y - i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else break;
            }

            int countBot = 0;
            for (int i = 1; i <= cons.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.Y + i >= cons.CHESS_BOARD_HEIGHT || point.X - i < 0) { break; }
                if (Matrix[point.Y + i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countBot++;
                }
                else
                {
                    break;
                }
            }
            return (countTop + countBot >= 5);
        }


        // Cho biet nguoi di truoc
        private void Mark(Button btn)
        {
            btn.BackgroundImage = Player[CurrentPlayer].Mark;
        }

        private void changePlayer() // doi ten nguoi choi vs hinh anh
        {
            PlayerName.Text = Player[CurrentPlayer].Name;

            PlayerMark.Image = Player[CurrentPlayer].Mark;
        }
        #endregion


    }
}
