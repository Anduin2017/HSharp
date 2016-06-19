namespace Obisoft.HSharp.Models
{
    public class HProp
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public HProp(string Key, string Value)
        {
            this.Key = Key;
            this.Value = Value;
        }
    }
}
