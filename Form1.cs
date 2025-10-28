using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Net.Sockets;
using System.Threading;
using GameCaro;
using System.Net.NetworkInformation;



namespace caro_game
{
    

    public partial class Form1 : Form
    {


        #region Properties
        ChessManager ChessBoard;
        SocketManager socket;
        private string player1Name;
        private string player2Name;

        #endregion
        public Form1(string p1, string p2)
        {
            InitializeComponent();

            player1Name = p1;
            player2Name = p2;


            ChessBoard = new ChessManager(pnlChessBoard, txbPlayerName, pictureBox2,player1Name,player2Name);
            ChessBoard.PlayerMarked += ChessBoard_PlayerMarked;
            ChessBoard.EndedGame += ChessBoard_EndedGame;


            progressBar1.Step = cons.COOL_DOWN_STEP;
            progressBar1.Maximum = cons.COOL_DOWN_TIME;
            progressBar1.Value = 0;

            tmCoolDown.Interval = cons.COOL_DOWN_INTERVAL;



            ChessBoard.DrawChessBoard();

        }
        public Form1()
        {
            InitializeComponent();
        }
        void EndGame()
        {
            tmCoolDown.Stop();
            pnlChessBoard.Enabled = false;
            undoToolStripMenuItem.Enabled = false; // kết thúc game thì ko undo dc
            MessageBox.Show("EndGame!");
        }


        private void ChessBoard_EndedGame(object sender, EventArgs e)
        {
            EndGame();
        }

        private void ChessBoard_PlayerMarked(object sender, EventArgs e)
        {
            tmCoolDown.Start();
            progressBar1.Value = 0;
        }

        private void tmCoolDown_Tick(object sender, EventArgs e)
        {
            progressBar1.PerformStep();

            if(progressBar1.Value >= progressBar1.Maximum){
                
                EndGame();
                
            }

        }



        void NewGame()
        {
            progressBar1.Value = 0;
            tmCoolDown.Stop();
            undoToolStripMenuItem.Enabled=true;
            ChessBoard.DrawChessBoard();
            
        }

        void Undo()
        {
            ChessBoard.Undo();
        }
        void Quit()
        {
                Application.Exit();
         
        }
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quit();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc là muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void txbPlayerName_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnlChessBoard_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = GameHistory.LoadResults();
            if (result.Count == 0)
            {
                MessageBox.Show("Chưa có game nào được lưu!");
                return;
            }
            string text = string.Join(Environment.NewLine, result.Select(r => r.ToString()));
            MessageBox.Show(text, "Lịch sử đấu");
        }

        private void btnLAN_Click(object sender, EventArgs e)
        {
            socket.IP = txbIP.Text;

            if (!socket.ConnectServer())
            {
                socket.CreateServer();

                Thread listenThread = new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(500);
                        try
                        {
                            Listen();
                        }
                        catch { }
                        
                    }
                });
                listenThread.IsBackground = true;
                listenThread.Start();

            }
            else
            {
                Thread listenThread = new Thread(() =>
                {
                    Listen();
                });
                listenThread.IsBackground = true;
                listenThread.Start();
                socket.Send("Thông tin từ client");
            }

            
        }

        private void Listen()
        {
            throw new NotImplementedException();
        }

    

        private void Form1_Shown(object sender, EventArgs e)
        {
            txbIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            if (string.IsNullOrEmpty(txbIP.Text))
            {
                txbIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet);

            }

            void Listen()
            {
                string data = (string)socket.Receive();

                MessageBox.Show(data);
            }
        }
    } 
}
