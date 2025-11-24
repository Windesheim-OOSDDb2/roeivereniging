using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Data.Repositories
{
    internal class ReservationRepository : DatabaseConnection, IReservationRepository
    {
        private readonly List<Reservation> reseravtionList;

        public ReservationRepository()
        {
            CreateTable(@"CREATE TABLE IF NOT EXISTS Reservations (
                            [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                            [Name] TEXT NOT NULL,
                            [PassengerCount] INTEGER NOT NULL,
                            [DateTime] TEXT NOT NULL,
                            [UserId] INTEGER NOT NULL,
                            [BoatId] INTEGER NOT NULL,
                            UNIQUE(UserId, BoatId, DateTime)
                )");
        }
    }
}
