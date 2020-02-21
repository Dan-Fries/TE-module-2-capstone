using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Capstone.Views
{
    public static class ObjectListViews
    {

        public static void DisplayParksDetailedView(IList<Park> parks)
        {
            string[] labels = { "Location:", "Established:", "Area:", "Annual Visitors:" };
            foreach (Park park in parks)
            {
                Console.WriteLine();
                Console.WriteLine(park.Name);
                Console.WriteLine($"{labels[0],-24}{park.Location}");
                Console.WriteLine($"{labels[1],-24}{park.EstablishDate:d}");
                Console.WriteLine($"{labels[2],-24}{park.Area:n} sq km");
                Console.WriteLine($"{labels[2],-24}{park.Visitors}");
                Console.WriteLine();
                DisplayParagraphWithWordWrap(park.Description);

            }
        }

        public static void DisplayParksSingleLine(IList<Park> parks)
        {
            Console.WriteLine();
            foreach (Park park in parks)
            {
                Console.WriteLine($"{park.ParkId}) {park.Name}");
            }
        }

        public static void DisplayCampgrounds(IList<Campground> campgrounds)
        {
            string[] labels = { "Name", "Open", "Close", "Daily Fee" };
            Console.WriteLine();
            Console.WriteLine($"       {labels[0],-36}{labels[1],-12}{labels[2],-12}{labels[3],-12}");
            for (int i = 0; i < campgrounds.Count(); i++)
            {

                Console.WriteLine($"#{campgrounds[i].CampgroundId,-6}{campgrounds[i].Name,-36}{intToMonth(campgrounds[i].OpenFromMonth),-12}{intToMonth(campgrounds[i].OpenToMonth),-12}{campgrounds[i].DailyFee,-12:C}");

            }
        }

        public static void DisplayCampSites(IList<Site> sites, IList<Campground> campgrounds, int campgroundSelection, int numDays)
        {
            string[] labels = { "Site No.", "Max Occup.", "Accessible?", "Max RV Length", "Utility", "Cost" };
            Console.WriteLine();
            Console.WriteLine($"{labels[0],-10}{labels[1],-16}{labels[2],-16}{labels[3],-16} {labels[4],-16} {labels[5],-16}");
            decimal price = 0m;

            foreach (Campground campground in campgrounds)
            {
                if (campground.CampgroundId == campgroundSelection)
                {
                    price = campground.DailyFee;
                    break;
                }
            }

            if (numDays != 0)
            {
                price *= numDays;
            }

            for (int i = 0; i < sites.Count(); i++)
            {

                Console.WriteLine($"#{sites[i].SiteNumber,-10}{sites[i].MaxOccupancy,-16}{FormatAccesibility(sites[i].Accessible),-16}{FormatRVLength(sites[i].MaxRVLength),-16}{FormatUtilities(sites[i].Utilities),-16}{price,-16:C}");

            }
        }

        public static void DisplaySingleReservation(Reservation reservation)
        {
            Console.WriteLine($"Reservation #{reservation.ReservationId} was booked on {reservation.BookingDate:d} for {reservation.StartDate:d} to {reservation.EndDate:d}.");
        }

        // Method to wrap the words of a paragraph according to console screen width, code found at:
        // https://social.msdn.microsoft.com/Forums/en-US/1ec953bc-f776-466c-a2f7-f29a2a3440c2/make-consolewriteline-wrap-words-instead-of-letters-with-methods
        private static void DisplayParagraphWithWordWrap(string text)
        {
            int width = Console.WindowWidth;
            string pattern = @"(?<line>.{1," + width + @"})(?<!\s)(\s+|$)|(?<line>.+?)(\s+|$)";
            var lines = Regex.Matches(text, pattern).Cast<Match>().Select(m => m.Groups["line"].Value);

            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        private static string intToMonth(int month)
        {
            Dictionary<int, string> numberToMonth = new Dictionary<int, string>()
            {
               { 1, "January" },
               { 2, "February"},
               { 3, "March"},
               {4, "April"},
               {5, "May"},
               {6, "June"},
               {7, "July"},
               {8, "August"},
               {9, "September"},
               {10, "October"},
               {11, "November"},
               {12, "December"}

            };

            return numberToMonth[month];
        }
        private static string FormatAccesibility(bool accessibility)
        {
            Dictionary<bool, string> boolToWord = new Dictionary<bool, string>()
            {
               { false, "No" },
               { true, "Yes"},
            };

            return boolToWord[accessibility];
        }

        private static string FormatRVLength(int rvLength)
        {
            if (rvLength == 0)
            {
                return "N/A";
            }
            return $"{rvLength}";
        }

        private static string FormatUtilities(bool utility)
        {
            Dictionary<bool, string> boolToWord = new Dictionary<bool, string>()
            {
               { false, "N/A" },
               { true, "Yes"},
            };

            return boolToWord[utility];
        }
    }
}