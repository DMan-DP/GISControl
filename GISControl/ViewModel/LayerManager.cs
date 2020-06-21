using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using GISControl.Model;
using GISControl.Model.Help;

namespace GISControl.ViewModel
{
    class LayerManager : Singleton<LayerManager>
    {
        private int selectImage = -1;
        public ObservableCollection<Layer> layers { get; } = new ObservableCollection<Layer>();
        public int SelectImage => selectImage;

        public void AddLayer(string fileName, string filePath)
        {
            layers.Add(new Layer(fileName, filePath));
            selectImage = layers.Count - 1;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void AddLayer(BitmapImage image, string name)
        {
            layers.Add(new Layer(image, name));
            selectImage = layers.Count - 1;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public int RemoveLayer(Button button)
        {
            selectImage = layers.IndexOf((Layer)button.DataContext) - 1;
            layers.Remove((Layer)button.DataContext);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (layers.Count != 0 && selectImage == -1) selectImage = 0;
            return selectImage;
        }

        public void RemoveAllLayer()
        {
            selectImage = -1;
            layers.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void SelectLayer(object sender)
        {
            if (((ListBox)sender).SelectedItem != null)
            {
                selectImage = ((ListBox)sender).SelectedIndex;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
