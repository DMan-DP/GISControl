using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GISControl.Model;
using GISControl.Model.Correct;
using GISControl.Model.Help;
using GISControl.ViewModel;
using Point = System.Windows.Point;

namespace GISControl.View
{
    public partial class ImageCorrectView : Window
    {
        private ColorCorrect colorCorrect;
        private Layer layer;
        private BitmapImage image = null;

        public ImageCorrectView(Layer layer, ColorCorrect colorCorrect)
        {
            this.layer = layer;
            this.colorCorrect = colorCorrect;
            InitializeComponent();

            if (colorCorrect.GetType() == typeof(BrightnessCorrect))
            {
                this.Title = "Яркость";
                ImageCorrectTitle.Text = "Яркость";
                Slider.Minimum = -255; Slider.Maximum = 255; Slider.Value = 0; Slider.TickFrequency = 1;
            }
            else if (colorCorrect.GetType() == typeof(ContrastCorrect) || colorCorrect.GetType() == typeof(GammaCorrect))
            {
                Slider.Minimum = 0; Slider.Maximum = 5; Slider.Value = 1; Slider.TickFrequency = 0.1;
                if (colorCorrect.GetType() == typeof(ContrastCorrect))
                {
                    this.Title = "Контрастность";
                    ImageCorrectTitle.Text = "Контрастность";
                }
                else
                {
                    this.Title = "Гамма коррекция";
                    ImageCorrectTitle.Text = "Гамма";
                }
            }
            else if (colorCorrect.GetType() == typeof(InvertColorCorrect))
            {
                this.Title = "Инверсия";
                Slider.Visibility = Visibility.Hidden;
                SliderValue.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Произошла внуренняя ошибка", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                this.Close();
            }
            MainImage.Source = layer.image;
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

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (colorCorrect.GetType() == typeof(BrightnessCorrect))
            {
                SliderValue.Text = Convert.ToInt32(Slider.Value).ToString();
            }
            else
            {
                SliderValue.Text = Math.Round(Slider.Value, 1).ToString();
            }
        }

        private async void CalculateButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                CorrectProgressBar.IsIndeterminate = true;
                image = await Controller.CollorCorrectTask(colorCorrect, layer.image, Slider.Value);
                MainImage.Source = image;
            }
            catch (OperationCanceledException)
            {
                image = null;
                MessageBox.Show("Произошла внуренняя ошибка. Код ошибки 1", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            CorrectProgressBar.IsIndeterminate = false;
        }

        private void SaveLayerButtonClick(object sender, RoutedEventArgs e)
        {
            if (image == null)
            {
                MessageBox.Show("Ошибка сохранения файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); return;
            }

            string imageName = layer.name;
            if (colorCorrect.GetType() == typeof(BrightnessCorrect))
            {
                imageName += "-B";
            }
            if (colorCorrect.GetType() == typeof(ContrastCorrect))
            {
                imageName += "-C";
            }
            if (colorCorrect.GetType() == typeof(GammaCorrect))
            {
                imageName += "-G";
            }
            if (colorCorrect.GetType() == typeof(InvertColorCorrect))
            {
                imageName += "-I";
            }

            LayerManager.instance.AddLayer(image, imageName);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            this.Close();
        }

        private void SaveImageButtonClick(object sender, RoutedEventArgs e)
        {
            if (image == null)
            {
                MessageBox.Show("Ошибка сохранения файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); return;
            }
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();

            saveDialog.Filter = "PNG (*.png)|*.png|" +
                                "JPG (*.jpg, *.jpeg)|*.jpg; *.jpeg;|" +
                                "TIFF (*.tiff)|*.tiff|" +
                                "BMP (*.bmp)|*.bmp|" +
                                "Все файлы (*.*)|*.*";
            saveDialog.Title = "Сохранить картинку как...";
            saveDialog.FileName = layer.name + Title;

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
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
