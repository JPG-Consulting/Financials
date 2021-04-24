using System;

namespace Financials.DataSources.DataSources.Yahoo
{
    /// <summary>
    /// Represents a line in the CSV file downloaded from Yahoo Finance for historical prices.
    /// </summary>
    public class HistoricalPrices
    {
        /// <summary>
        /// Gets or sets the date for this entry.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the opening price.
        /// </summary>
        public decimal Open { get; set; }

        /// <summary>
        /// Gets or sets the highest price.
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// Gets or sets the lowest prices.
        /// </summary>
        public decimal Low { get; set; }

        /// <summary>
        /// Gets or sets the closing price.
        /// </summary>
        public decimal Close { get; set; }

        /// <summary>
        /// Gets or sets the adjusted closing price.
        /// </summary>
        public decimal AdjustedClose { get; set; }

        /// <summary>
        /// Gets or set the traded volume.
        /// </summary>
        public decimal Volume { get; set; }
    }
}
