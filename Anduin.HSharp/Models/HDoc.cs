using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Anduin.HSharp.Models
{
    public class HDoc : IEnumerable<HTag>
    {
        private readonly List<HTag> _tempList = new List<HTag>();
        private void GetAll(HTag tag)
        {
            _tempList.Add(tag);
            tag.Children.ForEach(GetAll);
        }
        protected List<HTag> _MatchTag(string html)
        {
            var reg = new Regex(Values.HtmlMatch + "|" + Values.NoneHtmlMatch, RegexOptions.Compiled);
            var returnList = new List<HTag>();
            foreach (Match partMatch in reg.Matches(html))
            {
                var tc = partMatch.Groups["TC"].Value;
                var tn = partMatch.Groups["TN"].Value;
                var tp = partMatch.Groups["TP"].Value;
                var rs = partMatch.Groups["RS"].Value;
                var ts = partMatch.Groups["TS"].Value;
                //TextTag
                if (string.IsNullOrEmpty(tn))
                {
                    var element = new HTextTag(rs);
                    returnList.Add(element);
                }
                //Tag
                else if (string.IsNullOrEmpty(ts))
                {
                    var properties = new List<HProp>();
                    foreach (Match propertyMatch in Regex.Matches(tp, Values.PropertiesMatch))
                    {
                        var key = propertyMatch.Groups[1].Value;
                        var value = propertyMatch.Groups[2].Value;
                        properties.Add(new HProp(key, value));
                    }
                    var element = new HTag(tn, properties) { Children = _MatchTag(tc) };
                    element.Children.ForEach(t => t.Parent = element);
                    returnList.Add(element);
                }
            }
            return returnList;
        }

        public List<HTag> Children { get; set; } = new List<HTag>();
        public HTag Son => Children[0];
        public List<HTag> AllUnder
        {
            get
            {
                _tempList.Clear();
                Children.ForEach(GetAll);
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
        public HDoc(string sourceHtml)
        {
            Children = _MatchTag(sourceHtml);
        }

        public HDoc(DocumentOptions options)
        {
            if (options == DocumentOptions.BasicHtml)
            {
                var html = new HTag("html",
                  new HTag("head",
                      new HTag("meta", new HProp("charset", "utf-8")),
                      new HTag("title", "Example")),
                  new HTag("body"));
                Children.Add(html);
            }
        }

        public void Clear()
        {
            Children.Clear();
        }

        public virtual string GenerateHtml()
        {
            return string.Join("\r\n", Children.Select(t => t.GenerateHtml()));
        }
        //Add
        public void AddChild(HTag tag)
        {
            Children.Add(tag);
        }
        public void AddChild(string tagName)
        {
            AddChild(new HTag(tagName));
        }
        public void AddChild(string tagName, string innerContent)
        {
            AddChild(new HTag(tagName, new HTextTag(innerContent)));
        }
        public void AddChild(string tagName, params HTag[] children)
        {
            AddChild(new HTag(tagName, children));
        }
        public void AddChild(string tagName, IEnumerable<HTag> children)
        {
            AddChild(new HTag(tagName, children));
        }
        public void AddChild(string tagName, params HProp[] properties)
        {
            AddChild(new HTag(tagName, properties));
        }
        public void AddChild(string tagName, IEnumerable<HProp> properties)
        {
            AddChild(new HTag(tagName, properties));
        }
        public void AddChildren(params HTag[] children)
        {
            foreach (var child in children)
            {
                AddChild(child);
            }
        }
        public void AddChildren(IEnumerable<HTag> children)
        {
            children.ToList().ForEach(t => AddChild(t));
        }
        //Find
        public HTag FindTagById(string id)
        {
            return AllUnder.Find(t => t.Id == id);
        }
        public HTag FindTagByName(string name)
        {
            return AllUnder.Find(t => t.Name == name);
        }
        public HTag FindTagByTagName(string tagName)
        {
            return AllUnder.Find(t => t.TagName == tagName);
        }
        public List<HTag> SelectByTagName(string tagName)
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
