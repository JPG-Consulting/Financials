namespace Financials.DataSources.DataSources.Yahoo
{
    public class AutoCResponse
    {
        public AutoCResultSet ResultSet { get; set; }

    }

    public class AutoCResultSet
    {
        public string Query { get; set; }

        public AutoCResult[] Result { get; set; }
    }

    public class AutoCResult
    {
        [Newtonsoft.Json.JsonPropertyAttribute("symbol")]
        public string Symbol { get; set; }

        [Newtonsoft.Json.JsonPropertyAttribute("name")]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonPropertyAttribute("exch")]
        public string Exchange { get; set; }

        [Newtonsoft.Json.JsonPropertyAttribute("type")]
        public string Type { get; set; }

        [Newtonsoft.Json.JsonPropertyAttribute("exchDisp")]
        public string ExchangeDisplayName { get; set; }

        [Newtonsoft.Json.JsonPropertyAttribute("typeDisp")]
        public string TypeDisplayName { get; set; }
    }
}
