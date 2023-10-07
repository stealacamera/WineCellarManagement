namespace WineCellar.DataAccess.Repositories.IRepositories
{
    public interface IWorkUnit
    {
        IVarietalRepository Varietal { get; }
        IWineProducerRepository WineProducer { get; }
        ICountryRepository Country { get; }
        IRegionRepository Region { get; }
        IEstablishmentRepository Establishment { get; }
        IAppUserRepository AppUser { get; }
        IAppRoleRepository AppRole { get; }

        void Save();
    }
}
