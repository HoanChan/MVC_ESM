using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace WindowsFormsExam
{
    class Intro
    {
        public string MainText;
        public string SubText;
        public FontFamily MainFontFamily;
        public FontFamily SubFontFamily;
        public Rectangle ContainerRectangle;
        public float MainFontSize;
        public float SubFontSize;
        public FontStyle MainFontStyle;
        public FontStyle SubFontStyle;

        public void DrawIt(Graphics e)
        {
            Font MainFont = new Font(MainFontFamily, MainFontSize, MainFontStyle);
            Font SubFont = new Font(SubFontFamily, SubFontSize, SubFontStyle);
            RectangleF MainRect = new RectangleF(ContainerRectangle.X, ContainerRectangle.Y, ContainerRectangle.Width, ContainerRectangle.Height - e.MeasureString(MainText,MainFont).Height/2);
            RectangleF SubRect = new RectangleF(ContainerRectangle.X, ContainerRectangle.Y, ContainerRectangle.Width, ContainerRectangle.Height + e.MeasureString(MainText, MainFont).Height / 2);
            
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            e.SmoothingMode = SmoothingMode.AntiAlias;
            e.TextRenderingHint = TextRenderingHint.AntiAlias;

            e.DrawString(MainText,MainFont,new SolidBrush(Color.Red),MainRect, stringFormat);
            e.DrawString(SubText, SubFont, new SolidBrush(Color.Blue), SubRect, stringFormat);
        }
    }
}
