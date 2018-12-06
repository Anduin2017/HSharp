using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aiursoft.HSharp.Models
{
    public class HDoc : IEnumerable<HTag>
    {
        private List<HTag> _tempList = new List<HTag>();
        private void _GetAll(HTag tag)
        {
            _tempList.Add(tag);
            tag.Children.ForEach(t => _GetAll(t));
        }
        protected List<HTag> _MatchTag(string html)
        {
            var ReturnList = new List<HTag>();
            foreach (var Part in Regex.Matches(html, Values.HTMLMatch + "|" + Values.NoneHTMLMatch))
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
                _tempList.Clear();
                Children.ForEach(t => _GetAll(t));
                return _tempList.ToList();
            }
        }
        public HTag this[string tagName] => Children.Find(t => t.TagName == tagName);
        public HTag this[int tagIndex] => Children[tagIndex];
        public HTag this[string tagName, int index] => Children.Where(t => t.TagName == tagName).ToList()[index];
        //Construction
        public HDoc()
        {

        }
        public HDoc(string sourceHTML)
        {
            Children = _MatchTag(sourceHTML);
        }
        // public HDoc(Uri Url)
        // {
        //     var Result = HTTPService.Get(Url.AbsoluteUri);
        //     Children = _MatchTag(Result);
        // }
        public HDoc(DocumentOptions options)
        {
            if (options == DocumentOptions.BasicHTML)
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
        public virtual void AddChild(HTag tag)
        {
            Children.Add(tag);
        }
        public virtual void AddChild(string tagName)
        {
            AddChild(new HTag(tagName));
        }
        public virtual void AddChild(string tagName, string innerContent)
        {
            AddChild(new HTag(tagName, new HTextTag(innerContent)));
        }
        public virtual void AddChild(string tagName, params HTag[] children)
        {
            AddChild(new HTag(tagName, children));
        }
        public virtual void AddChild(string tagName, IEnumerable<HTag> children)
        {
            AddChild(new HTag(tagName, children));
        }
        public virtual void AddChild(string tagName, params HProp[] properties)
        {
            AddChild(new HTag(tagName, properties));
        }
        public virtual void AddChild(string tagName, IEnumerable<HProp> properties)
        {
            AddChild(new HTag(tagName, properties));
        }
        public virtual void AddChildren(params HTag[] children)
        {
            foreach (var Child in children)
            {
                AddChild(Child);
            }
        }
        public virtual void AddChildren(IEnumerable<HTag> children)
        {
            children.ToList().ForEach(t => AddChild(t));
        }
        //Find
        public virtual HTag FindTagById(string id)
        {
            return AllUnder.Find(t => t.Id == id);
        }
        public virtual HTag FindTagByName(string name)
        {
            return AllUnder.Find(t => t.Name == name);
        }
        public virtual HTag FindTagByTagName(string tagName)
        {
            return AllUnder.Find(t => t.TagName == tagName);
        }
        public virtual List<HTag> SelectByTagName(string tagName)
        {
            return Children.Where(t => t.TagName == tagName).ToList();
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
