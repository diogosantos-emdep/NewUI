using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using DevExpress.Mvvm.DataAnnotations;
using System.Windows.Media;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraRichEdit.API.Layout;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit;

namespace Emdep.Geos.UI.Helper
{
    public class RichEditControlSearchHelper : Behavior<RichEditControl>
    {
        public static readonly DependencyProperty SearchTextProperty =
           DependencyProperty.Register("SearchText", typeof(string), typeof(RichEditControlSearchHelper), new PropertyMetadata(OnSearchTextPropertyChanged));
        static void OnSearchTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RichEditControlSearchHelper)d).OnSearchTextChanged(e);
        }

        List<DocumentRange> searchResultsList = new List<DocumentRange>();

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }
        protected RichEditControl Editor { get { return AssociatedObject; } }

        protected override void OnAttached()
        {
            base.OnAttached();
            Editor.BeforePagePaint += richEditControl1_BeforePagePaint;
            Editor.ContentChanged += OnContentChanged;
            UpdateHighlighting();
        }
        protected override void OnDetaching()
        {
            Editor.ContentChanged -= OnContentChanged;
            Editor.BeforePagePaint -= richEditControl1_BeforePagePaint;
            base.OnDetaching();
        }
        protected virtual void OnContentChanged(object sender, EventArgs e)
        {
            UpdateHighlighting();
        }
        protected virtual void OnSearchTextChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateHighlighting();
        }
        protected virtual void UpdateHighlighting()
        {
            if (Editor == null)
                return;
            if (string.IsNullOrEmpty(SearchText))
                searchResultsList.Clear();
            else
                searchResultsList = Editor.Document.FindAll(SearchText, SearchOptions.None).ToList();
            Editor.Refresh();
        }
        PlainTextBox GetElement(DocumentPosition pos)
        {
            return Editor.DocumentLayout.GetElement<PlainTextBox>(pos);
        }
        void richEditControl1_BeforePagePaint(object sender, BeforePagePaintEventArgs e)
        {
            if (e.CanvasOwnerType == CanvasOwnerType.Printer)
                return;
            CustomDrawPagePainter customPagePainter = new CustomDrawPagePainter(Editor, searchResultsList);
            e.Painter = customPagePainter;
        }

    }

    public class CustomDrawPagePainter : PagePainter
    {
        List<FixedRange> searchResulstList;
        List<FixedRange> visibleSearchResultsList;
        RichEditControl richEditControl;

        public CustomDrawPagePainter(RichEditControl richEdit, List<DocumentRange> resultList)
            : base()
        {
            richEditControl = richEdit;
            searchResulstList = resultList.ConvertAll(x => new FixedRange(x.Start.ToInt(), x.Length));
        }
        public override void DrawPage(LayoutPage page)
        {
            visibleSearchResultsList = searchResulstList.FindAll(x => page.MainContentRange.Intersect(x));
            base.DrawPage(page);
        }
        public override void DrawPlainTextBox(PlainTextBox plainTextBox)
        {
            foreach (FixedRange range in visibleSearchResultsList)
                if (range.Contains(plainTextBox.Range))
                    HighlightElement(plainTextBox.Bounds);
            base.DrawPlainTextBox(plainTextBox);
        }
        void HighlightElement(System.Drawing.Rectangle bounds)
        {
            this.Canvas.FillRectangle(new RichEditBrush(Colors.Yellow), bounds);
        }
    }
}
