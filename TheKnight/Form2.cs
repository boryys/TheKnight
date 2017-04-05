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
    public partial class Form2 : Form
    {
        Form1 mainform;

        public Form2(Form1 _mainform)
        {
            mainform = _mainform;

            InitializeComponent();
            this.ShowInTaskbar = false;
            comboBox.Text = "8 x 8";
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if(comboBox.Text == "8 x 8") mainform.BoardSize = 8;
            else
            {
                if(comboBox.Text == "10 x 10") mainform.BoardSize = 10;
                else mainform.BoardSize = 12;
            }

            mainform.NewGame(mainform.BoardSize);
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
