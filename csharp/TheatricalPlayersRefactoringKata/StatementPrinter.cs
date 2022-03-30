using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public const int TragedyPrice = 40000;
        public const int ComedyPrice = 30000;

        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances) 
            {
                var play = plays[perf.PlayID];
                var thisAmount = CalculatePrice(play.Type, perf.Audience);
                // add volume credits
                volumeCredits = CalculateVolumeCredits(volumeCredits, perf, play.Type);
                // add extra credit for every ten comedy attendees=

                // print line for this order
                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience);
                totalAmount += thisAmount;
            }
            result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += string.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }

        private static int CalculateVolumeCredits(int volumeCredits, Performance perf, string playType)
        {
            volumeCredits += Math.Max(perf.Audience - 30, 0);
            if ("comedy" == playType) volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);
            return volumeCredits;
        }

        private static int CalculatePrice(string playType, int audience)
        {
            switch (playType)
            {
                case "tragedy":
                    return CalculatePriceByAudience(audience, TragedyPrice);
                case "comedy":
                    return CalculatePriceByAudience(audience, ComedyPrice) + 300 * audience;
                default:
                    throw new Exception("unknown type: " + playType);
            }
        }

        private static int CalculatePriceByAudience(int audience, int thisAmount)
        {
            if (audience > 30 && thisAmount > 30000)
            {
                thisAmount += 1000 * (audience - 30);

            }else if (audience > 20)
            {
                thisAmount += 10000 + 500 * (audience - 20);
            }

            return thisAmount;
        }
    }
}
