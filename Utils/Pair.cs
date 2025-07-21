/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Utils
{
    [Serializable]
    public class Pair<TFirst, TSecond> : IEquatable<Pair<TFirst, TSecond>>, INotifyPropertyChanged
    {
        public Pair()
        {
        }
        public Pair(TFirst first, TSecond second)
        {
            _first = first;
            _second = second;
        }
        private TFirst _first;
        public TFirst First
        {
            get { return _first; }
            set 
            { 
                _first = value;
                OnPropertyChanged("First");
            }
        }
        private TSecond _second;
        public TSecond Second
        {
            get { return _second; }
            set 
            {
                _second = value;
                OnPropertyChanged("Second");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IEquatable<Pair<TFirst,TSecond>> Members

        public bool Equals(Pair<TFirst, TSecond> other)
        {
            return other != null && First.Equals(other.First) && Second.Equals(other.Second);
        }

        #endregion
    }

    public class PairComparer<TFirst, TSecond> : IEqualityComparer<Pair<TFirst, TSecond>>
    {

        #region IEqualityComparer<Pair<TFirst,TSecond>> Members

        public bool Equals(Pair<TFirst, TSecond> x, Pair<TFirst, TSecond> y)
        {
            return x.First.Equals(y.First) && x.Second.Equals(y.Second);
        }

        public int GetHashCode(Pair<TFirst, TSecond> obj)
        {
            return obj.First.GetHashCode() ^ obj.Second.GetHashCode();
        }

        #endregion
    }

    public class Arg : Pair<string, object>
    {
        public Arg(string name, object value)
            : base(name, value)
        { }
    }
}
