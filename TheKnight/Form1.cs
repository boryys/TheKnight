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
        Form3 splash;
        Random rnd = new Random();
        public int BoardSize { get; set; }
        Point Pos;
        Point PosKey;
        Point PosDoor;
        bool direction;
        bool dooropen;

        bool EditMode = false;
        bool EditWall = true;
        PictureBox helper;

        bool moved = true;
        bool moveu = true;
        bool movel = true;
        bool mover = true;

        public Form1(Form3 _splash)
        {
            splash = _splash;
            InitializeComponent();
            BoardSize = 8;
            NewGame(BoardSize);
        }

        public void NewGame(int size)
        {
            PictureBoxList = new List<PictureBox>();
            direction = true;
            dooropen = false;

            Board.Controls.Clear();
            Board.ColumnStyles.Clear();
            Board.RowStyles.Clear();

            Board.ColumnCount = size;
            Board.RowCount = size;

            for (int row = 0; row < size; row++)
            {
                Board.RowStyles.Add(new RowStyle(SizeType.Percent, (float)100.0 / size));
                Board.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)100.0 / size));

                for (int column = 0; column < size; column++)
                {
                    PictureBox PictureBox = new PictureBox();
                    PictureBox.Tag = new Point(column, row);
                    PictureBox.Dock = DockStyle.Fill;
                    PictureBox.Margin = new Padding(0, 0, 0, 0);
                    PictureBox.MouseDown += new MouseEventHandler(PictureBox_MouseDown);

                    ColorCells(PictureBox, column, row);

                    PictureBoxList.Add(PictureBox);
                    Board.Controls.Add(PictureBox);
                }
            }

            SetKnight();
            SetKeyandDoor(true);
            SetKeyandDoor(false);
        }

        private void ColorCells(PictureBox box, int column, int row)
        {
            int clr;
            bool wall = false;

            if(column-1 >= 0)
            {
                if (Board.GetControlFromPosition(column-1, row).BackColor == Color.Maroon) wall = true;
                else
                {
                    if(row-1 >= 0)
                    {
                        if (Board.GetControlFromPosition(column, row-1).BackColor == Color.Maroon) wall = true;
                    }
                }
            }

            if(wall) clr = rnd.Next(0, 2);
            else clr = rnd.Next(0, 5);

            if (clr == 0) box.BackColor = Color.Maroon;
            else box.BackColor = Color.ForestGreen;
        }

        private void NextGame(int size)
        {
            PictureBox box;
            direction = true;
            dooropen = false;

            for (int row = 0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    box = (PictureBox)Board.GetControlFromPosition(column, row);
                    box.Image = null;
                    ColorCells(box, column, row);
                }
            }
            SetKnight();
            SetKeyandDoor(true);
            SetKeyandDoor(false);
        }

        private void SetKnight()
        {
            int i = 0;
            while (PictureBoxList.ElementAt(i).BackColor == Color.Maroon) i++;

            PictureBox prc = PictureBoxList.ElementAt(i);
            Pos = (Point)prc.Tag;

            DrawKnight(prc);
        }

        private void SetKeyandDoor(bool key)
        {
            int random = 0;
            bool grass = false;
            Bitmap src;

            while (!grass)
            {
                random = rnd.Next(0, PictureBoxList.Count);
                if (PictureBoxList.ElementAt(random).Image == null)
                {
                    if (PictureBoxList.ElementAt(random).BackColor == Color.ForestGreen) grass = true;
                }
            }

            if (key)
            {
                PosKey = (Point)PictureBoxList.ElementAt(random).Tag;
                src = Properties.Resources.key2;
            }
            else
            {
                PosDoor = (Point)PictureBoxList.ElementAt(random).Tag;
                src = Properties.Resources.closed_door;
            }

            src.MakeTransparent();
            PictureBoxList.ElementAt(random).Image = src;
            PictureBoxList.ElementAt(random).SizeMode = PictureBoxSizeMode.StretchImage;
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

        private void MoveKnight(PictureBox current, int nextX, int nextY)
        {
            Point next = new Point(nextX, nextY);
            PictureBox NextStep = (PictureBox)Board.GetControlFromPosition(nextX, nextY);
            if (NextStep.BackColor == Color.ForestGreen && (next != PosDoor || dooropen))
            {
                current.Image = null;
                DrawKnight(NextStep);
                Pos.Y = nextY;
                Pos.X = nextX;
            }
            else DrawKnight(current);

            if(Pos == PosKey)
            {
                dooropen = true;
                PictureBox Door = (PictureBox)Board.GetControlFromPosition(PosDoor.X, PosDoor.Y);
                Bitmap src = Properties.Resources.opened_door;
                src.MakeTransparent();
                Door.Image = src;
                Door.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            if (Pos == PosDoor)
            {
                NextGame(BoardSize);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!EditMode)
            {
                PictureBox current = (PictureBox)Board.GetControlFromPosition(Pos.X, Pos.Y);

                if (e.KeyCode == Keys.Down)
                {
                    if (moved == true)
                    {
                        if (Pos.Y + 1 < BoardSize)
                        {
                            MoveKnight(current, Pos.X, Pos.Y + 1);
                            moved = false;
                        }
                    }
                }

                if (e.KeyCode == Keys.Up)
                {
                    if (moveu == true)
                    {
                        if (Pos.Y - 1 >= 0)
                        {
                            MoveKnight(current, Pos.X, Pos.Y - 1);
                            moveu = false;
                        }
                    }
                }

                if (e.KeyCode == Keys.Right)
                {
                    if (mover == true)
                    {
                        direction = true;
                        if (Pos.X + 1 < BoardSize)
                        {
                            MoveKnight(current, Pos.X + 1, Pos.Y);
                            mover = false;
                        }
                        else DrawKnight(current);
                    }
                }

                if (e.KeyCode == Keys.Left)
                {
                    if (movel == true)
                    {
                        direction = false;
                        if (Pos.X - 1 >= 0)
                        {
                            MoveKnight(current, Pos.X - 1, Pos.Y);
                            movel = false;
                        }
                        else DrawKnight(current);
                    }
                }

                if (e.KeyCode == Keys.Space)
                {
                    try
                    {
                        Board.GetControlFromPosition(Pos.X - 1, Pos.Y).BackColor = Color.ForestGreen;
                    }
                    catch { }
                    try
                    {
                        Board.GetControlFromPosition(Pos.X + 1, Pos.Y).BackColor = Color.ForestGreen;
                    }
                    catch { }
                    try
                    {
                        Board.GetControlFromPosition(Pos.X, Pos.Y - 1).BackColor = Color.ForestGreen;
                    }
                    catch { }
                    try
                    {
                        Board.GetControlFromPosition(Pos.X, Pos.Y + 1).BackColor = Color.ForestGreen;
                    }
                    catch { }
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
                splash.Dispose();
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

        private void editModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EditMode)
            {
                EditMode = true;
                menuStrip.BackColor = Color.Gold;
                leftClickToolStripMenuItem.Visible = true;
                editModeToolStripMenuItem.Text = "Game mode";
            }
            else
            {
                EditMode = false;
                menuStrip.BackColor = SystemColors.Control;
                leftClickToolStripMenuItem.Visible = false;
                editModeToolStripMenuItem.Text = "Edit mode";
            }
        }

        private void grassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grassToolStripMenuItem.Checked = true;
            wallToolStripMenuItem.Checked = false;

            EditWall = false;
        }

        private void wallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grassToolStripMenuItem.Checked = false;
            wallToolStripMenuItem.Checked = true;

            EditWall = true;
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (EditMode)
            {
                helper = (PictureBox)sender;

                if (e.Button == MouseButtons.Left)
                {
                    if (EditWall)
                    {
                        if (helper.Image == null) helper.BackColor = Color.Maroon;
                    }
                    else
                    {
                        if (helper.Image == null) helper.BackColor = Color.ForestGreen;
                    }
                }

                if (e.Button == MouseButtons.Right)
                {
                    context.Show(Cursor.Position);
                }
            }
        }

        private void knightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (helper.Image == null)
            {
                PictureBox current = (PictureBox)Board.GetControlFromPosition(Pos.X, Pos.Y);
                current.Image = null;
                helper.BackColor = Color.ForestGreen;
                DrawKnight(helper);
                Pos = (Point)helper.Tag;
            }
        }

        private void keyToolStripMenuItem_Click(object sender, EventArgs e)
        {
             if (helper.Image == null)
             {
                    PictureBox current = (PictureBox)Board.GetControlFromPosition(PosKey.X, PosKey.Y);
                    current.Image = null;
                    helper.BackColor = Color.ForestGreen;

                    Bitmap src;
                    src = Properties.Resources.key2;
                    src.MakeTransparent();
                    helper.Image = src;
                    helper.SizeMode = PictureBoxSizeMode.StretchImage;

                if (Pos == PosKey)
                {
                    DrawKnight((PictureBox)Board.GetControlFromPosition(Pos.X, Pos.Y));
                }

                PosKey = (Point)helper.Tag;
            }
        }

        private void doorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (helper.Image == null)
            {
                PictureBox current = (PictureBox)Board.GetControlFromPosition(PosDoor.X, PosDoor.Y);
                current.Image = null;
                helper.BackColor = Color.ForestGreen;

                Bitmap src = Properties.Resources.closed_door;

                src.MakeTransparent();
                helper.Image = src;
                helper.SizeMode = PictureBoxSizeMode.StretchImage;
                PosDoor = (Point)helper.Tag;
                dooropen = false;
            }
        }
    }
}
