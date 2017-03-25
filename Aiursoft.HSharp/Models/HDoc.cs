using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aiursoft.HSharp.Models
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
                var TS = PartMatch.Groups["TS"].Value;
                //TextTag
                if (string.IsNullOrEmpty(TN))
                {
                    var Element = new HTextTag(RS);
                    ReturnList.Add(Element);
                }
                //Tag
                else if (string.IsNullOrEmpty(TS))
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
                return _TempList.ToList();
            }
        }
        public HTag this[string TagName] => Children.Find(t => t.TagName == TagName);
        public HTag this[int TagIndex] => Children[TagIndex];
        public HTag this[string TagName, int Index] => Children.Where(t => t.TagName == TagName).ToList()[Index];
        //Construction
        public HDoc()
        {

        }
        public HDoc(string SourceHTML)
        {
            Children = _MatchTag(SourceHTML);
        }
        // public HDoc(Uri Url)
        // {
        //     var Result = HTTPService.Get(Url.AbsoluteUri);
        //     Children = _MatchTag(Result);
        // }
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
        //Fuctions
        public virtual void Clear()
        {
            AllUnder.ForEach(t => t = null);
            Children.Clear();
        }
        public virtual string GenerateHTML()
        {
            string Result = string.Empty;
            Children.ForEach(t => Result += t.GenerateHTML() + "\r\n");
            return Result;
        }
        //Add
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
        //Find
        public virtual HTag FindTagById(string Id)
        {
            return AllUnder.Find(t => t.Id == Id);
        }
        public virtual HTag FindTagByName(string Name)
        {
            return AllUnder.Find(t => t.Name == Name);
        }
        public virtual HTag FindTagByTagName(string TagName)
        {
            return AllUnder.Find(t => t.TagName == TagName);
        }
        public virtual List<HTag> SelectByTagName(string TagName)
        {
            return Children.Where(t => t.TagName == TagName).ToList();
        }
        //IEnumerator Service
        public IEnumerator<HTag> GetEnumerator()
        {
            return ((IEnumerable<HTag>)Children).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<HTag>)Children).GetEnumerator();
        }
        //Dynamic Service
        //public virtual Dictionary<string, dynamic> DynamicData()
        //{
        //    Dictionary<string, dynamic> _Data = new Dictionary<string, dynamic>();
        //    foreach (var Child in Children)
        //    {
        //        if (!_Data.ContainsKey(Child.ToString()))
        //            _Data.Add(Child.ToString(), Child.Root);
        //    }
        //    _Data.Add("Children", Children);
        //    return _Data;
        //}
        //public virtual dynamic Root
        //{
        //    get
        //    {
        //        return new DynamicHDictionary(DynamicData);
        //    }
        //}
    }
}
