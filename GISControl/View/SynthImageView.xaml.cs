using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GISControl.Model;
using GISControl.Model.Index;
using GISControl.Model.MapValue;
using GISControl.Model.SynthethisImage;
using GISControl.ViewModel;

namespace GISControl.View
{
    public partial class SynthImageView : Window
    {
        private SynthImage synthImage;
        private BitmapImage image = null;
        private Layer[] layers;

        private string[] defaultName = new string[4];

        public SynthImageView(SynthImage synthImage)
        {
            InitializeComponent();
            if (synthImage.GetType() == typeof(NDVI))
            {
                Title = "NDVI";
                layers = new Layer[2] {null, null};
                BandAButton.Content = "NIR";
                BandGButton.Visibility = Visibility.Hidden;
                BandBButton.Visibility = Visibility.Hidden;
                CoeffName.Visibility = Visibility.Hidden;
                CoeffValue.Visibility = Visibility.Hidden;
            }
            else if (synthImage.GetType() == typeof(IPVI))
            {
                Title = "IPVI";
                layers = new Layer[2] { null, null };
                BandAButton.Content = "NIR";
                BandGButton.Visibility = Visibility.Hidden;
                BandBButton.Visibility = Visibility.Hidden;
                CoeffName.Visibility = Visibility.Hidden;
                CoeffValue.Visibility = Visibility.Hidden;
            }
            else if (synthImage.GetType() == typeof(SAVI))
            {
                Title = "SAVI";
                layers = new Layer[2] { null, null };
                BandAButton.Content = "NIR";
                BandGButton.Visibility = Visibility.Hidden;
                BandBButton.Visibility = Visibility.Hidden;
                CoeffName.Text = "L";
            }
            else if (synthImage.GetType() == typeof(MSAVI))
            {
                Title = "MSAVI";
                layers = new Layer[2] { null, null };
                BandAButton.Content = "NIR";
                BandGButton.Visibility = Visibility.Hidden;
                BandBButton.Visibility = Visibility.Hidden;
                CoeffName.Visibility = Visibility.Hidden;
                CoeffValue.Visibility = Visibility.Hidden;
            }
            else if (synthImage.GetType() == typeof(GEMI))
            {
                Title = "GEMI";
                layers = new Layer[2] { null, null };
                BandAButton.Content = "NIR";
                BandGButton.Visibility = Visibility.Hidden;
                BandBButton.Visibility = Visibility.Hidden;
                CoeffName.Visibility = Visibility.Hidden;
                CoeffValue.Visibility = Visibility.Hidden;
            }
            else if (synthImage.GetType() == typeof(ARVI))
            {
                Title = "SAVI";
                layers = new Layer[3] { null, null, null};
                BandAButton.Content = "NIR";
                BandBButton.Visibility = Visibility.Hidden;
                BandGButton.Content = "B";
                BandGButton.Background = new SolidColorBrush(Color.FromRgb(128, 124, 218));
                CoeffName.Text = "a";
            }
            else if (synthImage.GetType() == typeof(BlendChanel))
            {
                Title = "Смешивание каналов";
                layers = new Layer[3] { null, null, null };
                BandBButton.Visibility = Visibility.Hidden;
                CoeffName.Visibility = Visibility.Hidden;
                CoeffValue.Visibility = Visibility.Hidden;
                SaveResButton.Visibility = Visibility.Hidden;
                PaletteComboBox.Visibility = Visibility.Hidden;

                BandAButton.Content = "R";
                BandAButton.Background = new SolidColorBrush(Color.FromRgb(218, 124, 124));
                BandRButton.Content = "G";
                BandRButton.Background = new SolidColorBrush(Color.FromRgb(145, 218, 124));
                BandGButton.Content = "B";
                BandGButton.Background = new SolidColorBrush(Color.FromRgb(128, 124, 218));
                CoeffName.Text = "a";
            }
            else
            {
                MessageBox.Show("Произошла внуренняя ошибка", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                this.Close();
            }

            defaultName[0] = BandAButton.Content.ToString();
            defaultName[1] = BandRButton.Content.ToString();
            defaultName[2] = BandGButton.Content.ToString();
            defaultName[3] = BandBButton.Content.ToString();

            ListBoxLayers.ItemsSource = LayerManager.instance.layers;
            ListBoxLayers.SelectedIndex = LayerManager.instance.SelectImage;
            this.synthImage = synthImage;
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

        private async void CalculateButtonClick(object sender, RoutedEventArgs e)
        {
            // Проерка коэффцициентов
            double coef = 0;
            try
            {
                coef = Convert.ToDouble(CoeffValue.Text.Replace(".", ","));
            }
            catch
            {
                MessageBox.Show("Не правильно введен коэффициент", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (synthImage.GetType() == typeof(SAVI))
            {
                if (coef < 0 || coef > 1)
                {
                    MessageBox.Show("Выберите L = [0; 1]", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else synthImage.SetCoeff(coef);
            }
            if (synthImage.GetType() == typeof(ARVI))
            {
                if (coef < 0 || coef > 1)
                {
                    MessageBox.Show("Выберите a = [0; 1]", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else synthImage.SetCoeff(coef);
            }

            // Проверка на выбор каналов
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i] == null)
                {
                    MessageBox.Show("Необходимо выбрать изображения", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            Palette palette = (Palette)PaletteComboBox.SelectedIndex;

            try
            {
                CorrectProgressBar.IsIndeterminate = true;
                image = await Controller.SynthImageTask(synthImage, layers, palette);
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

        private void SelectedBand(object sender, RoutedEventArgs e)
        {
            if (LayerManager.instance.SelectImage == -1)
            {
                MessageBox.Show("Необходимо выбрать изображения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            var button = sender as Button;

            if (button.Name == BandAButton.Name && layers.Length >= 1)
            {
                layers[0] = LayerManager.instance.layers[LayerManager.instance.SelectImage];
            }
            if (button.Name == BandRButton.Name && layers.Length >= 2)
            {
                layers[1] = LayerManager.instance.layers[LayerManager.instance.SelectImage];
            }
            if (button.Name == BandGButton.Name && layers.Length >= 3)
            {
                layers[2] = LayerManager.instance.layers[LayerManager.instance.SelectImage];
            }
            if (button.Name == BandBButton.Name && layers.Length == 4)
            {
                layers[3] = LayerManager.instance.layers[LayerManager.instance.SelectImage];
            }

            button.IsEnabled = false;
            button.Content = (LayerManager.instance.SelectImage + 1).ToString() + " " + button.Content;
        }

        private void SelectLayer(object sender, SelectionChangedEventArgs e)
        {
            LayerManager.instance.SelectLayer(sender);
        }

        private void ClearLayerButtonClick(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = null;
            }

            BandAButton.IsEnabled = true;
            BandRButton.IsEnabled = true;
            BandGButton.IsEnabled = true;
            BandBButton.IsEnabled = true;

            BandAButton.Content = defaultName[0];
            BandRButton.Content = defaultName[1];
            BandGButton.Content = defaultName[2];
            BandBButton.Content = defaultName[3];
        }

        private void SaveLayerButtonClick(object sender, RoutedEventArgs e)
        {
            if (image == null)
            {
                MessageBox.Show("Ошибка сохранения файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); return;
            }

            LayerManager.instance.AddLayer(image, synthImage.GetType().Name);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            this.Close();
        }

        private void SaveResultButtonClick(object sender, RoutedEventArgs e)
        {
            if (image == null)
            {
                MessageBox.Show("Ошибка сохранения файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop); return;
            }
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();

            saveDialog.Filter = "TXT (*.txt)|*.txt|" +
                                "Все файлы (*.*)|*.*";
            saveDialog.Title = "Сохранить расчет...";
            
            saveDialog.FileName = synthImage.GetType().Name;

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveDialog.FileName, false, System.Text.Encoding.Default))
                    {
                        for (int i = 0; i < 20 ; i++)
                        {
                            sw.WriteLine(synthImage.plotValue.values[i,0].ToString() + "\t" + synthImage.plotValue.values[i, 1].ToString());
                        }
                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    MessageBox.Show("Успешно сохранено", "Успешно", MessageBoxButton.OK, MessageBoxImage.None);
                }
                catch
                {
                    MessageBox.Show("Невозможно сохранить изображение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void SaveImageButtonClick(object sender, RoutedEventArgs e)
        {
            if (image == null)
            {
                MessageBox.Show("Ошибка сохранения файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop); return;
            }
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();

            saveDialog.Filter = "PNG (*.png)|*.png|" +
                                "JPG (*.jpg, *.jpeg)|*.jpg; *.jpeg;|" +
                                "TIFF (*.tiff)|*.tiff|" +
                                "BMP (*.bmp)|*.bmp|" +
                                "Все файлы (*.*)|*.*";
            saveDialog.Title = "Сохранить картинку как...";
            saveDialog.FileName = synthImage.GetType().Name;

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    using (var fileStream = new FileStream(saveDialog.FileName, System.IO.FileMode.Create))
                    {
                        encoder.Save(fileStream);
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    MessageBox.Show("Успешно сохранено", "Успешно", MessageBoxButton.OK, MessageBoxImage.None);
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
