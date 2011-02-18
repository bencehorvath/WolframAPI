using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

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
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {      
            var client = new WAClient("557QYQ-UUUWTKX95V");
            
            var solution = client.Solve(textBox1.Text);

            MessageBox.Show(solution);
        }
    }
}
