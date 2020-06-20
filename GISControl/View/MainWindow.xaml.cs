using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GISControl.ViewModel;

namespace GISControl
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ListBoxLayers.ItemsSource = LayerManager.instance.layers;
        }

        #region ImageScrolPreview

        private Point scrollMousePoint = new Point();
        private double horizontalOffset = 1;
        private double verticalOffset = 1;


        private void ImageWheelScrol(object sender, MouseWheelEventArgs e)
        {
            if (MainImage.IsMouseOver)
            {
                System.Windows.Point mouseAtImage = e.GetPosition(MainImage);
                System.Windows.Point mouseAtScrollViewer = e.GetPosition(ScrollViewer);

                ScaleTransform st = ViewBox.LayoutTransform as ScaleTransform;
                if (st == null)
                {
                    st = new ScaleTransform();
                    ViewBox.LayoutTransform = st;
                }

                if (e.Delta > 0)
                {
                    st.ScaleX = st.ScaleY = st.ScaleX * 1.25;
                    if (st.ScaleX > 256) st.ScaleX = st.ScaleY = 256;
                }
                else
                {
                    st.ScaleX = st.ScaleY = st.ScaleX / 1.25;
                    if (st.ScaleX < 1) st.ScaleX = st.ScaleY = 1;
                }
                #region [this step is critical for offset]
                ScrollViewer.ScrollToHorizontalOffset(0);
                ScrollViewer.ScrollToVerticalOffset(0);
                this.UpdateLayout();
                #endregion

                Vector offset = MainImage.TranslatePoint(mouseAtImage, ScrollViewer) - mouseAtScrollViewer;
                ScrollViewer.ScrollToHorizontalOffset(offset.X);
                ScrollViewer.ScrollToVerticalOffset(offset.Y);
                this.UpdateLayout();

                e.Handled = true;
            }
        }

        private void NavigatorMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            scrollMousePoint = e.GetPosition(ScrollViewer);
            horizontalOffset = ScrollViewer.HorizontalOffset;
            verticalOffset = ScrollViewer.VerticalOffset;
            Cursor = Cursors.ScrollAll;
        }

        private void NavigatorMouseMove(object sender, MouseEventArgs e)
        {
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                Point position = e.GetPosition(this);

                if (Math.Abs(position.X - scrollMousePoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - scrollMousePoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    ScrollViewer.ScrollToHorizontalOffset(horizontalOffset + (scrollMousePoint.X - e.GetPosition(ScrollViewer).X));
                    ScrollViewer.ScrollToVerticalOffset(verticalOffset + (scrollMousePoint.Y - e.GetPosition(ScrollViewer).Y));
                }
            }
        }

        private void NavigarorLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        #endregion

        private void AddImageButtonClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();
            openDialog.Filter = "Image files (*.BMP, *.JPG, *.PNG, *.JPEG)|*.bmp;*.jpg; *.png; *.jpeg";
            openDialog.Multiselect = true;

            if (openDialog.ShowDialog() == true)
            {
                for (int i = 0; i < openDialog.FileNames.Length; i++)
                {
                    LayerManager.instance.AddLayer(openDialog.SafeFileNames[i], openDialog.FileNames[i]);
                }
                ListBoxLayers.SelectedIndex = LayerManager.instance.SelectImage;
            }
        }

        private void DeleteImageButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ListBoxLayers.SelectedIndex = LayerManager.instance.RemoveLayer(button);
        }

        private void SelectLayer(object sender, SelectionChangedEventArgs e)
        {
            LayerManager.instance.SelectLayer(sender);
        }

        private void RemoveAllImageButtonClick(object sender, RoutedEventArgs e)
        {
            MainImage.Source = LayerManager.instance.layers[LayerManager.instance.SelectImage].image;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
