using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spire.Doc;

namespace BKT_KDCLGD_2020.DocumentCommon
{
    public class DocHeading
    {
        private Document doc_;

        private static int countNameStyle_ = 0;

        private int indexSection_ = 1;

        public DocHeading(Document doc)
        {
            doc_ = doc;
        }

        public DocHeading(Document doc, int indexSection)
        {
            doc_ = doc;

            indexSection_ = indexSection;
        }

        public void AddHeading(string heading, BuiltinStyle builtinStyle, float fontSize)
        {
            var paragraph = doc_.Sections[indexSection_].AddParagraph();
            //paragraph.AppendText(heading);
            paragraph.ApplyStyle(builtinStyle);
            //paragraph.Format.LeftIndent = 10;
            paragraph.Format.HorizontalAlignment = HorizontalAlignment.Center;

            var TR = paragraph.AppendText(heading);
            TR.CharacterFormat.FontName = "Times New Roman";
            TR.CharacterFormat.FontSize = fontSize;
            TR.CharacterFormat.Bold = true;
            TR.CharacterFormat.Italic = false;
            TR.CharacterFormat.UnderlineStyle = false == true ? UnderlineStyle.Single : UnderlineStyle.None;

            paragraph.Format.AfterSpacing = 6;
            paragraph.Format.BeforeSpacing = 6;
        }

        public void AddHeading(string heading, BuiltinStyle builtinStyle, string nameListStyle, float fontSize)
        {
            var paragraph = doc_.Sections[indexSection_].AddParagraph();
            //paragraph.Format.LeftIndent = 0;
            paragraph.ApplyStyle(builtinStyle);
            //paragraph.ListFormat.ApplyNumberedStyle();
            paragraph.ListFormat.ApplyStyle(nameListStyle);

            var TR = paragraph.AppendText(heading);
            TR.CharacterFormat.FontName = "Times New Roman";
            TR.CharacterFormat.FontSize = fontSize;
            TR.CharacterFormat.Bold = true;
            TR.CharacterFormat.Italic = false;
            TR.CharacterFormat.UnderlineStyle = false == true ? UnderlineStyle.Single : UnderlineStyle.None;

            paragraph.Format.AfterSpacing = 6;
            paragraph.Format.BeforeSpacing = 6;
        }

        public string CreateNodeHeading(ListType listType, bool usePrevLevelPattern, string numberPrefix)
        {
            ListStyle listStyle = new ListStyle(doc_, listType);
            foreach (ListLevel listLev in listStyle.Levels)
            {
                listLev.UsePrevLevelPattern = usePrevLevelPattern;
                listLev.NumberPrefix = numberPrefix;
            }

            listStyle.Name = string.Format("MyStyle{0}", countNameStyle_);
            countNameStyle_++;

            doc_.ListStyles.Add(listStyle);

            return listStyle.Name;
        }
    }
}
