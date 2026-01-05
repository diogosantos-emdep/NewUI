using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Emdep.Geos.UI.Helper
{
    /// <summary>
    ///  <!--[GEOS2-8194][pramod.misal][28.05.2025]-->  https://helpdesk.emdep.com/browse/GEOS2-8194
    /// </summary>
    public class SpiEditBehavior : Behavior<SpinEdit>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            Subscribe();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            Unsubscribe();
        }

        void Subscribe()
        {
            if (AssociatedObject != null)
                AssociatedObject.Spin += OnSpin;
        }

        void Unsubscribe()
        {
            if (AssociatedObject != null)
                AssociatedObject.Spin -= OnSpin;
        }

        void OnSpin(object sender, SpinEventArgs e)
        {
            var edit = sender as SpinEdit;
            if (edit == null || !(edit.EditValue is decimal currentVal))
                return;

            string text = edit.Text;
            int caretIndex = edit.CaretIndex;
            int decimalIndex = text.IndexOf('.');

            bool afterDecimal = decimalIndex >= 0 && caretIndex > decimalIndex;

            decimal newValue = currentVal;

            if (afterDecimal)
            {
                
                if (e.IsSpinUp)
                {
                    newValue += 0.1m;
                }
                else
                {
                    newValue -= 0.1m;
                }

               
                newValue = Math.Round(newValue, 1, MidpointRounding.AwayFromZero);
            }
            else
            {
               
                if (e.IsSpinUp)
                {
                    newValue += 1.0m;
                }
                else
                {
                    newValue -= 1.0m;
                }
            }

            
            e.Handled = true;
            edit.EditValue = newValue;

            
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                if (edit.IsLoaded)
                    edit.CaretIndex = caretIndex;
            }), System.Windows.Threading.DispatcherPriority.Background);
        }


       
    }

}
