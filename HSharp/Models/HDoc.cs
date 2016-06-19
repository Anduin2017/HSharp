using Obisoft.HSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Obisoft.HSharp
{
    public class HDoc : IEnumerable<HTag>
    {
        private List<HTag> _TempList = new List<HTag>();
        private void _GetAll(HTag Tag)
        {
            _TempList.Add(Tag);
            Tag.Children.ForEach(t => _GetAll(t));
        }
        protected List<HTag> _MatchTag(string HTML)
        {
            var ReturnList = new List<HTag>();
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
                    var Element = new HTextTag(RS);
                    ReturnList.Add(Element);
                }
                //Tag
                else
                {
                    var Properties = new List<HProp>();
                    foreach (var PropertyPart in Regex.Matches(TP, Values.PropertiesMatch))
                    {
                        var PropertyMatch = PropertyPart as Match;
                        var Key = PropertyMatch.Groups[1].Value;
                        var Value = PropertyMatch.Groups[2].Value;
                        Properties.Add(new HProp(Key, Value));
                    }
                    var Element = new HTag(TN, Properties);
                    Element.Children = _MatchTag(TC);
                    Element.Children.ForEach(t => t.Parent = Element);
                    ReturnList.Add(Element);
                }
            }
            return ReturnList;
        }

        public List<HTag> Children { get; set; } = new List<HTag>();
        public HTag Son => Children[0];
        public List<HTag> AllUnder
        {
            get
            {
                _TempList.Clear();
                Children.ForEach(t => _GetAll(t));
                return _TempList;
            }
        }
        public HTag this[string TagName] => Children.Find(t => t.TagName == TagName);
        public HTag this[int TagIndex] => Children[TagIndex];
        public HTag this[string TagName, int Index] => Children.Where(t => t.TagName == TagName).ToList()[Index];

        public HDoc()
        {

        }
        public HDoc(string SourceHTML)
        {
            Children = _MatchTag(SourceHTML);
        }
        public HDoc(DocumentOptions Options)
        {
            if (Options == DocumentOptions.BasicHTML)
            {
                var HTML = new HTag("html",
                  new HTag("head",
                      new HTag("meta", new HProp("charset", "utf-8")),
                      new HTag("title", "Example")),
                  new HTag("body"));
                Children.Add(HTML);
            }
        }

        public virtual string GenerateHTML()
        {
            string Result = string.Empty;
            Children.ForEach(t => Result += t.GenerateHTML() + "\r\n");
            return Result;
        }
        public virtual void AddChild(HTag Tag)
        {
            Children.Add(Tag);
        }
        public virtual void AddChild(string TagName)
        {
            AddChild(new HTag(TagName));
        }
        public virtual void AddChild(string TagName, string InnerContent)
        {
            AddChild(new HTag(TagName, new HTextTag(InnerContent)));
        }
        public virtual void AddChild(string TagName, params HTag[] Children)
        {
            AddChild(new HTag(TagName, Children));
        }
        public virtual void AddChild(string TagName, IEnumerable<HTag> Children)
        {
            AddChild(new HTag(TagName, Children));
        }
        public virtual void AddChild(string TagName, params HProp[] Properties)
        {
            AddChild(new HTag(TagName, Properties));
        }
        public virtual void AddChild(string TagName, IEnumerable<HProp> Properties)
        {
            AddChild(new HTag(TagName, Properties));
        }

        public virtual void AddChildren(params HTag[] Children)
        {
            foreach (var Child in Children)
            {
                AddChild(Child);
            }
        }
        public virtual void AddChildren(IEnumerable<HTag> Children)
        {
            Children.ToList().ForEach(t => AddChild(t));
        }

        public IEnumerator<HTag> GetEnumerator()
        {
            return ((IEnumerable<HTag>)Children).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<HTag>)Children).GetEnumerator();
        }
    }
}
