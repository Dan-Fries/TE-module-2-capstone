using Capstone.DAL;
using System;
using System.Collections.Generic;

namespace Capstone.Views
{
    /// <summary>
    /// The top-level menu in our Market Application
    /// </summary>
    public class MainMenu : CLIMenu
    {
        // DAOs - Interfaces to our data objects can be stored here...
        protected IParkDAO parkDAO;
        protected ICampgroundDAO campgroundDAO;
        protected ISiteDAO siteDAO;
        protected IReservationDAO reservationDAO;
        //protected ICountryDAO countryDAO;

        /// <summary>
        /// Constructor adds items to the top-level menu. YOu will likely have parameters for one or more DAO's here...
        /// </summary>
        public MainMenu(IParkDAO parkDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO) : base("Main Menu")
        {
            this.parkDAO = parkDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }

        protected override void SetMenuOptions()
        {
            this.menuOptions.Add("1", "List National Parks");
            this.menuOptions.Add("2", "List Campgrounds at a National Park");
            this.menuOptions.Add("3", "Make a reservation");
            this.menuOptions.Add("Q", "Quit program");
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
                case "1": // Display all parks with summary information
                    ObjectListViews.DisplayParks(parkDAO.GetAllParks());
                    Pause("");
                    return true;    // Keep running the main menu
                case "2": // Display all the campgrounds at a selected national park
                    ObjectListViews.DisplayParksSingleLine(parkDAO.GetAllParks());
                    int parkId = GetInteger("Please select a national park by Id to display the campgrounds at that park:");
                    ObjectListViews.DisplayCampgrounds(campgroundDAO.GetCampgroundsByParkId(parkId));
                    Pause("");
                    return true;    // Keep running the main menu
                case "3": // Create and show the reservation sub-menu
                    ReservationMenu rm = new ReservationMenu(parkDAO, campgroundDAO, siteDAO, reservationDAO);
                    rm.Run();
                    return true;    // Keep running the main menu
            }
            return true;
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
        }


        private void PrintHeader()
        {
            SetColor(ConsoleColor.Yellow);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("National Parks"));
            ResetColor();
        }
    }
}
