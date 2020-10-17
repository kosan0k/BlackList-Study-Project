using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace BlackList.Ui.Wpf.Common.Controls
{
    [TemplatePart(Name = CaptionTextBlockTemplateName, Type = typeof(TextBlock))]
    public class BaseDataControl : Control
    {
        private const string CaptionTextBlockTemplateName = "PART_TextBlockCaption";

        public bool IsRequired
        {
            get { return (bool)GetValue(IsRequiredProperty); }
            set { SetValue(IsRequiredProperty, value); }
        }

        public static readonly DependencyProperty IsRequiredProperty =
            DependencyProperty.Register(nameof(IsRequired), typeof(bool), typeof(BaseDataControl));



        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register(nameof(Caption), typeof(string), typeof(BaseDataControl));


        public override void OnApplyTemplate()
        {
            InitializeControls();
            base.OnApplyTemplate();
        }

        private void InitializeControls()
        {
            if (GetTemplateChild(CaptionTextBlockTemplateName) is TextBlock textBlock)
            {
                var textBinding = new Binding
                {
                    Path = new PropertyPath(nameof(Caption)),
                    Source = this
                };

                textBlock.SetBinding(TextBlock.TextProperty, textBinding);
            }
        }
    }

}
