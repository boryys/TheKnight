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
        List<PictureBox> PictureBoxList;
        Random rnd = new Random();
        public int BoardSize { get; set; }
        Point Pos;
        bool direction = true;

        bool moved = true;
        bool moveu = true;
        bool movel = true;
        bool mover = true;

        public Form1()
        {
            InitializeComponent();
            BoardSize = 8;
            NewGame(BoardSize);
        }

        public void NewGame(int size)
        {
            PictureBoxList = new List<PictureBox>();

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

                    PictureBoxList.Add(PictureBox);
                    Board.Controls.Add(PictureBox);
                }
            }

            SetKnight();
        }

        private void NextGame(int size)
        {
            int clr;

            for (int i = 0; i < PictureBoxList.Count; i++)
            {
                clr = rnd.Next(0, 2);
                if (clr == 0) PictureBoxList.ElementAt(i).BackColor = Color.Maroon;
                else PictureBoxList.ElementAt(i).BackColor = Color.ForestGreen;
                PictureBoxList.ElementAt(i).Image = null;
            }
            SetKnight();
        }

        private void SetKnight()
        {
            int i = 0;
            while (PictureBoxList.ElementAt(i).BackColor == Color.Maroon) i++;

            PictureBox prc = PictureBoxList.ElementAt(i);
            Pos = (Point)prc.Tag;

            DrawKnight(prc);
        }

        private void DrawKnight(PictureBox box)
        {
            box.Image = null;

            Bitmap src;
            if (direction) src = Properties.Resources.knight;
            else src = Properties.Resources.knight2;

            src.MakeTransparent();
            box.Image = src;
            box.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            PictureBox current = (PictureBox)Board.GetControlFromPosition(Pos.Y, Pos.X);

            if (e.KeyCode == Keys.Down)
            {
                if (moved == true)
                {
                    if (Pos.X + 1 < BoardSize)
                    {
                        PictureBox NextStep = (PictureBox)Board.GetControlFromPosition(Pos.Y, Pos.X + 1);
                        if (NextStep.BackColor == Color.ForestGreen)
                        {
                            moved = false;
                            current.Image = null;
                            DrawKnight(NextStep);
                            Pos.X++;
                        }
                    }
                }
            }

            if (e.KeyCode == Keys.Up)
            {
                if (moveu == true)
                {
                    if (Pos.X - 1 >= 0)
                    {
                        PictureBox NextStep = (PictureBox)Board.GetControlFromPosition(Pos.Y, Pos.X - 1);
                        if (NextStep.BackColor == Color.ForestGreen)
                        {
                            moveu = false;
                            current.Image = null;
                            DrawKnight(NextStep);
                            Pos.X--;
                        }
                    }
                }
            }

            if (e.KeyCode == Keys.Right)
            {
                if (mover == true)
                {
                    direction = true;
                    if (Pos.Y + 1 < BoardSize)
                    { 
                        PictureBox NextStep = (PictureBox)Board.GetControlFromPosition(Pos.Y + 1, Pos.X);
                        if (NextStep.BackColor == Color.ForestGreen)
                        {
                            mover = false;
                            current.Image = null;
                            DrawKnight(NextStep);
                            Pos.Y++;
                        }
                        else DrawKnight(current);
                    }
                    else DrawKnight(current);
                }
            }

            if (e.KeyCode == Keys.Left)
            {
                if (movel == true)
                {
                    direction = false;
                    if (Pos.Y - 1 >= 0)
                    {
                        direction = false;
                        PictureBox NextStep = (PictureBox)Board.GetControlFromPosition(Pos.Y - 1, Pos.X);
                        if (NextStep.BackColor == Color.ForestGreen)
                        {
                            movel = false;
                            current.Image = null;
                            DrawKnight(NextStep);
                            Pos.Y--;
                        }
                        else DrawKnight(current);
                    }
                    else DrawKnight(current);
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down) moved = true;
            if (e.KeyCode == Keys.Up) moveu = true;
            if (e.KeyCode == Keys.Left) movel = true;
            if (e.KeyCode == Keys.Right) mover = true;
        }



        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NextGame(BoardSize);
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
                NextGame(BoardSize);
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
