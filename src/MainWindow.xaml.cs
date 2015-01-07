using System;
using System.Collections.Generic;
using System.Windows;
using DotLiquid;

namespace template_designer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private dynamic _data;

        public MainWindow()
        {
            InitializeComponent();
            GenerateTestData();
            TemplateTextEditor.TextChanged += (sender, args) => RenderTemplate(TemplateTextEditor.Text);
            TemplateTextEditor.Text = Properties.Resources.Digest;
            TemplateTextEditor.WordWrap = true;
            TemplateTextEditor.Options.HighlightCurrentLine = true;
            TemplateTextEditor.Options.IndentationSize = 2;
        }

        private void GenerateTestData()
        {
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
        }

        private void RenderTemplate(string text)
        {
            try
            {
                var template = DotLiquid.Template.Parse(text);
                var rendered = template.Render(Hash.FromAnonymousObject(_data));
                PreviewWindow.NavigateToString(rendered);
            }
            catch
            {
            }
        }
    }
}
