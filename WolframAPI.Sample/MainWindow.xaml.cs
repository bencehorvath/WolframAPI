using System;
using System.Windows;
using System.Windows.Threading;

namespace WolframAPI.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            libVersion.Content = WAClient.Version;
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            var client = new WAClient("557QYQ-UUUWTKX95V");

            var expr = textBox1.Text;
            //var solution = client.Solve(textBox1.Text);

            //MessageBox.Show(solution);

            client.OnSolutionReceived += (solution, expression) =>
                                             {
                                                 MessageBox.Show(solution);

                                                 Dispatcher.BeginInvoke(DispatcherPriority.Send,
                                                                        new Action(() => textBox1.Text = expression));
                                             };

            client.SolveAsync(expr);

            textBox1.Text = string.Format("Solving: {0}", expr);
        }
    }
}
