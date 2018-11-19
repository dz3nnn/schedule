using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Models;
using MainLibrary;

namespace Schedule_WF
{
    public partial class AddBookItem : Form
    {
        private string BookType;
        BookController bc = new BookController();
        public AddBookItem()
        {
            InitializeComponent();
        }

        public AddBookItem(string btype)
        {
            InitializeComponent();
            this.BookType = btype;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bc.AddBookItem(new BookItem {Name = textBox1.Text },this.BookType);
            this.Close();
        }

        private void AddBookItem_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(sender, e);
        }
    }
}
