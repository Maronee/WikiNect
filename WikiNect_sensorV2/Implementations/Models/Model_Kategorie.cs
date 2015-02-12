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
    public class ModelKategorie : INotifyPropertyChanged
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

        private BitmapImage _imagesource;
        public BitmapImage imagesource
        {
            get { return _imagesource; }
            set
            {
                _imagesource = value;
                OnPropertyChanged(new PropertyChangedEventArgs("imagesource"));
            }
        }

        private Category _categorie;
        public Category categorie
        {
            get { return _categorie; }
            set
            {
                _categorie = value;
                OnPropertyChanged(new PropertyChangedEventArgs("categorie"));
            }
        }

        public String _selected = "0";
        public String selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("selected"));
            }
        }

        public String _visible = "Hidden";
        public String visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("visible"));
            }
        }
    }
}
