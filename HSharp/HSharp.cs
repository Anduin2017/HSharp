using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Obisoft.HSharp
{
    public enum DocumentOptions
    {
        Empty,
        BasicHTML
    }
    static class Values
    {
        public static string HTMLMatch = @"<(?'TN'\w+)\s*(?'TP'[^<>]*)>(?'TC'[\d\D]*?(((?'Open'<\k'TN'[^>]*>)[\d\D]*?)+((?'-Open'</\k'TN'[^>]*>)[\d\D]*?)+)*(?(Open)(?!)))</\k'TN'>|<(?'TN'\w+)\b*(?'TP'[^<>]*)/\s*>";
        public static string PropertiesMatch = $@"\s*([-\w]+)={"\"([^\"]*?)\""}\s*";
        public static string NoneHTMLMatch = $@"(?'RS'\w+)";
    }
    public class Property
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public Property(string Key, string Value)
        {
            this.Key = Key;
            this.Value = Value;
        }
    }
    public class Document : IEnumerable<Tag>
    {
        private List<Tag> _TempList = new List<Tag>();
        private void _GetAll(Tag Tag)
        {
            _TempList.Add(Tag);
            Tag.Children.ForEach(t => _GetAll(t));
        }
        protected List<Tag> _MatchTag(string HTML)
        {
            var ReturnList = new List<Tag>();
            foreach (var Part in Regex.Matches(HTML, Values.HTMLMatch + "|" + Values.NoneHTMLMatch))
            {
                var PartMatch = Part as Match;
                var TC = PartMatch.Groups["TC"].Value;
                var TN = PartMatch.Groups["TN"].Value;
                var TP = PartMatch.Groups["TP"].Value;
                var RS = PartMatch.Groups["RS"].Value;
                //TextTag
                if (string.IsNullOrEmpty(TN))
                {
                    var Element = new TextTag(RS);
                    ReturnList.Add(Element);
                }
                //Tag
                else
                {
                    var Properties = new List<Property>();
                    foreach (var PropertyPart in Regex.Matches(TP, Values.PropertiesMatch))
                    {
                        var PropertyMatch = PropertyPart as Match;
                        var Key = PropertyMatch.Groups[1].Value;
                        var Value = PropertyMatch.Groups[2].Value;
                        Properties.Add(new Property(Key, Value));
                    }
                    var Element = new Tag(TN, Properties);
                    Element.Children = _MatchTag(TC);
                    Element.Children.ForEach(t => t.Parent = Element);
                    ReturnList.Add(Element);
                }
            }
            return ReturnList;
        }

        public List<Tag> Children { get; set; } = new List<Tag>();
        public Tag Son => Children[0];
        public List<Tag> AllUnder
        {
            get
            {
                _TempList.Clear();
                Children.ForEach(t => _GetAll(t));
                return _TempList;
            }
        }
        public Tag this[string TagName] => Children.Find(t => t.TagName == TagName);
        public Tag this[int TagIndex] => Children[TagIndex];
        public Tag this[string TagName, int Index] => Children.Where(t => t.TagName == TagName).ToList()[Index];

        public Document()
        {

        }
        public Document(string SourceHTML)
        {
            Children = _MatchTag(SourceHTML);
        }
        public Document(DocumentOptions Options)
        {
            if (Options == DocumentOptions.BasicHTML)
            {
                var HTML = new Tag("html",
                  new Tag("head",
                      new Tag("meta", new Property("charset", "utf-8")),
                      new Tag("title", "Example")),
                  new Tag("body"));
                Children.Add(HTML);
            }
        }

        public virtual string GenerateHTML()
        {
            string Result = string.Empty;
            Children.ForEach(t => Result += t.GenerateHTML() + "\r\n");
            return Result;
        }
        public virtual void AddChild(Tag Tag)
        {
            Children.Add(Tag);
        }
        public virtual void AddChild(string TagName)
        {
            AddChild(new Tag(TagName));
        }
        public virtual void AddChild(string TagName, string InnerContent)
        {
            AddChild(new Tag(TagName, new TextTag(InnerContent)));
        }
        public virtual void AddChild(string TagName, params Tag[] Children)
        {
            AddChild(new Tag(TagName, Children));
        }
        public virtual void AddChild(string TagName, IEnumerable<Tag> Children)
        {
            AddChild(new Tag(TagName, Children));
        }
        public virtual void AddChild(string TagName, params Property[] Properties)
        {
            AddChild(new Tag(TagName, Properties));
        }
        public virtual void AddChild(string TagName, IEnumerable<Property> Properties)
        {
            AddChild(new Tag(TagName, Properties));
        }

        public virtual void AddChildren(params Tag[] Children)
        {
            foreach (var Child in Children)
            {
                AddChild(Child);
            }
        }
        public virtual void AddChildren(IEnumerable<Tag> Children)
        {
            Children.ToList().ForEach(t => AddChild(t));
        }

        public IEnumerator<Tag> GetEnumerator()
        {
            return ((IEnumerable<Tag>)Children).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Tag>)Children).GetEnumerator();
        }
    }

    public class Tag : Document
    {
        public string TagName { get; set; }
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
        public Tag Parent { get; set; }

        public Tag(string TagName)
        {
            this.TagName = TagName;
        }
        public Tag(string TagName, string InnerContent) : this(TagName)
        {
            AddChild(new TextTag(InnerContent));
        }
        public Tag(string TagName, params Tag[] Children) : this(TagName)
        {
            AddChildren(Children);
        }
        public Tag(string TagName, IEnumerable<Tag> Children)
        {
            AddChildren(Children);
        }
        public Tag(string TagName, params Property[] Properties) : this(TagName)
        {
            AddProperties(Properties);
        }
        public Tag(string TagName, IEnumerable<Property> Properties) : this(TagName)
        {
            AddProperties(Properties);
        }

        public void AddProperties(params Property[] Properties)
        {
            foreach (var property in Properties)
            {
                this.Properties.Add(property.Key, property.Value);
            }
        }
        public void AddProperties(IEnumerable<Property> Properties)
        {
            foreach (var property in Properties)
            {
                this.Properties.Add(property.Key, property.Value);
            }
        }

        public override string GenerateHTML()
        {
            string PropertiesResult = string.Empty;
            foreach (var Property in Properties)
            {
                PropertiesResult += $" {Property.Key}=\"{Property.Value}\"";
            }
            if (Children.Count == 0)
            {
                return $"<{TagName + PropertiesResult}></{TagName}>";
            }
            else
            {
                string AllChildren = string.Empty;
                Children.ForEach(t => AllChildren += $"{t.GenerateHTML()}\r\n");
                return $"<{TagName + PropertiesResult}>\r\n{AllChildren}</{TagName}>";
            }
        }
        public override string ToString() => TagName;
    }
    public class TextTag : Tag
    {
        public string InnerContent { get; set; }
        public TextTag() : base(string.Empty)
        {
        }
        public TextTag(string InnerContent) : base(string.Empty)
        {
            this.InnerContent = InnerContent;
        }
        public override string GenerateHTML() => InnerContent;
        public override string ToString() => InnerContent;
    }

    public static class HtmlConvert
    {
        public static string SerializeHtml(Document Document)
        {
            return Document.GenerateHTML();
        }
        public static Document DeserializeHtml(string HTML)
        {
            var Document = new Document(HTML);
            return Document;
        }
    }
}
