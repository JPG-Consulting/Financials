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
    public partial class SymbolSearchDialog : Form
    {
        /// <summary>
        /// Gets the search result.
        /// </summary>
        public AutoCResult SearchResult { get; private set; }

        public SymbolSearchDialog()
        {
            InitializeComponent();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string query = this.textBoxSearchQuery.Text.Trim();

            this.Enabled = false;

            try
            {
                Search(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (this.listViewSearchResults.SelectedIndices.Count != 1)
            {
                MessageBox.Show("No symbol has been selected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            this.SearchResult = this.listViewSearchResults.SelectedItems[0].Tag as AutoCResult;

            if (this.SearchResult != null)
                this.DialogResult = DialogResult.OK;
        }

        private void Search(string query)
        {
            if (String.IsNullOrWhiteSpace(query))
                throw new InvalidOperationException("Must specify something to search for.");

            query = query.Trim();

            this.listViewSearchResults.BeginUpdate();

            try
            {
                // Clear results.
                this.listViewSearchResults.Items.Clear();

                YahooFinanceClient client = new YahooFinanceClient();
                AutoCResponse response = client.Search(query);

                // Check we have results.
                if ((response == null) || (response.ResultSet == null) || (response.ResultSet.Result == null) || (response.ResultSet.Result.Length < 1))
                    return;

                // Cache.
                try
                {
                    YahooCache cache = new YahooCache();
                    {
                        cache.Save(response);
                    }
                }
                catch
                {
                    // If something goes wrong while caching data it should continue on.
                }

                // Load results
                for (int i = 0; i < response.ResultSet.Result.Length; i++)
                {
                    AutoCResult currentResult = response.ResultSet.Result[i];

                    ListViewItem lvi = new ListViewItem(currentResult.Symbol);
                    lvi.SubItems.Add(currentResult.Name);
                    lvi.SubItems.Add(currentResult.ExchangeDisplayName);
                    lvi.SubItems.Add(currentResult.TypeDisplayName);
                    lvi.Tag = currentResult;

                    this.listViewSearchResults.Items.Add(lvi);
                }
            }
            catch
            {
                // Clear results just in case we added some.
                this.listViewSearchResults.Items.Clear();

                // Rethrow exception.
                throw;
            }
            finally
            {
                this.listViewSearchResults.EndUpdate();
            }
        }

        private void textBoxSearchQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Search(this.textBoxSearchQuery.Text.Trim());
        }
    }
}
