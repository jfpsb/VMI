﻿using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace VandaModaIntimaWpf.Model.Pix
{
    public class QRCode : AModel, IModel
    {
        private int _id;
        private string _qrcode;
        private string _imagemQrcode;

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public virtual string Qrcode
        {
            get
            {
                return _qrcode;
            }

            set
            {
                _qrcode = value;
                OnPropertyChanged("Qrcode");
            }
        }

        public virtual string ImagemQrcode
        {
            get
            {
                return _imagemQrcode;
            }

            set
            {
                _imagemQrcode = value;
                OnPropertyChanged("ImagemQrcode");
            }
        }

        public virtual BitmapImage QrCodeBitmap
        {
            get
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(Convert.FromBase64String(ImagemQrcode));
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }
    }
}
