using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DotLiquid;
using template_designer.Properties;

namespace template_designer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModel _viewModel;
        private dynamic _data;


        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ViewModel();
            DataContext = _viewModel;
            var notification1 = new
            {
                type = "Answer",
                date = DateTime.Now,
                title = "Alignment of the Series-H Oil process pumps",
                highlight =
                    "Here is the answer short test of about xxx characters. Here is the answer short test of about xxx characters. Here is the answer short test of about xxx characters. Here is the answer short test of about . Here is the answer short test of about xxx characters",
                author = new
                {
                    fullName = "Marc Vermeulen",
                    link = "https=//mondale.n365.com/people"
                }
            };
            var notification2 = new
            {
                type = "Comment",
                date = DateTime.Now,
                title = "Alignment of the Series-H Oil process pumps",
                highlight =
                    "Here is the answer short test of about xxx characters. Here is the answer short test of about xxx characters. Here is the answer short test of about xxx characters. Here is the answer short test of about . Here is the answer short test of about xxx characters",
                author = new
                {
                    fullName = "Marc Vermeulen",
                    link = "https=//mondale.n365.com/people"
                }
            };
            var notifications = new List<dynamic>
            {
                notification1,
                notification2
            }.ToArray();
            _data = new
            {
                tenant = new
                {
                    name = "microsoft"
                },
                notifications = notifications
            };

            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals("EmailTemplate"))
                {
                    RenderTemplate();
                }
            };
            RenderTemplate();
        }

        private void RenderTemplate()
        {
            try
            {
                var template = DotLiquid.Template.Parse(_viewModel.EmailTemplate);
                var rendered = template.Render(Hash.FromAnonymousObject(_data));
                PreviewWindow.NavigateToString(rendered);
            }
            catch
            {
            }
        }
    }

    public class ViewModel: INotifyPropertyChanged
    {

        private string _emailTemplate= Resources.Digest;

        public string EmailTemplate
        {
            get { return _emailTemplate; }
            set
            {
                _emailTemplate = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
