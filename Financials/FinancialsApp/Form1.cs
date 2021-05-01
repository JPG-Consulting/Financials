using FinancialsApp.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancialsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dividendsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoricalDividendDialog dialog = new HistoricalDividendDialog();

            dialog.ShowDialog(this);
        }

        private void pricesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistorialPricesDialog dialog = new HistorialPricesDialog();

            dialog.ShowDialog(this);
        }
    }
}
