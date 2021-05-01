using Financials.Cache.Yahoo;
using Financials.DataSources.DataSources.Yahoo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancialsApp.Dialogs
{
    public partial class HistoricalDividendDialog : Form
    {
        private string _cachedSymbol = null;
        private DateTime _cachedStartDate = DateTime.MinValue;
        private DateTime _cachedEndDate = DateTime.MaxValue;
        private HistoricalDividend[] _cachedDividends = null;

        public HistoricalDividendDialog()
        {
            InitializeComponent();

            this.dateTimePickerEndDate.Value = DateTime.Today;
            this.toolStripComboGraphInterval.SelectedIndex = 1;
        }

        private bool HasCachedData(string symbol, DateTime startDate, DateTime endDate)
        {
            return (symbol.Equals(this._cachedSymbol) && startDate.Equals(this._cachedStartDate) && endDate.Equals(this._cachedEndDate));
        }

        private void GenerateGraph()
        {

            this.formsPlot1.Reset();

            if ((this._cachedDividends == null) || (this._cachedDividends.Length < 1))
                return;

            double[] xs = new double[this._cachedDividends.Length];
            double[] ys = new double[this._cachedDividends.Length];

            int currentYear = this._cachedDividends[0].Date.Year;
            int currentIndex = 0;
            decimal totalDividends = 0;

            for (int index = 0; index < this._cachedDividends.Length; index++)
            {
                if (this.toolStripComboGraphInterval.SelectedIndex == 1)
                {
                    if (this._cachedDividends[index].Date.Year.Equals(currentYear))
                    {
                        // Add and continue.
                        totalDividends += this._cachedDividends[index].Dividend;
                        continue;
                    }

                    ys[currentIndex] = Convert.ToDouble(totalDividends);
                    xs[currentIndex] = currentYear;

                    currentYear = this._cachedDividends[index].Date.Year;
                    totalDividends = this._cachedDividends[index].Dividend;
                    currentIndex++;
                }
                else
                {
                    xs[currentIndex] = this._cachedDividends[index].Date.Date.ToOADate();
                    ys[currentIndex] = Convert.ToDouble(this._cachedDividends[index].Dividend);

                    currentIndex++;
                }
            }

            // If anually do not plot the current year.
            if (this.toolStripComboGraphInterval.SelectedIndex == 1)
            {
                if (!currentYear.Equals(DateTime.Now.Year))
                {
                    ys[currentIndex] = Convert.ToDouble(totalDividends);
                    xs[currentIndex] = currentYear;
                    currentIndex++;
                }

                Array.Resize(ref xs, currentIndex);
                Array.Resize(ref ys, currentIndex);
            }

            if (this.toolStripComboGraphInterval.SelectedIndex == 1)
            {
                formsPlot1.plt.XLabel("Years");
            }
            else
            {
                this.formsPlot1.plt.Ticks(dateTimeX: true);
                formsPlot1.plt.XLabel("Dates");
            }

            this.formsPlot1.plt.PlotScatter(xs, ys);

            // Additional styling
            formsPlot1.plt.Title("Dividends");
            formsPlot1.plt.YLabel("Dividend per share");

            // Render
            this.formsPlot1.Render();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            YahooFinanceClient client = new YahooFinanceClient();

            this.panelTop.Enabled = false;

            this.listViewDividendData.BeginUpdate();
            
            try
            {
                if (String.IsNullOrWhiteSpace(this.textBoxSymbol.Text))
                    throw new Exception("Must provide a stock symbol.");

                DateTime from = new DateTime(1980, 1, 1);
                DateTime to = DateTime.Now;


                HistoricalDividend[] dividends = null;

                if (HasCachedData(this.textBoxSymbol.Text.Trim(), this.dateTimePickerStartDate.Value.Date, this.dateTimePickerEndDate.Value.Date))
                {
                    dividends = this._cachedDividends;
                }
                else
                {
                    dividends = client.GetHistoricalDividends(this.textBoxSymbol.Text.Trim(), this.dateTimePickerStartDate.Value.Date, this.dateTimePickerEndDate.Value.Date, YahooFrequency.Daily);

                    if ((dividends != null) && (dividends.Length > 0))
                    {
                        YahooCache cache = new YahooCache();
                        cache.SaveHistoricalDividends(this.textBoxSymbol.Text, dividends);
                    }

                    this._cachedSymbol = this.textBoxSymbol.Text.Trim();
                    this._cachedStartDate = this.dateTimePickerStartDate.Value.Date;
                    this._cachedEndDate = this.dateTimePickerEndDate.Value.Date;
                    this._cachedDividends = dividends;
                }

                if ((dividends != null) && (dividends.Length > 0))
                {
                    for (int index = 0; index < dividends.Length; index++)
                    {
                        ListViewItem lvi = new ListViewItem(dividends[index].Date.ToShortDateString());
                        lvi.SubItems.Add(dividends[index].Dividend.ToString("F4"));

                        this.listViewDividendData.Items.Add(lvi);
                    }
                }
            }
            catch (Exception ex)
            {
                this.listViewDividendData.Items.Clear();

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Generate the graph. 
                GenerateGraph();

                this.listViewDividendData.EndUpdate();
                this.panelTop.Enabled = true;
            }
        }

        private void toolStripComboGraphInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateGraph();
        }

        private void buttonSearchSymbol_Click(object sender, EventArgs e)
        {
            SymbolSearchDialog dialog = new SymbolSearchDialog();

            if ((dialog.ShowDialog(this) == DialogResult.OK) && (dialog.SearchResult != null))
            {
                this.textBoxSymbol.Text = dialog.SearchResult.Symbol;
            }
        }
    }
}
