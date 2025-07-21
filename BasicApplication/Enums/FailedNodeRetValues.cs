/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.BasicApplication.Enums
{
    //NOT_PRIMARY_CONTROLLER = 1;
    //NO_CALLBACK_FUNCTION = 2;
    //FAILED_NODE_NOT_FOUND = 3;
    //FAILED_NODE_REMOVE_PROCESS_BUSY = 4;
    //FAILED_NODE_REMOVE_FAIL = 5;

    public enum FailedNodeRetValues
    {
        ZW_FAILED_NODE_REMOVE_STARTED = 0x00,
        ZW_NOT_PRIMARY_CONTROLLER = 0x02,
        ZW_NO_CALLBACK_FUNCTION = 0x04,
        ZW_FAILED_NODE_NOT_FOUND = 0x08,
        ZW_FAILED_NODE_REMOVE_PROCESS_BUSY = 0x10,
        ZW_FAILED_NODE_REMOVE_FAIL = 0x20
    }
}
