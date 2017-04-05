using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheKnight
{
    public partial class Form1 : Form
    {
        Random rnd = new Random();
        public int BoardSize { get; set; }

        public Form1()
        {
            InitializeComponent();
            BoardSize = 8;
            NewGame(BoardSize);
        }

        public void NewGame(int size)
        {
            int clr;

            Board.Controls.Clear();
            Board.ColumnStyles.Clear();
            Board.RowStyles.Clear();

            Board.ColumnCount = size;
            Board.RowCount = size;

            for (int i = 0; i < size; i++)
            {
                Board.RowStyles.Add(new RowStyle(SizeType.Percent, (float)100.0 / size));
                Board.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)100.0 / size));

                for (int j = 0; j < size; j++)
                {
                    PictureBox PictureBox = new PictureBox();
                    PictureBox.Tag = new Point(i, j);
                    PictureBox.Dock = DockStyle.Fill;
                    PictureBox.Margin = new Padding(0,0,0,0);

                    clr = rnd.Next(0, 2);
                    if (clr == 0) PictureBox.BackColor = Color.Maroon;
                    else PictureBox.BackColor = Color.ForestGreen;

                    Board.Controls.Add(PictureBox);
                }
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame(BoardSize);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2(this);
            form.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (PreClosingConfirmation() == System.Windows.Forms.DialogResult.Yes)
            {
                Dispose();
            }
            else
            {
                e.Cancel = true;
            }
        }

        public DialogResult PreClosingConfirmation()
        {
            DialogResult res = System.Windows.Forms.MessageBox.Show("Do you want to quit?", "Closing", MessageBoxButtons.YesNo);
            return res;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.N))
            {
                NewGame(BoardSize);
                return true;
            }
            if (keyData == (Keys.Control | Keys.M))
            {
                Form2 form = new Form2(this);
                form.ShowDialog();

                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
