/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Utils.UI.Bind;

namespace Utils.UI
{
    public class UListFactory : ISubscribeCollectionFactory
    {
        #region ISubscribeCollectionFactory Members

        public ISubscribeCollection<T> Create<T>() where T : class
        {
            return new UList<T>();
        }

        public Collection<T> CreateCollection<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public ISubscribeCollection<T> Create<T>(IEnumerable<T> innerData) where T : class
        {
            if (innerData == null)
                return Create<T>();
            return new UList<T>(innerData);
        }

        public Collection<T> CreateCollection<T>(IEnumerable<T> innerData) where T : class
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
