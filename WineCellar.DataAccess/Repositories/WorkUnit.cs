﻿using WineCellar.DataAccess.Repositories.IRepositories;

namespace WineCellar.DataAccess.Repositories
{
    public class WorkUnit : IWorkUnit
    {
        private readonly AppDbContext db;
        public IVarietalRepository Varietal { get; private set; }
        public IWineProducerRepository WineProducer { get; private set; }
        public ICountryRepository Country { get; private set; }
        public IRegionRepository Region { get; private set; }

        public WorkUnit(AppDbContext db)
        {
            this.db = db;

            Varietal = new VarietalRepository(db);
            WineProducer = new WineProducerRepository(db);
            Country = new CountryRepository(db);
            Region = new RegionRepository(db);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
