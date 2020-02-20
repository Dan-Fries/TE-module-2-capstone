using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace Capstone.DAL
{
    public interface ISiteDAO
    {
        IList<Site> GetAllSites();

        IList<Site> GetSitesByCampgroundId(int campgroundId);

        IList<Site> GetAvailableSites(int campgroundId, DateTime startDate, DateTime endDate);

        IList<Site> GetAvailableSitesAdvanced(int campgroundId, DateTime startDate, DateTime endDate, int maxOccupancyRequired, bool isAccessible, int rvSizeRequired, bool isHookupRequired);

    }
}
