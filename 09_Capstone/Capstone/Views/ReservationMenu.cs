using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.Views
{
    /// <summary>
    /// The top-level menu in our Market Application
    /// </summary>
    public class ReservationMenu : CLIMenu
    {
        // Store any private variables, including DAOs here....
        protected IParkDAO parkDAO;
        protected ICampgroundDAO campgroundDAO;
        protected ISiteDAO siteDAO;
        protected IReservationDAO reservationDAO;

        /// <summary>
        /// Constructor adds items to the top-level menu
        /// </summary>
        public ReservationMenu(IParkDAO parkDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO) :
            base("Sub-Menu 1")
        {
            this.parkDAO = parkDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }

        protected override void SetMenuOptions()
        {
            this.menuOptions.Add("1", "Book a campsite");
            this.menuOptions.Add("2", "Book a campsite with advanced search options");
            this.menuOptions.Add("B", "Back to Main Menu");
            this.quitKey = "B";
        }

        /// <summary>
        /// The override of ExecuteSelection handles whatever selection was made by the user.
        /// This is where any business logic is executed.
        /// </summary>
        /// <param name="choice">"Key" of the user's menu selection</param>
        /// <returns></returns>
        protected override bool ExecuteSelection(string choice)
        {
            switch (choice)
            {
                case "1": // Do whatever option 1 is
                    MakeReservation(false);
                    Pause("");
                    return true;
                case "2": // Do whatever option 2 is
                    //int campgroundSelection = GetInteger("Choose a campground: ");
                    //ObjectListViews.DisplayCampSites(siteDAO.GetSitesByCampgroundId(campgroundSelection), campgroundsByID, campgroundSelection);
                    MakeReservation(true);


                    WriteError("Not yet implemented");
                    Pause("");
                    return true;
            }
            return true;
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
        }

        protected override void AfterDisplayMenu()
        {
            base.AfterDisplayMenu();
            SetColor(ConsoleColor.Cyan);
            Console.WriteLine("Display some data here, AFTER the sub-menu is shown....");
            ResetColor();
        }

        private void PrintHeader()
        {
            SetColor(ConsoleColor.Magenta);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Reservations"));
            ResetColor();
        }

        private void MakeReservation(bool isAdvancedSearch)
        {
            // Display a list of all parks for the user to choose from
            ObjectListViews.DisplayParksSingleLine(parkDAO.GetAllParks());

            // Prompt the user to select a park and pass the verified selection to Display campgrounds at that park
            ObjectListViews.DisplayCampgrounds(campgroundDAO.GetCampgroundsByParkId(GetInteger("Please choose a park to view available campgrounds: ")));
            
            // Prompt user to choose a campground and store this value for later use
            int campgroundId = GetInteger("Choose a campground (Select 0 to cancel): ");
            if (campgroundId == 0) return;

            // Prompt the user to enter an arrival and departure date and store them and then calculate the total number of days of the stay
            DateTime startDate = GetDateTime("Please enter an arrival date: ");
            DateTime endDate = GetDateTime("Please enter a departure date: ");
            int numDays = (int)((endDate - startDate).TotalDays);

            // Display a list of available campsites that meet the users selected options
            IList<Site> sites = new List<Site>();
            if (isAdvancedSearch)
            {
                int maxOccupancyRequired = GetInteger("What is the max occupancy required: ");
                bool isAccessible = GetBool("Do you need a weelchair accessible site (y/n): ");
                int rvSizeRequired = GetInteger("What size RV parking is required (Enter 0 for no RV): ");
                bool isHookupRequired = GetBool("Do you need utility hookups (y/n): ");
                sites = siteDAO.GetAvailableSitesAdvanced(campgroundId, startDate, endDate, maxOccupancyRequired, isAccessible, rvSizeRequired, isHookupRequired);
            }
            else
            {
                sites = siteDAO.GetAvailableSites(campgroundId, startDate, endDate);
            }

            ObjectListViews.DisplayCampSites(sites, campgroundDAO.GetAllCampgrounds(), campgroundId, numDays);
            
            // Prompt the user to choose a site to reserve
            int siteNumber = GetInteger("Select a site that you want to reserve (enter 0 to cancel):");
            if (siteNumber == 0) return;

            // Prompt the user for a name to book the reservation under
            string name = GetString("Enter the name for the reservation: ");

            // Make the reservation and return to the user a confirmation number
            int reservationId = reservationDAO.MakeReservation(siteNumber, campgroundId, name, startDate, endDate);
            if (reservationId == 0)
            {
                Console.WriteLine("Sorry but you did not enter a valid campsite");
            }
            else
            {
                Console.WriteLine($"The reservation has been made and the confirmation id is {reservationId}");
            }
            return;
        }

    }
}
