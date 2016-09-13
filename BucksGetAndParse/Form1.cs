using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BucksGetAndParse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private BindingContext bindingContext;
        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        public BindingContext Bindingcontext
        {
            get
            {
                return bindingContext;
            }
            set
            {
                bindingContext = value;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CBRService cbr = new CBRService(textBox1.Text, label1);
            bindingSource1.DataSource = cbr;
            cbr.AsyncGetCurrencyRateOnDate(DateTime.Now, textBox1.Text);
            
        }
    }
}
