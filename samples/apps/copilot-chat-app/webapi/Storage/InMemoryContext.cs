﻿// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Concurrent;

namespace SemanticKernel.Service.Storage;

/// <summary>
/// A storage context that stores entities in memory.
/// </summary>
public class InMemoryContext<T> : IStorageContext<T> where T : IStorageEntity
{
    /// <summary>
    /// Using a concurrent dictionary to store entities in memory.
    /// </summary>
    private readonly ConcurrentDictionary<string, T> _entities;

    /// <summary>
    /// Initializes a new instance of the InMemoryContext class.
    /// </summary>
    public InMemoryContext()
    {
        this._entities = new ConcurrentDictionary<string, T>();
    }

    /// <inheritdoc/>
    public IQueryable<T> QueryableEntities => this._entities.Values.AsQueryable();

    /// <inheritdoc/>
    public Task CreateAsync(T entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Id))
        {
            throw new ArgumentOutOfRangeException(nameof(entity.Id), "Entity Id cannot be null or empty.");
        }

        this._entities.TryAdd(entity.Id, entity);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task DeleteAsync(T entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Id))
        {
            throw new ArgumentOutOfRangeException(nameof(entity.Id), "Entity Id cannot be null or empty.");
        }

        this._entities.TryRemove(entity.Id, out _);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<T> ReadAsync(string entityId)
    {
        if (string.IsNullOrWhiteSpace(entityId))
        {
            throw new ArgumentOutOfRangeException(nameof(entityId), "Entity Id cannot be null or empty.");
        }

        if (this._entities.TryGetValue(entityId, out T? entity))
        {
            return Task.FromResult(entity);
        }
        else
        {
            throw new KeyNotFoundException($"Entity with id {entityId} not found.");
        }
    }

    /// <inheritdoc/>
    public Task UpdateAsync(T entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Id))
        {
            throw new ArgumentOutOfRangeException(nameof(entity.Id), "Entity Id cannot be null or empty.");
        }

        this._entities.TryUpdate(entity.Id, entity, this._entities[entity.Id]);

        return Task.CompletedTask;
    }
}
