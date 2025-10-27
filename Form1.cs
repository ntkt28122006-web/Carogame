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



namespace caro_game
{
    

    public partial class Form1 : Form
    {


        #region Properties
        ChessManager ChessBoard;
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
    } // test dong bo 123 123 123 89932523
}
