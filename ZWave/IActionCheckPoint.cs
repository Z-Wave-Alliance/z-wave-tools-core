/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace ZWave
{
    public interface IActionCheckPoint
    {
        /// <summary>
        /// Gets a value indicating if check point thread can accept operation
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// Starts operations checkpoint thread 
        /// </summary>
        /// <param name="delay">Delay before handling next operation</param>
        void Start(int delay);

        /// <summary>
        /// Stops operations checkpoint thread 
        /// </summary>
        void Stop();

        /// <summary>
        /// Adds operation to checkpoint thread
        /// </summary>
        /// <param name="operation">Operation to be handled</param>
        void Pass(ActionBase operation);
    }
}
