using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace BlackList.Ui.Wpf.Common.Controls
{
    [TemplatePart(Name = "PART_TextBoxValue", Type = typeof(TextBox))]
    public class StringDataControl : BaseDataControl
    {
        public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent("TextChanged",
            RoutingStrategy.Direct, typeof(TextChangedEventHandler), typeof(StringDataControl));

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(StringDataControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        // Using a DependencyProperty as the backing store for IsEditable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(StringDataControl),
                new PropertyMetadata(false));

        // Using a DependencyProperty as the backing store for TextChangedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextChangedCommandProperty =
            DependencyProperty.Register("TextChangedCommand", typeof(ICommand), typeof(StringDataControl),
                new PropertyMetadata(null));



        public int TextLenght
        {
            get { return (int)GetValue(TextLenghtProperty); }
            set { SetValue(TextLenghtProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextLenght.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextLenghtProperty =
            DependencyProperty.Register("TextLenght", typeof(int), typeof(StringDataControl), new PropertyMetadata(0));




        private TextBox _valueTextBox;


        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }


        public ICommand TextChangedCommand
        {
            get => (ICommand)GetValue(TextChangedCommandProperty);
            set => SetValue(TextChangedCommandProperty, value);
        }


        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }


        public event TextChangedEventHandler TextChanged
        {
            add => AddHandler(TextChangedEvent, value);
            remove => RemoveHandler(TextChangedEvent, value);
        }


        private void RaiseValueChangingEvent(TextChangedEventArgs e)
        {
            RaiseEvent(e);
        }


        public override void OnApplyTemplate()
        {
            InitializeControls();
            base.OnApplyTemplate();
        }


        private void InitializeControls()
        {
            _valueTextBox = GetTemplateChild("PART_TextBoxValue") as TextBox;

            if (_valueTextBox != null)
            {
                var textBinding = new Binding
                {
                    Path = new PropertyPath(nameof(Text)),
                    Source = this,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };

                _valueTextBox.SetBinding(TextBox.TextProperty, textBinding);
            }
        }
    }

}
