﻿using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        IList<Reservation> GetAllReservations();

        IList<Reservation> GetAllReservationsByCampgroundId(int campgroundId);

        int MakeReservation(int siteId, int campgroundId, string name, DateTime startDate, DateTime endDate);
    }
}
