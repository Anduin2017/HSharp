namespace Anduin.HSharp.Models
{
    public class HProp
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public HProp(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
