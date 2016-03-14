using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLeh.SocketListener.Client
{
    public delegate void DataEventHandler<T>(object sender, DataEventArgs<T> e);

    public class DataEventArgs<T> : EventArgs
    {
        public T Data { get; set; }

        public DataEventArgs(T obj)
        {
            Data = obj;
        }
    }
}
