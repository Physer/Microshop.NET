﻿namespace Application.Interfaces.Indexing;

public interface IIndexingClient
{
    Task AddDocumentsAsync<T>(IMicroshopIndex index, IEnumerable<T> documentsToIndex);
    Task DeleteAllDocumentsAsync(IMicroshopIndex index);
    Task<IEnumerable<T>> GetAllDocumentsAsync<T>(IMicroshopIndex index);
}