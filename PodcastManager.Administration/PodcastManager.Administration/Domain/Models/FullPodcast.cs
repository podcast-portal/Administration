namespace PodcastManager.Administration.Domain.Models;

public record FullPodcast(
        object? Status,
        int Code,
        string Title,
        bool IsPublished = false);