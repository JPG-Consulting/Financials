using System;

namespace Financials.DataSources.DataSources.Yahoo
{
    /// <summary>
    /// Represents a line in the CSV file downloaded from Yahoo Finance for historical dividends.
    /// </summary>
    public class HistoricalDividend
    {
        /// <summary>
        /// Gets or sets the date of the dividend.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the price paid per share.
        /// </summary>
        public decimal Dividend { get; set; }

        /// <summary>
        /// Returns a string that represents the current historical dividend.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}: {1}", this.Date.ToShortDateString(), this.Dividend);
        }
    }
}
