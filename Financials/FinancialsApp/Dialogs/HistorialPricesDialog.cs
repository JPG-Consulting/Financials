using Financials.DataSources.DataSources.Yahoo;
using System;
using System.Windows.Forms;

namespace FinancialsApp.Dialogs
{
    public partial class HistorialPricesDialog : Form
    {
        private string _cachedSymbol = null;
        private DateTime _cachedStartDate = DateTime.MinValue;
        private DateTime _cachedEndDate = DateTime.MaxValue;
        private HistoricalPrices[] _cachedPrices = null;

        public HistorialPricesDialog()
        {
            InitializeComponent();
        }

        private void GenerateGraph()
        {

            this.formsPlot1.Reset();

            if ((this._cachedPrices == null) || (this._cachedPrices.Length < 1))
                return;

            double[] xs = new double[this._cachedPrices.Length];
            double[] ys = new double[this._cachedPrices.Length];

            double[] lowXs = new double[this._cachedPrices.Length];
            double[] lowYs = new double[this._cachedPrices.Length];

            double[] highXs = new double[this._cachedPrices.Length];
            double[] highYs = new double[this._cachedPrices.Length];


            int currentIndex = 0;

            for (int index = 0; index < this._cachedPrices.Length; index++)
            {
                xs[currentIndex] = this._cachedPrices[index].Date.Date.ToOADate();
                ys[currentIndex] = Convert.ToDouble(this._cachedPrices[index].Close);

                lowXs[currentIndex] = this._cachedPrices[index].Date.Date.ToOADate();
                lowYs[currentIndex] = Convert.ToDouble(this._cachedPrices[index].Low);

                highXs[currentIndex] = this._cachedPrices[index].Date.Date.ToOADate();
                highYs[currentIndex] = Convert.ToDouble(this._cachedPrices[index].High);

                currentIndex++;
            }

           

            this.formsPlot1.plt.Ticks(dateTimeX: true);
            formsPlot1.plt.XLabel("Dates");

            this.formsPlot1.plt.PlotScatter(xs, ys, System.Drawing.Color.Black, markerShape: ScottPlot.MarkerShape.none);

            // Si el margen es muy amplio sólo muestro el precio.
            if (xs[0] >= DateTime.Now.Subtract(new TimeSpan(400, 0, 0, 0)).ToOADate())
            {
                this.formsPlot1.plt.PlotScatter(lowXs, lowYs, System.Drawing.Color.Red, markerShape: ScottPlot.MarkerShape.none);

                this.formsPlot1.plt.PlotScatter(highXs, highYs, System.Drawing.Color.Green, markerShape: ScottPlot.MarkerShape.none);
            }

            // Additional styling
            DateTime startDate = DateTime.FromOADate(xs[0]);
            DateTime endDate = DateTime.FromOADate(xs[xs.Length - 1]);
            formsPlot1.plt.Title(String.Format("{0} prices ({1} - {2})", this._cachedSymbol, startDate.ToShortDateString(), endDate.ToShortDateString()));
            formsPlot1.plt.YLabel("Price per share");

            // Render
            this.formsPlot1.Render();
        }

        private bool HasCachedData(string symbol, DateTime startDate, DateTime endDate)
        {
            return (symbol.Equals(this._cachedSymbol) && startDate.Equals(this._cachedStartDate) && endDate.Equals(this._cachedEndDate));
        }

        private void buttonSearchSymbol_Click(object sender, EventArgs e)
        {
            SymbolSearchDialog dialog = new SymbolSearchDialog();

            if ((dialog.ShowDialog(this) == DialogResult.OK) && (dialog.SearchResult != null))
            {
                this.textBoxSymbol.Text = dialog.SearchResult.Symbol;

                // Simulamos la busqueda.
                buttonSearch_Click(sender, e);
            }
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


                HistoricalPrices[] prices = null;

                if (HasCachedData(this.textBoxSymbol.Text.Trim(), this.dateTimePickerStartDate.Value.Date, this.dateTimePickerEndDate.Value.Date))
                {
                    prices = this._cachedPrices;
                }
                else
                {
                    prices = client.GetHistoricalPrices(this.textBoxSymbol.Text.Trim(), this.dateTimePickerStartDate.Value.Date, this.dateTimePickerEndDate.Value.Date, YahooFrequency.Daily);

                    if ((prices != null) && (prices.Length > 0))
                    {
                        //YahooCache cache = new YahooCache();
                        //cache.SaveHistoricalPrices(this.textBoxSymbol.Text, prices);
                    }

                    this._cachedSymbol = this.textBoxSymbol.Text.Trim();
                    this._cachedStartDate = this.dateTimePickerStartDate.Value.Date;
                    this._cachedEndDate = this.dateTimePickerEndDate.Value.Date;
                    this._cachedPrices = prices;
                }

                if ((prices != null) && (prices.Length > 0))
                {
                    for (int index = 0; index < prices.Length; index++)
                    {
                        //ListViewItem lvi = new ListViewItem(prices[index].Date.ToShortDateString());
                        //lvi.SubItems.Add(prices[index].Dividend.ToString("F4"));

                        //this.listViewDividendData.Items.Add(lvi);
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

        private void textBoxSymbol_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(this.textBoxSymbol.Text))
                    buttonSearch_Click(sender, e);
                else
                    buttonSearchSymbol_Click(sender, e);
            }
        }

        private void HistorialPricesDialog_Load(object sender, EventArgs e)
        {
            this.dateTimePickerStartDate.Value = DateTime.Now.Subtract(new TimeSpan(200, 0, 0, 0));
            this.dateTimePickerEndDate.Value= DateTime.Now;
        }
    }
}
