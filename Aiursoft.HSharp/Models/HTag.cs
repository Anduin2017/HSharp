using System;
using System.Collections.Generic;

namespace Aiursoft.HSharp.Models
{
    public class HTag : HDoc
    {
        public string TagName { get; set; }
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
        public string Id
        {
            get
            {
                try
                {
                    return Properties["id"];
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        public string Name
        {
            get
            {
                try
                {
                    return Properties["name"];
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        public HTag Parent { get; set; }

        public HTag(string tagName)
        {
            this.TagName = tagName;
        }
        public HTag(string tagName, string innerContent) : this(tagName)
        {
            AddChild(new HTextTag(innerContent));
        }
        public HTag(string tagName, params HTag[] children) : this(tagName)
        {
            AddChildren(children);
        }
        public HTag(string tagName, IEnumerable<HTag> children)
        {
            AddChildren(children);
        }
        public HTag(string tagName, params HProp[] properties) : this(tagName)
        {
            AddProperties(properties);
        }
        public HTag(string tagName, IEnumerable<HProp> properties) : this(tagName)
        {
            AddProperties(properties);
        }

        public void AddProperties(params HProp[] properties)
        {
            foreach (var property in properties)
            {
                this.Properties.Add(property.Key, property.Value);
            }
        }
        public void AddProperties(IEnumerable<HProp> properties)
        {
            foreach (var property in properties)
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
                Children.ForEach(t => AllChildren += $"{t.GenerateHTML()}");
                return $"<{TagName + PropertiesResult}>\r\n{AllChildren}</{TagName}>\r\n";
            }
        }
        public override string ToString() => TagName;
        //public override Dictionary<string, dynamic> DynamicData()
        //{
        //    var Base = base.DynamicData();
        //    Base.Add("Properties",Properties);
        //    return Base;
        //}
    }
}
