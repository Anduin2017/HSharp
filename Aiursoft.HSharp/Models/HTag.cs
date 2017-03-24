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

        public HTag(string TagName)
        {
            this.TagName = TagName;
        }
        public HTag(string TagName, string InnerContent) : this(TagName)
        {
            AddChild(new HTextTag(InnerContent));
        }
        public HTag(string TagName, params HTag[] Children) : this(TagName)
        {
            AddChildren(Children);
        }
        public HTag(string TagName, IEnumerable<HTag> Children)
        {
            AddChildren(Children);
        }
        public HTag(string TagName, params HProp[] Properties) : this(TagName)
        {
            AddProperties(Properties);
        }
        public HTag(string TagName, IEnumerable<HProp> Properties) : this(TagName)
        {
            AddProperties(Properties);
        }

        public void AddProperties(params HProp[] Properties)
        {
            foreach (var property in Properties)
            {
                this.Properties.Add(property.Key, property.Value);
            }
        }
        public void AddProperties(IEnumerable<HProp> Properties)
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
                Children.ForEach(t => AllChildren += $"{t.GenerateHTML()}");
                return $"<{TagName + PropertiesResult}>\r\n{AllChildren}</{TagName}>\r\n";
            }
        }
        public override string ToString() => TagName;
        public override Dictionary<string, dynamic> DynamicData()
        {
            var Base = base.DynamicData();
            Base.Add("Properties",Properties);
            return Base;
        }
    }
}
