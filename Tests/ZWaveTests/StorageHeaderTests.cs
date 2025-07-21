/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using NUnit.Framework;
using ZWave;

namespace ZWaveTests
{
    [TestFixture]
    public class StorageHeaderTests
    {
        [Test]
        public void Create_Default_VersionIs103()
        {
            StorageHeader sh = new StorageHeader();
            sh = StorageHeader.GetHeader(sh.GetBuffer());
            Assert.AreEqual(104, sh.Version);
            Assert.IsTrue(StorageHeader.IsValid(sh.GetBuffer()));
        }
    }
}
