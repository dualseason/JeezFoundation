﻿using System.Dynamic;
using System.Xml.Linq;

namespace JeezFoundation.Core.Domain
{
    /// <summary>
    /// 动态XML类型
    /// </summary>
    public class DynamicXml : DynamicObject
    {
        XElement _root;
        private DynamicXml(XElement root)
        {
            _root = root;
        }

        public static DynamicXml Parse(string xmlString)
        {
            return new DynamicXml(XDocument.Parse(xmlString).Root);
        }

        public static DynamicXml Load(string filename)
        {
            return new DynamicXml(XDocument.Load(filename).Root);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            var att = _root.Attribute(binder.Name);
            if (att != null)
            {
                result = att.Value;
                return true;
            }

            var nodes = _root.Elements(binder.Name);
            if (nodes.Count() > 1)
            {
                result = nodes.Select(n => n.HasElements ? (object)new DynamicXml(n) : n.Value).ToList();
                return true;
            }

            var node = _root.Element(binder.Name);
            if (node != null)
            {
                result = node.HasElements ? (object)new DynamicXml(node) : node.Value;
                return true;
            }

            return true;
        }
    }
}