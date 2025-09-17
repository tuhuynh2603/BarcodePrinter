using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using BarcodePrintLabel.ViewModels;

namespace BarcodePrintLabel.Views
{
    public class LabelTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UnitTemplate { get; set; }
        public DataTemplate BoxTemplate { get; set; }
        public DataTemplate CartonTemplate { get; set; }
        public DataTemplate FullTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is LabelPreviewDialogViewModel vm)
            {
                return vm.SelectedLabelType switch
                {
                    "Unit" => UnitTemplate,
                    "Box" => BoxTemplate,
                    "Carton" => CartonTemplate,
                    "Full" => FullTemplate,
                    _ => base.SelectTemplate(item, container),
                };
            }
            return base.SelectTemplate(item, container);
        }
    }
}
