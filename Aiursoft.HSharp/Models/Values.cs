namespace Aiursoft.HSharp.Models
{
    public enum DocumentOptions
    {
        Empty,
        BasicHtml
    }
    static class Values
    {
        public static readonly string HtmlMatch = @"<(?'TN'\w+)\s*(?'TP'[^<>]*)>(?'TC'[\d\D]*?(((?'Open'<\k'TN'[^>]*>)[\d\D]*?)+((?'-Open'</\k'TN'[^>]*>)[\d\D]*?)+)*(?(Open)(?!)))</\k'TN'>|<(?'TN'\w+)\b*(?'TP'[^<>]*)/?\s*>|<!(?'TS'[^>]+)>";
        public static readonly string PropertiesMatch = $@"\s*([-\w]+)=""([^""]*?)""\s*";
        public static readonly string NoneHtmlMatch = $@"(?'RS'\w.+)";
    }
}