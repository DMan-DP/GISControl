using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GISControl.View
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            DragMove();
        }

        void MinButtonClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        void MaxButtonClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
            else WindowState = WindowState.Maximized;
        }

        void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ImageWheelScrol(object sender, MouseWheelEventArgs e)
        {
            /*
                Скрол по горизонтали с зажатым shift 
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (e.Delta > 0)
                        ScrollViewer.LineLeft();
                    else
                        ScrollViewer.LineRight();
                    e.Handled = true;
                }

                Увеличение картинки при скроле с зажатым ctrl
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            */
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

        private Point scrollMousePoint = new Point();
        private double horizontalOffset = 1;
        private double verticalOffset = 1;

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
    }
}