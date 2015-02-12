using DataConnection;
using DataStore;
using Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace WikiNectLayout.Implementions.Model
{
    public class ModelImage : ModelKategorie    {

        public String _artist;
        public String artist
        {
            get { return _artist; }
            set
            {
                _artist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("artist"));
            }
        }

        public String _year;
        public String year
        {
            get { return _year; }
            set
            {
                _year = value;
                OnPropertyChanged(new PropertyChangedEventArgs("year"));
            }
        }

        public String _museum;
        public String museum
        {
            get { return _museum; }
            set
            {
                _museum = value;
                OnPropertyChanged(new PropertyChangedEventArgs("museum"));
            }
        }

        public String _url;
        public String url
        {
            get { return _url; }
            set
            {
                _url = value;
                OnPropertyChanged(new PropertyChangedEventArgs("url"));
            }
        }

        public KinImage _kimage;
        public KinImage kimage
        {
            get { return _kimage; }
            set
            {
                _kimage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("kimage"));
            }
        }
    }
}
