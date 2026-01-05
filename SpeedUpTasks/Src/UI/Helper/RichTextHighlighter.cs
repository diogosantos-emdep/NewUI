using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Emdep.Geos.UI.Helper
{
   public static class RichTextHighlighter
    {

        public static readonly DependencyProperty SearchTextProperty =
           DependencyProperty.RegisterAttached("SearchText", typeof(string), typeof(RichTextHighlighter), new PropertyMetadata(UpdateSearchText));
        public static string GetSearchText(DependencyObject obj)
        {
            return (string)obj.GetValue(SearchTextProperty);
        }
        public static void SetSearchText(DependencyObject obj, string value)
        {
            obj.SetValue(SearchTextProperty, value);
        }

        static void UpdateSearchText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var richTextBox = d as RichTextBox;
            if (richTextBox != null)
                richTextBox.Update();
        }

        public static void Update(this RichTextBox richTextBox)
        {
            string searchString = GetSearchText(richTextBox);
            IEnumerable<TextRange> wordRanges = GetAllWordRanges(richTextBox.Document);
            if (string.IsNullOrEmpty(searchString))
            {
                foreach (TextRange wordRange in wordRanges)
                    wordRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Transparent);
            }
            else
            {
                foreach (TextRange wordRange in wordRanges)
                {
                    if (wordRange.Text.ToLower() == searchString.ToLower())
                    {
                        wordRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
                    }
                    else
                    {
                        wordRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Transparent);
                    }
                }
            }
        }

        static IEnumerable<TextRange> GetAllWordRanges(FlowDocument document)
        {
            string pattern = @"[^\W\d](\w|[-']{1,2}(?=\w))*";
            TextPointer pointer = document.ContentStart;
            while (pointer != null)
            {
                if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = pointer.GetTextInRun(LogicalDirection.Forward);
                    MatchCollection matches = Regex.Matches(textRun, pattern);
                    foreach (Match match in matches)
                    {
                        int startIndex = match.Index;
                        int length = match.Length;
                        TextPointer start = pointer.GetPositionAtOffset(startIndex);
                        TextPointer end = start.GetPositionAtOffset(length);
                        yield return new TextRange(start, end);
                    }
                }
                pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            }
        }
    }
}
