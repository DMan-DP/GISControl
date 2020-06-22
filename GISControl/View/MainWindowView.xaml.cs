using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GISControl.Model;
using GISControl.Model.Correct;
using GISControl.Model.Index;
using GISControl.Model.SynthethisImage;
using GISControl.View;
using GISControl.ViewModel;
using Microsoft.Win32;

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
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Файлы изображений (*.bmp, *.jpg, *.jpeg, *.png, *.tiff)|*.bmp; *.jpg; *.jpeg; *.png; *.tiff|" +
                                "Файлы изображений JP2 (*.jp2, *.j2k, *.jpg2)|*.jp2; *.j2k; *.jpg2 |" +
                                "Все файлы (*.*)|*.*";
            openDialog.Multiselect = true;
            openDialog.Title = "Выберите изображения";

            if (openDialog.ShowDialog() == true)
            {
                for (int i = 0; i < openDialog.FileNames.Length; i++)
                {
                    var fileXt = Path.GetExtension(openDialog.SafeFileNames[i]).ToLower();
                    if (fileXt != ".jp2" && fileXt != ".j2k" && fileXt != ".jpg2" && 
                        fileXt != ".bmp" && fileXt != ".jpg" && fileXt != ".jpeg" && fileXt != ".png" && fileXt != ".tiff")
                    {
                        MessageBox.Show("Данный формат файла не поддерживается", "Ошибка", 
                             MessageBoxButton.OK, MessageBoxImage.Error);
                         return;
                    }
                    LayerManager.instance.AddLayer(Path.GetFileNameWithoutExtension(openDialog.SafeFileNames[i]), openDialog.FileNames[i]);
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
            MainImage.Source = null;
            LayerManager.instance.RemoveAllLayer();
        }

        private async void SizeSelectButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                MainImage.Source = await LayerManager.instance.layers[LayerManager.instance.SelectImage].GetImage();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (OperationCanceledException)
            {
                MainImage = null;
            }
        }

        #region Calculate Index

        private void OpenSynthImageWindow(SynthImage synthImage)
        {
            SynthImageView window = new SynthImageView(synthImage);
            var currenCount = ListBoxLayers.Items.Count;
            window.ShowDialog();
            if (ListBoxLayers.Items.Count != currenCount)
            ListBoxLayers.SelectedIndex = LayerManager.instance.SelectImage;
        }


        private void CalcNDVI(object sender, RoutedEventArgs e)
        {
            OpenSynthImageWindow(new NDVI());
        }

        private void CalcIPVI(object sender, RoutedEventArgs e)
        {
            OpenSynthImageWindow(new IPVI());
        }

        private void CalcSAVI(object sender, RoutedEventArgs e)
        {
            OpenSynthImageWindow(new SAVI());
        }

        private void CalcMSAVI(object sender, RoutedEventArgs e)
        {
            OpenSynthImageWindow(new MSAVI()); ;
        }

        private void CalcGEMI(object sender, RoutedEventArgs e)
        {
            OpenSynthImageWindow(new GEMI());
        }

        private void CalcARVI(object sender, RoutedEventArgs e)
        {
            OpenSynthImageWindow(new ARVI());
        }

        private void CalcRGB(object sender, RoutedEventArgs e)
        {
            OpenSynthImageWindow(new BlendChanel());
        }

        #endregion

        #region Color Correct

        private void OpenImageCorrectWindow(ColorCorrect correct)
        {
            if (LayerManager.instance.SelectImage == -1)
            {
                MessageBox.Show("Необходимо добавить изображение", "Ошибка", MessageBoxButton.OK);
                return;
            }
            ImageCorrectView window = new ImageCorrectView(LayerManager.instance.layers[LayerManager.instance.SelectImage], correct);
            var currenCount = ListBoxLayers.Items.Count;
            window.ShowDialog();
            if (ListBoxLayers.Items.Count != currenCount)
                ListBoxLayers.SelectedIndex = LayerManager.instance.SelectImage;
        }

        private void BrightnessImageCorrect(object sender, RoutedEventArgs e)
        {
            OpenImageCorrectWindow(new BrightnessCorrect());
        }

        private void ContrastImageCorrect(object sender, RoutedEventArgs e)
        {
            OpenImageCorrectWindow(new ContrastCorrect());
        }

        private void GammaImageCorrect(object sender, RoutedEventArgs e)
        {
            OpenImageCorrectWindow(new GammaCorrect());
        }

        private void InvertImageCorrect(object sender, RoutedEventArgs e)
        {
            OpenImageCorrectWindow(new InvertColorCorrect());
        }

        #endregion

        private void SaveImageAs(object sender, RoutedEventArgs e)
        {
            if (LayerManager.instance.SelectImage == -1)
            {
                MessageBox.Show("Отсуствует изображение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); return;
            }
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "TIFF (*.tiff)|*.tiff|" +
                                "PNG (*.png)|*.png|" +
                                "JPG (*.jpg, *.jpeg)|*.jpg; *.jpeg;|" +
                                "BMP (*.bmp)|*.bmp|" +
                                "Все файлы (*.*)|*.*";
            saveDialog.Title = "Сохранить картинку как...";
            saveDialog.FileName = LayerManager.instance.layers[LayerManager.instance.SelectImage].name + LayerManager.instance.layers[LayerManager.instance.SelectImage].data;
            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(LayerManager.instance.layers[LayerManager.instance.SelectImage].image));
                    using (var fileStream = new System.IO.FileStream(saveDialog.FileName, System.IO.FileMode.Create))
                    {
                        encoder.Save(fileStream);
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Невозможно сохранить изображение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
