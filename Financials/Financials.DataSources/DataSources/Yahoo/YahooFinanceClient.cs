using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Financials.DataSources.DataSources.Yahoo
{
    public enum YahooFrequency
    {
        Daily,
        Weekly,
        Monthly
    }

    public enum YahooDownloadableEvents
    {
        Dividends,
        HistoricalPrices,
        StockSplits
    }

    public class YahooFinanceClient
    {
        protected Uri GetDownloadUri(string symbol, DateTime fromDate, DateTime toDate, YahooFrequency frequency, YahooDownloadableEvents show, bool includeAdjustedClose)
        {
            // Change DateTime to Unix timestamps.
            DateTimeOffset period1 = (fromDate < toDate) ? new DateTimeOffset(fromDate.Date) : new DateTimeOffset(toDate.Date);
            DateTimeOffset period2 = (fromDate < toDate) ? new DateTimeOffset(toDate.Date) : new DateTimeOffset(fromDate.Date);

            // Call the overidable method.
            return GetDownloadUri(symbol, period1.ToUnixTimeSeconds(), period2.ToUnixTimeSeconds(), frequency, show, includeAdjustedClose);
        }

        protected Uri GetDownloadUri(string symbol, DateTime fromDate, int interval, YahooFrequency frequency, YahooDownloadableEvents show, bool includeAdjustedClose)
        {
            DateTime toDate = DateTime.Today;

            // Calculate the end date.
            switch (frequency)
            {
                case YahooFrequency.Daily:
                    toDate = fromDate.AddDays(Convert.ToDouble(interval));
                    break;
                case YahooFrequency.Monthly:
                    toDate = fromDate.AddMonths(interval);
                    break;
                case YahooFrequency.Weekly:
                    toDate = fromDate.AddDays(Convert.ToDouble(interval * 7));
                    break;
            }

            // Do not lookup in the future.
            if (toDate > DateTime.Today)
                toDate = DateTime.Today;

            // Change DateTime to Unix timestamps.
            DateTimeOffset period1 = (fromDate < toDate) ? new DateTimeOffset(fromDate.Date) : new DateTimeOffset(toDate.Date);
            DateTimeOffset period2 = (fromDate < toDate) ? new DateTimeOffset(toDate.Date) : new DateTimeOffset(fromDate.Date);

            // Call the overidable method.
            return GetDownloadUri(symbol, period1.ToUnixTimeSeconds(), period2.ToUnixTimeSeconds(), frequency, show, includeAdjustedClose);
        }

        protected virtual Uri GetDownloadUri(string symbol, long period1, long period2, YahooFrequency frequency, YahooDownloadableEvents show, bool includeAdjustedClose)
        {
            string interval = null;
            string events = null;

            // frequency parameter.
            switch (frequency)
            {
                case YahooFrequency.Daily:
                    interval = "1d";
                    break;
                case YahooFrequency.Monthly:
                    interval = "1m";
                    break;
                case YahooFrequency.Weekly:
                    interval = "1wk";
                    break;
                default:
                    throw new ArgumentException(ExceptionHelper.GetString("InvalidParameterValue_Generic"));
            }

            // events parameter.
            switch (show)
            {
                case YahooDownloadableEvents.Dividends:
                    events = "div";
                    break;
                case YahooDownloadableEvents.HistoricalPrices:
                    events = "history";
                    break;
                case YahooDownloadableEvents.StockSplits:
                    events = "split";
                    break;
                default:
                    throw new ArgumentException(ExceptionHelper.GetString("InvalidParameterValue_Generic"));
            }

            // Generate the uri string.
            string uriString = String.Format("https://query1.finance.yahoo.com/v7/finance/download/{0}?period1={1}&period2={2}&interval={3}&events={4}&includeAdjustedClose={5}", symbol, period1, period2, interval, events, includeAdjustedClose);

            return new Uri(uriString);
        }

        /// <summary>
        /// Creates a <see cref="WebRequest"/> for the specified <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> to use when creating the <see cref="WebRequest"/>.</param>
        /// <returns>The <see cref="WebRequest"/> for the specified <see cref="Uri"/>.</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="uri"/> parameter is <c>null</c>.</exception>
        protected virtual WebRequest GetWebRequest(Uri uri)
        {
            if (uri == null)
                throw new InvalidOperationException(ExceptionHelper.GetFormattedString("ParameterNull", "uri"));

            return WebRequest.Create(uri);
        }

        /// <summary>
        /// Returns a response from a request to a url.
        /// </summary>
        /// <returns>A response from a synchronous request to a url.</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="request"/> parameter is <c>null</c>.</exception>
        protected virtual WebResponse GetWebResponse(WebRequest request)
        {
            if (request == null)
                throw new InvalidOperationException(ExceptionHelper.GetFormattedString("ParameterNull", "request"));
            
            return request.GetResponse();
        }

        /// <summary>
        /// Returns a response from a request to a url.
        /// </summary>
        /// <returns>A response from a asynchronous request to a url.</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="request"/> parameter is <c>null</c>.</exception>
        protected virtual WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            if (request == null)
                throw new InvalidOperationException(ExceptionHelper.GetFormattedString("ParameterNull", "request"));

            return request.EndGetResponse(result);
        }

        public HistoricalDividend[] GetHistoricalDividends(string symbol, DateTime from, DateTime to, YahooFrequency frequency)
        {
            Uri requestUri = GetDownloadUri(symbol, from, to, frequency, YahooDownloadableEvents.Dividends, true);
            WebRequest request = GetWebRequest(requestUri);

            WebResponse response = GetWebResponse(request);

            // Parse CSV file
            Stream responseStream = response.GetResponseStream();

            List<HistoricalDividend> dividends = new List<HistoricalDividend>();

            using (StreamReader reader = new StreamReader(responseStream))
            {
                // Empty file: no dividends.
                if (reader.EndOfStream)
                    return new HistoricalDividend[0];

                // Process the header line
                String headerLine = reader.ReadLine();
                String[] headerFields = headerLine.Split(new char[] { ',' });

                // Culture for CSV file.
                CultureInfo csvCulture = new CultureInfo("en-US");

                while (!reader.EndOfStream)
                {
                    string dataLine = reader.ReadLine();

                    if (String.IsNullOrWhiteSpace(dataLine))
                        continue;

                    String[] dataFields = dataLine.Split(new char[] { ',' });

                    // We can set the DTO values.
                    HistoricalDividend dividend = new HistoricalDividend() {
                        Date = DateTime.ParseExact(dataFields[0], "yyyy-MM-dd", csvCulture),
                        Dividend = Convert.ToDecimal(dataFields[1], csvCulture)
                    };

                    dividends.Add(dividend);
                }
            }

            if (dividends.Count < 1)
                return new HistoricalDividend[0];

            // Return dividends shorted by date.
            return dividends.OrderBy(o => o.Date).ToArray();
        }


    }
}
