﻿// Copyright (c) Microsoft. All rights reserved.

using SemanticKernel.Service.Skills;

namespace SemanticKernel.Service.Storage;

/// <summary>
/// A repository for chat sessions.
/// </summary>
public class ChatSessionRepository : Repository<ChatSession>
{
    /// <summary>
    /// Initializes a new instance of the ChatSessionRepository class.
    /// </summary>
    /// <param name="storageContext">The storage context.</param>
    public ChatSessionRepository(IStorageContext<ChatSession> storageContext)
        : base(storageContext)
    {
    }

    /// <summary>
    /// Finds chat sessions by user id.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <returns>A list of chat sessions.</returns>
    public Task<IEnumerable<ChatSession>> FindByUserIdAsync(string userId)
    {
        return Task.FromResult(base.StorageContext.QueryableEntities.Where(e => e.UserId == userId).AsEnumerable());
    }
}
