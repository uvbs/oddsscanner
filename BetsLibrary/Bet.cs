using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetsLibrary
{
    public abstract class Bet
    {
        public double Odds { get; protected set; }

        public void ChangeOdds(double newOdds)
        {
            this.Odds = newOdds;
        }
    }

    public enum ResultBetType
    {
        P1,
        P2,
        First,
        Draw,
        Second,
        FirstOrDraw,
        FirstOrSecond,
        SecondOrDraw
    }

    public class ResultBet : Bet
    {
        public ResultBetType ResultBetType { get; private set; }
        public Time Time { get; protected set; }

        public ResultBet(ResultBetType ResultBetType, Time time, double odds)
        {
            this.ResultBetType = ResultBetType;
            this.Time = time;
            this.Odds = odds;
        }

        public override int GetHashCode()
        {
            return ResultBetType.GetHashCode() ^ Time.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is ResultBet bet)
            {
                return Equals(bet);
            }
            else return false;
        }

        public bool Equals(ResultBet bet)
        {
            return bet.Time == Time && bet.ResultBetType == ResultBetType;
        }

    }

    public enum HandicapBetType
    {
        F1,
        F2
    }

    public class HandicapBet : Bet
    {
        public HandicapBetType HandicapBetType { get; private set; }
        public Time Time { get; protected set; }
        public double HandicapValue { get; private set; }

        public HandicapBet(HandicapBetType handicapBetType, double HandicapValue, Time time, double odds)
        {
            this.Odds = odds;
            this.HandicapBetType = handicapBetType;
            this.HandicapValue = HandicapValue;
            this.Time = time;
        }

        public override int GetHashCode()
        {
            return HandicapBetType.GetHashCode() ^ Time.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is HandicapBet bet)
            {
                return Equals(bet);
            }
            else return false;
        }

        public bool Equals(HandicapBet bet)
        {
            return bet.Time == Time && bet.HandicapBetType == HandicapBetType && bet.HandicapValue == HandicapValue;
        }

    }

    public enum TotalBetType
    {
        Over,
        Under
    }

    public class TotalBet : Bet
    {   
        public TotalBetType TotalBetType { get; private set; }
        public Time Time { get; protected set; }
        public double TotalValue { get; private set; }
        public Team Team { get; private set; }

        public TotalBet(TotalBetType TotalBetType, double TotalValue, Time time, Team team, double odds)
        {
            this.Odds = odds;
            this.TotalBetType = TotalBetType;
            this.TotalValue = TotalValue;
            this.Time = time;
            this.Team = team;
        }

        public override int GetHashCode()
        {
            return TotalBetType.GetHashCode() ^ Time.GetHashCode() ^ Team.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is TotalBet bet)
            {
                return Equals(bet);
            }
            else return false;
        }

        public bool Equals(TotalBet bet)
        {
            return bet.Time == Time && bet.TotalBetType == TotalBetType && bet.TotalValue == TotalValue && bet.Team == Team;
        }
    }

    public enum Time
    {
        First,
        Second,
        Third,
        Fourth,
        AllGame
    }

    public enum Team
    {
        First,
        Second,
        All
    }

    

    
}
