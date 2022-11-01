using Catalog.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.DAL.UOW.Interface
{
    public interface IUnitOfWork
    {
        IAuthorRepository AuthorRepository { get; }
        IExemplarRepository ExemplarRepository { get; }
        IGenreRepository GenreRepository { get; }
        ILiteratureRepository LiteratureRepository { get; }
        IPublisherRepository PublisherRepository { get; }
        ITypeRepository TypeRepository { get; }
        IWritingRepository WritingRepository { get; }

        Task SaveChangesAsync();
    }
}
