using Microsoft.Win32;
using MoonPdfLib;
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
using System.Windows.Shapes;

namespace CalligraphyAssistantMain.Controls
{
    public partial class PDFFrom : Window
    {
        public string FilePath { get; set; }


        private bool _isLoaded = false;
        public PDFFrom(string parameter)
        {
            InitializeComponent();
            FilePath = parameter;
            this.Loaded += new RoutedEventHandler(Window_Loaded);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                moonPdfPanel.OpenFile(FilePath);
                moonPdfPanel.Zoom(1);
                _isLoaded = true;
            }
            catch (Exception ex)
            {
                _isLoaded = false;
            }
        }

        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                moonPdfPanel.ZoomIn();
            }
        }

        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                moonPdfPanel.ZoomOut();
            }
        }

        private void NormalButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                moonPdfPanel.Zoom(1.0);
            }
        }

        private void TwoNormalButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                moonPdfPanel.Zoom(2.0);
            }
        }

        private void FitToHeightButton_Click(object sender, RoutedEventArgs e)
        {
            moonPdfPanel.ZoomToHeight();
        }

        private void FacingButton_Click(object sender, RoutedEventArgs e)
        {
            moonPdfPanel.ViewType = MoonPdfLib.ViewType.Facing;
        }

        private void SinglePageButton_Click(object sender, RoutedEventArgs e)
        {
            moonPdfPanel.ViewType = MoonPdfLib.ViewType.SinglePage;
        }
    }
}
