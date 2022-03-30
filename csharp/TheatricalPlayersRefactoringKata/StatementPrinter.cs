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
            var result = GetCustomerString(invoice);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances) 
            {
                var play = plays[perf.PlayID];
                var thisAmount = CalculatePrice(play.Type, perf.Audience);

                volumeCredits = CalculateVolumeCredits(volumeCredits, perf.Audience, play.Type);

                result += GetSeatsString(cultureInfo, play, thisAmount, perf);
                totalAmount += thisAmount;
            }
            result += GetAmountString(cultureInfo, totalAmount);
            result += GetCreditsString(volumeCredits);
            return result;
        }

        private static string GetCreditsString(int volumeCredits)
        {
            return string.Format("You earned {0} credits\n", volumeCredits);
        }

        private static string GetAmountString(CultureInfo cultureInfo, int totalAmount)
        {
            return string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
        }

        private static string GetCustomerString(Invoice invoice)
        {
            return string.Format("Statement for {0}\n", invoice.Customer);
        }

        private static string GetSeatsString(CultureInfo cultureInfo, Play play, int thisAmount, Performance perf)
        {
            return string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience);
        }

        private static int CalculateVolumeCredits(int volumeCredits, int audience, string playType)
        {
            volumeCredits += Math.Max(audience - 30, 0);
            if ("comedy" == playType) volumeCredits += (int)Math.Floor((decimal)audience / 5);
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
