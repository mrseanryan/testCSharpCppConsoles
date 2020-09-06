using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dotnetWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            getReviewsButton.Click += OnClickGetReviews;
        }

        // async *void* is only OK with event handler (else would causes issues, e.g. exception handling)
        private async void OnClickGetReviews(object sender, RoutedEventArgs e)
        {
            DumpSeparator();

            var cmd = new GetBookReviewsCommandAsync();

            await GetReady();

            var reviews = await cmd.ExecuteAsync();

            Dump(SummariseReviews(reviews));
            Dump("[done]");
        }

        private void DumpSeparator()
        {
            Dump("-------");
        }

        private void Dump(string text)
        {
            outputTextBox.Text += text + Environment.NewLine;
        }

        private async Task GetReady()
        {
            Dump("Preparing...");
            await Task.Delay(2000);
            Dump("Ready.");
        }

        private string SummariseReviews(IEnumerable<string> reviews)
        {
            return string.Join(Environment.NewLine, reviews);
        }
    }
}
