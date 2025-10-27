using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace caro_game
{
    public partial class FormLogin : Form
    {

        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string player1 = txtPlayer1Name.Text.Trim();
            string player2 = txtPlayer2Name.Text.Trim();

            if (string.IsNullOrEmpty(txtPlayer1Name.Text) || (string.IsNullOrEmpty(txtPlayer2Name.Text)))
            {
                MessageBox.Show("Vui lòng nhập tên người chơi", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Form1 frm = new Form1(player1, player2);
            this.Hide();
            frm.ShowDialog();
            this.Show();

        }
    }
}
