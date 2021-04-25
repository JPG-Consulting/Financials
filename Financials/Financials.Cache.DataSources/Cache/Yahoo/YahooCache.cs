using Financials.DataSources.DataSources.Yahoo;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Financials.Cache.Yahoo
{
    public class YahooCache
    {
        private readonly string _cacheDirectory;

        private void CreateDatabase(string templateName, string path)
        {
            using (Stream stream = GetDatabaseTemplateStream(templateName))
            {
                using (FileStream outStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(outStream);
                    outStream.Flush();
                }
            }
        }

        private Stream GetDatabaseTemplateStream(string name)
        {
            string templatePath = String.Format("Financials.Cache.Yahoo.DatabaseTemplates.{0}.sqlite3", name);

            TypeInfo typeInfo = typeof(YahooCache).GetTypeInfo();
            Assembly assembly = typeInfo.Assembly;

            return assembly.GetManifestResourceStream(templatePath);
        }
        
        private void SaveAutoCResponse(AutoCResponse autoCResponse, CultureInfo culture)
        {
            // If no results, then return right away.
            if ((autoCResponse == null) || (autoCResponse.ResultSet == null) || (autoCResponse.ResultSet.Result == null) || (autoCResponse.ResultSet.Result.Length < 1))
                return;

            // Create tha path to the cache database.
            string dbPath = Path.Combine(this._cacheDirectory, "_YahooCache.sqlite3");

            // Create the database if it doesn't exit.
            if (!File.Exists(dbPath))
                CreateDatabase("YahooCache", dbPath);

            // Get unique types.
            Dictionary<string, string> uniqueTypes = new Dictionary<string, string>();
            Dictionary<string, string> uniqueExchanges = new Dictionary<string, string>();

            foreach (AutoCResult currentResult in autoCResponse.ResultSet.Result)
            {
                // Unique types
                if (!uniqueTypes.ContainsKey(currentResult.Type))
                    uniqueTypes.Add(currentResult.Type, currentResult.TypeDisplayName);

                // Unique exchanges
                if (!uniqueExchanges.ContainsKey(currentResult.Exchange))
                    uniqueExchanges.Add(currentResult.Exchange, currentResult.ExchangeDisplayName);
            }

            // Start the connection.
            using (SQLiteConnection connection = new SQLiteConnection(String.Format("Data Source={0};Version=3;", dbPath)))
            {
                connection.Open();

                try
                {
                    //
                    // AutoC_Types
                    //
                    // INSERT INTO AutoC_Types (type, lang, display) VALUES (@type, @lang, @display)
                    foreach (KeyValuePair<string, string> currentType in uniqueTypes)
                    {
                        using (SQLiteCommand command = connection.CreateCommand())
                        {
                            try
                            {
                                command.CommandText = "SELECT COUNT(type) FROM [AutoC_Types] WHERE type=@type AND lang=@lang";
                                command.Parameters.AddWithValue("@type", currentType.Key);
                                command.Parameters.AddWithValue("@lang", culture.Name);

                                Int64 typeCount = Convert.ToInt64(command.ExecuteScalar());

                                if (typeCount == 0)
                                {
                                    command.CommandText = "INSERT INTO [AutoC_Types] (type, lang, display) VALUES (@type, @lang, @display)";
                                    command.Parameters.AddWithValue("@display", currentType.Value);

                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }
                    }

                    //
                    // AutoC_Exchange
                    //
                    // INSERT INTO AutoC_Exchange (exch, lang, display) VALUES (@exch, @lang, @display)
                    foreach (KeyValuePair<string, string> currentType in uniqueExchanges)
                    {
                        using (SQLiteCommand command = connection.CreateCommand())
                        {
                            try
                            {
                                command.CommandText = "SELECT COUNT(exch) FROM [AutoC_Exchanges] WHERE exch=@exch AND lang=@lang";
                                command.Parameters.AddWithValue("@exch", currentType.Key);
                                command.Parameters.AddWithValue("@lang", culture.Name);

                                Int64 typeCount = Convert.ToInt64(command.ExecuteScalar());

                                if (typeCount == 0)
                                {
                                    command.CommandText = "INSERT INTO [AutoC_Exchanges] (exch, lang, display) VALUES (@exch, @lang, @display)";
                                    command.Parameters.AddWithValue("@display", currentType.Value);

                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }
                    }

                    //
                    // AutoC_Results
                    //
                    // INSERT INTO AutoCResults (symbol, type, name,  exch) VALUES (@symbol, @type, @name,  @exch)
                    foreach (AutoCResult currentResult in autoCResponse.ResultSet.Result)
                    {
                        using (SQLiteCommand command = connection.CreateCommand())
                        {
                            try
                            {
                                command.CommandText = "SELECT COUNT(symbol) FROM [AutoC_Results] WHERE symbol=@symbol AND type=@type";
                                command.Parameters.AddWithValue("@symbol", currentResult.Symbol);
                                command.Parameters.AddWithValue("@type", currentResult.Type);

                                Int64 typeCount = Convert.ToInt64(command.ExecuteScalar());

                                if (typeCount == 0)
                                {
                                    command.CommandText = "INSERT INTO [AutoC_Results] (symbol, type, name, exch) VALUES (@symbol, @type, @name, @exch)";
                                    command.Parameters.AddWithValue("@name", currentResult.Name);
                                    command.Parameters.AddWithValue("@exch", currentResult.Exchange);

                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }
                    }

                    //
                    // AutoC_Queries
                    //
                    // INSERT INTO AutoCQueries (query) VALUES (@query)
                }
                finally
                {
                    if (connection.State != System.Data.ConnectionState.Closed)
                        connection.Close();
                }
            }
        }

        public void Save(object obj)
        {
            Save(obj, CultureInfo.CurrentUICulture);
        }

        public void Save(object obj, CultureInfo culture)
        {
            if (obj is AutoCResponse)
                SaveAutoCResponse((AutoCResponse)obj, culture);
        }
        
        public void SaveHistoricalDividends(string symbol, HistoricalDividend[] dividends)
        {
            if (String.IsNullOrWhiteSpace(symbol) || (dividends == null) || (dividends.Length < 1))
                return;

            // Create tha path to the cache database.
            string dbPath = Path.Combine(this._cacheDirectory, String.Format("{0}_Historical.sqlite3", symbol));

            // Create the database if it doesn't exit.
            if (!File.Exists(dbPath))
                CreateDatabase("SymbolCache", dbPath);

            SQLiteTransaction transaction = null;

            using (SQLiteConnection connection = new SQLiteConnection(String.Format("Data Source={0};Version=3;", dbPath)))
            {
                connection.Open();

                try
                {
                    List<DateTime> insertedDates = new List<DateTime>();

                    transaction = connection.BeginTransaction();

                    foreach (HistoricalDividend dividend in dividends)
                    {
                        using (SQLiteCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "SELECT COUNT(date) FROM [Dividends] WHERE date=@date";
                            command.Parameters.AddWithValue("@date", dividend.Date);

                            Int64 rowCount = Convert.ToInt64(command.ExecuteScalar());

                            // Allow duplicates as dividends may be split in 2 payments the same day.
                            if ((rowCount == 0) || (insertedDates.Contains(dividend.Date)))
                            {
                                command.CommandText = "INSERT INTO [Dividends] (date, dividend) VALUES (@date, @dividend)";
                                command.Parameters.AddWithValue("@dividend", dividend.Dividend);
                                command.ExecuteNonQuery();

                                if (!insertedDates.Contains(dividend.Date))
                                    insertedDates.Add(dividend.Date);
                            }
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    if (transaction != null)
                        transaction.Rollback();
                    throw;
                }
                finally
                {
                    if (connection.State != System.Data.ConnectionState.Closed)
                        connection.Close();
                }
            }
        }

        public YahooCache()
            : this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache"))
        { }

        public YahooCache(string path)
        {
            this._cacheDirectory = Path.GetFullPath((String.IsNullOrWhiteSpace(path) ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache") : path));

            if (!Directory.Exists(this._cacheDirectory))
                Directory.CreateDirectory(this._cacheDirectory);
        }
    }
}
