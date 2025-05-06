/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave.Security
{
    public class AuthData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthData"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public AuthData(byte[] key)
        {
            mKey = key;
        }
        private byte[] mKey;
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public byte[] Key
        {
            get { return mKey; }
            set { mKey = value; }
        }

        private byte[] mNonces;
        /// <summary>
        /// Gets or sets the nonces.
        /// </summary>
        /// <value>The nonces.</value>
        public byte[] Nonces
        {
            get { return mNonces; }
            set { mNonces = value; }
        }

        private byte[] mIV;
        /// <summary>
        /// Gets or sets the IV.
        /// </summary>
        /// <value>The IV.</value>
        public byte[] IV
        {
            get { return mIV; }
            set { mIV = value; }
        }

        private byte mRI;
        /// <summary>
        /// Gets or sets the RI.
        /// </summary>
        /// <value>The RI.</value>
        public byte RI
        {
            get { return mRI; }
            set { mRI = value; }
        }

        private byte[] mMAC;
        /// <summary>
        /// Gets or sets the MAC.
        /// </summary>
        /// <value>The MAC.</value>
        public byte[] MAC
        {
            get { return mMAC; }
            set { mMAC = value; }
        }

        private byte[] mEncryptedPayload;

        /// <summary>
        /// Gets or sets the encrypted payload.
        /// </summary>
        /// <value>The encrypted payload.</value>
        public byte[] EncryptedPayload
        {
            get { return mEncryptedPayload; }
            set { mEncryptedPayload = value; }
        }


        private bool mIsValid;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid
        {
            get { return mIsValid; }
            set { mIsValid = value; }
        }

        private ZWaveSecurityAuthData mSecurityAuthData;

        /// <summary>
        /// Gets or sets the security auth data.
        /// </summary>
        /// <value>The security auth data.</value>
        public ZWaveSecurityAuthData SecurityAuthData
        {
            get { return mSecurityAuthData; }
            set { mSecurityAuthData = value; }
        }
    }
}
