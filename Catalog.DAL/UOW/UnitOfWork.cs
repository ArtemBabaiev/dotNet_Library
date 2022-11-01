using Catalog.DAL.Repository.Interface;
using Catalog.DAL.UOW.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.DAL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly dotNet_CatalogContext databaseContext;

        public IAuthorRepository AuthorRepository { get; }

        public IExemplarRepository ExemplarRepository { get; }

        public IGenreRepository GenreRepository { get; }

        public ILiteratureRepository LiteratureRepository { get; }

        public IPublisherRepository PublisherRepository { get; }

        public ITypeRepository TypeRepository { get; }

        public IWritingRepository WritingRepository { get; }

        public UnitOfWork(dotNet_CatalogContext databaseContext, IAuthorRepository authorRepository, 
            IExemplarRepository exemplarRepository, IGenreRepository genreRepository, 
            ILiteratureRepository literatureRepository, IPublisherRepository publisherRepository, 
            ITypeRepository typeRepository, IWritingRepository writingRepository)
        {
            this.databaseContext = databaseContext;
            this.AuthorRepository = authorRepository;
            this.ExemplarRepository = exemplarRepository;
            this.GenreRepository = genreRepository;
            this.LiteratureRepository = literatureRepository;
            this.PublisherRepository = publisherRepository;
            this.TypeRepository = typeRepository;
            this.WritingRepository = writingRepository;
        }

        public async Task SaveChangesAsync()
        {
            await databaseContext.SaveChangesAsync();
        }
    }
}
