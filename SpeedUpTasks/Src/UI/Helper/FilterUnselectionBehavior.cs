using System.Windows;
using System.Windows.Data;

using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Navigation;
using DevExpress.Data.Filtering;
using DevExpress.Mvvm.POCO;

namespace Emdep.Geos.UI.Helper
{
    public class FilterUnselectionBehavior : Behavior<TileBar>
    {
        bool selectFilterEnable = true;

        public static readonly DependencyProperty SelectedFilterProperty =
            DependencyProperty.Register("SelectedFilter", typeof(FilterItem), typeof(FilterUnselectionBehavior),
                new PropertyMetadata(null, (d, e) => ((FilterUnselectionBehavior)d).OnSelectedFilterChanged()));
        static readonly DependencyProperty TileBarItemInternalProperty =
            DependencyProperty.Register("TilebarItemInternal", typeof(FilterItem), typeof(FilterUnselectionBehavior),
                new PropertyMetadata(null, (d, e) => ((FilterUnselectionBehavior)d).OnTileBarItemInternalChanged()));

        public FilterItem SelectedFilter
        {
            get { return (FilterItem)GetValue(SelectedFilterProperty); }
            set { SetValue(SelectedFilterProperty, value); }
        }
        FilterItem TileBarItemInternal
        {
            get { return (FilterItem)GetValue(TileBarItemInternalProperty); }
            set { SetValue(TileBarItemInternalProperty, value); }
        }

        void OnSelectedFilterChanged()
        {
            if (AssociatedObject == null || AssociatedObject.ItemsSource == null || SelectedFilter == TileBarItemInternal) return;
            if (SelectedFilter == null)
            {
                SelectTileBarItem(null);
                return;
            }
            foreach (var item in AssociatedObject.ItemsSource)
                if (item == SelectedFilter)
                {
                    SelectTileBarItem(SelectedFilter);
                    return;
                }
            SelectTileBarItem(null);
        }
        void OnTileBarItemInternalChanged()
        {
            if (selectFilterEnable)
                SelectedFilter = TileBarItemInternal;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            BindingOperations.SetBinding(this, FilterUnselectionBehavior.TileBarItemInternalProperty, new Binding("SelectedItem") { Source = AssociatedObject, Mode = BindingMode.OneWay });
            OnSelectedFilterChanged();
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            BindingOperations.ClearBinding(this, FilterUnselectionBehavior.TileBarItemInternalProperty);
        }

        void SelectTileBarItem(FilterItem item)
        {
            selectFilterEnable = false;
            AssociatedObject.SelectedItem = item;
            selectFilterEnable = true;
        }
    }
    public class FilterItem
    {
        public static FilterItem Create(int entitiesCount, string name, CriteriaOperator filterCriteria, string imageUri)
        {
            return ViewModelSource.Create(() => new FilterItem(entitiesCount, name, filterCriteria, imageUri));
        }

        protected FilterItem(int entitiesCount, string name, CriteriaOperator filterCriteria, string imageUri)
        {
            this.Name = name;
            this.FilterCriteria = filterCriteria;
            this.ImageUri = imageUri;
            Update(entitiesCount);
        }

        public virtual string Name { get; set; }

        public virtual CriteriaOperator FilterCriteria { get; set; }

        public virtual int EntitiesCount { get; protected set; }

        public virtual string DisplayText { get; protected set; }

        public virtual string ImageUri { get; protected set; }

        public void Update(int entitiesCount)
        {
            this.EntitiesCount = entitiesCount;
            DisplayText = string.Format("{0} ({1})", Name, entitiesCount);
        }

        public FilterItem Clone()
        {
            return FilterItem.Create(EntitiesCount, Name, FilterCriteria, ImageUri);
        }
        public FilterItem Clone(string name, string imageUri)
        {
            return FilterItem.Create(EntitiesCount, name, FilterCriteria, imageUri);
        }

        protected virtual void OnNameChanged()
        {
            Update(EntitiesCount);
        }
    }
}
