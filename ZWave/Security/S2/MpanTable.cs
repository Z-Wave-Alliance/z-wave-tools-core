/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using System.Collections.Generic;
using Utils;
using ZWave.Devices;
using System.Collections.Concurrent;

namespace ZWave.Security
{
    public class MpanTable
    {
        public const int MAX_RECORDS_COUNT = 10;
        private ConcurrentDictionary<NodeGroupId, MpanContainer> _table;

        /// <summary>
        /// Creates mpan table with default MPAN records capacity of 10
        /// </summary>
        public MpanTable() : this(MAX_RECORDS_COUNT)
        {
        }

        /// <summary>
        /// Creates mpan table with specified MPAN records capacity
        /// </summary>
        /// <param name="maxRecordsCount">MPAN records capacity</param>
        public MpanTable(int maxRecordsCount)
        {
            _table = new ConcurrentDictionary<NodeGroupId, MpanContainer>();
        }

        public MpanContainer GetContainer(NodeGroupId peerGroupId)
        {
            MpanContainer ret = null;
            if (_table.TryGetValue(peerGroupId, out ret))
            { }
            return ret;
        }

        public MpanContainer AddOrReplace(NodeGroupId nodeGroupId, byte sequenceNumber, NodeTag[] receiverGroupHandle, byte[] mpanState)
        {
            ResetLatestContainerByOwnerId(nodeGroupId.Node);

            var mpanContainer = new MpanContainer(nodeGroupId, mpanState, sequenceNumber, receiverGroupHandle)
            {
                IsLatestFromOwner = true
            };
            while (_table.Count >= MAX_RECORDS_COUNT)
            {
                var keyValue = _table.FirstOrDefault();
                _table.TryRemove(keyValue.Key, out MpanContainer tmp);
            }
            _table.AddOrUpdate(nodeGroupId, mpanContainer, (x, y) => mpanContainer);
            return mpanContainer;
        }

        public void RemoveRecord(NodeGroupId nodeGroupId)
        {
            if (_table.TryRemove(nodeGroupId, out MpanContainer container))
            {
            }
        }

        public void ClearMpanTable()
        {
            _table.Clear();
        }

        public bool CheckMpanExists(NodeGroupId nodeGroupId)
        {
            return _table.ContainsKey(nodeGroupId);
        }

        public byte FindGroup(NodeTag[] destNodesIds)
        {
            var mpanPair = _table.FirstOrDefault(mpan => mpan.Value.DestNodesEquals(destNodesIds));
            return mpanPair.Value != null ? mpanPair.Value.NodeGroupId.GroupId : (byte)0;
        }

        public bool IsRecordInMOSState(NodeGroupId nodeGroupId)
        {
            if (_table.ContainsKey(nodeGroupId))
            {
                return _table[nodeGroupId].IsMosState;
            }
            return false;
        }

        public byte[] SelectGroupIds(NodeTag owner)
        {
            var groupIds = from mpan in _table
                           where mpan.Value.NodeGroupId.Node.Id == owner.Id
                           select mpan.Value.NodeGroupId.GroupId;

            return groupIds.ToArray();
        }

        public List<MpanContainer> GetExistingRecords()
        {
            return _table.Select(x => x.Value).ToList();
        }

        public void ResetLatestContainerByOwnerId(NodeTag owner)
        {
            foreach (var container in _table.Where(item => item.Value.NodeGroupId.Node.Id == owner.Id).Select(x => x.Value))
            {
                container.IsLatestFromOwner = false;
            }
        }

        public MpanContainer GetLatestContainerByOwnerId(NodeTag owner)
        {
            MpanContainer ret = null;
            var record = _table.Where(item => item.Value.NodeGroupId.Node.Id == owner.Id && item.Value.IsLatestFromOwner)
                 .FirstOrDefault();

            ret = record.Value;
            return ret;
        }
    }
}
