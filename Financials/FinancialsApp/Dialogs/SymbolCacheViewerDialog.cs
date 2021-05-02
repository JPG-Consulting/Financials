using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace FinancialsApp.Dialogs
{
    public partial class SymbolCacheViewerDialog : Form
    {
        public SymbolCacheViewerDialog()
        {
            InitializeComponent();
            LoadSymbols();
        }

        private void LoadSymbols()
        {
            this.comboBox1.BeginUpdate();
            this.comboBox1.Items.Clear();

            try
            {
                string cachePath = Path.GetFullPath("cache");
                string[] files = Directory.GetFiles(cachePath, "*_Historical.sqlite3");
                Array.Sort(files);

                foreach (string filename in files)
                {
                    string symbol = Path.GetFileName(filename);
                    symbol = symbol.Substring(0, symbol.Length - 19);
                    this.comboBox1.Items.Add(symbol);
                }
            }
            catch (Exception ex)
            {
                this.comboBox1.Items.Clear();

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.comboBox1.EndUpdate();
            }
        }

        private void LoadGraphs()
        {
            this.formsPlot1.plt.Clear();
            this.formsPlot2.plt.Clear();

            if (this.comboBox1.SelectedIndex < 0)
                return;

            object o = this.comboBox1.Items[this.comboBox1.SelectedIndex];
            if (o == null)
                return;

            string dbPath = Path.GetFullPath(String.Format(@"cache\{0}_Historical.sqlite3", o.ToString()));

            using (SQLiteConnection connection = new SQLiteConnection(String.Format("Data Source={0};Version=3;", dbPath)))
            {
                connection.Open();

                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT date, close FROM [Prices] ORDER BY date ASC";

                    List<double> xs = new List<double>();
                    List<double> ys = new List<double>();

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xs.Add(reader.GetDateTime(0).ToOADate());
                            ys.Add(reader.GetDouble(1));
                        }
                    }

                    this.formsPlot1.plt.PlotScatter(xs.ToArray(), ys.ToArray(), markerShape: ScottPlot.MarkerShape.none);
                }

                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT date, dividend FROM [Dividends] ORDER BY date ASC";

                    List<double> xs = new List<double>();
                    List<double> ys = new List<double>();

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {                        
                        while (reader.Read())
                        {
                            xs.Add(reader.GetDateTime(0).ToOADate());
                            ys.Add(reader.GetDouble(1));
                        }
                    }

                    this.formsPlot2.plt.PlotScatter(xs.ToArray(), ys.ToArray(), markerShape: ScottPlot.MarkerShape.none);
                }
            }

            this.formsPlot1.plt.Ticks(dateTimeX: true);
            this.formsPlot2.plt.Ticks(dateTimeX: true);

            // Render
            this.formsPlot1.Render();
            this.formsPlot2.Render();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGraphs();
        }
    }
}
