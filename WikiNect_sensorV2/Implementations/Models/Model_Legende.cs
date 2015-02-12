using DataConnection;
using DataStore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace WikiNectLayout.Implementions.Model
{
    public class ModelLegende: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private String _title;
        public String title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("title"));
            }
        }

        private String _color;
        public String color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged(new PropertyChangedEventArgs("subTitle"));
            }
        }
    }
}
