﻿using System;
using Bencodex.Types;
using Libplanet;
using Nekoyume.Action;
using Nekoyume.Model.State;

namespace Nekoyume.Model.Event
{
    public class EventDungeonInfo : IState
    {
        public static Address DeriveAddress(Address address, int dungeonId)
        {
            return address.Derive($"event_dungeon_info_{dungeonId}");
        }

        private int _remainingTickets;
        private int _clearedStageId;

        public EventDungeonInfo()
        {
            _remainingTickets = 0;
            _clearedStageId = 0;
        }

        public EventDungeonInfo(Bencodex.Types.List serialized)
        {
            _remainingTickets = serialized[0].ToInteger();
            _clearedStageId = serialized[1].ToInteger();
        }

        public EventDungeonInfo(Bencodex.Types.IValue serialized)
            : this((Bencodex.Types.List)serialized)
        {
        }

        public IValue Serialize() => Bencodex.Types.List.Empty
            .Add(_remainingTickets.Serialize())
            .Add(_clearedStageId.Serialize());

        public void ResetTickets(int tickets)
        {
            if (tickets < 0)
            {
                throw new ArgumentException(
                    $"{nameof(tickets)} must be greater than or equal to 0.");
            }

            _remainingTickets = tickets;
        }

        public bool HasTickets(int tickets)
        {
            if (tickets < 0)
            {
                throw new ArgumentException(
                    $"{nameof(tickets)} must be greater than or equal to 0.");
            }

            return _remainingTickets >= tickets;
        }

        public bool TryUseTickets(int tickets)
        {
            if (tickets < 0)
            {
                throw new ArgumentException(
                    $"{nameof(tickets)} must be greater than or equal to 0.");
            }

            if (_remainingTickets < tickets)
            {
                return false;
            }

            _remainingTickets -= tickets;
            return true;
        }

        public void ClearStage(int stageId)
        {
            if (_clearedStageId >= stageId)
            {
                return;
            }

            _clearedStageId = stageId;
        }

        public bool IsCleared(int stageId) =>
            _clearedStageId >= stageId;

        protected bool Equals(EventDungeonInfo other)
        {
            return _remainingTickets == other._remainingTickets &&
                   _clearedStageId == other._clearedStageId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EventDungeonInfo)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_remainingTickets, _clearedStageId);
        }
    }
}