using AspTemplate.Core.Enums;

namespace AspTemplate.Core.Models;

[StronglyTypedId(
    backingType: StronglyTypedIdBackingType.String, 
    jsonConverter: StronglyTypedIdJsonConverter.SystemTextJson)]
public partial struct MediaId { }

public class Media
{
    public Media(
        MediaId objectKey,
        MediaType mediaType,
        string originalFileName,
        UserId? createdByUserId,
        DateTimeOffset createdAt,
        int? width = null,
        int? height = null,
        uint? durationInMs = null)
    {
        ObjectKey = objectKey;
        MediaType = mediaType;
        OriginalFileName = originalFileName;
        CreatedByUserId = createdByUserId;
        CreatedAt = createdAt;
        Width = width;
        Height = height;
        DurationInMs = durationInMs;
    }

    public MediaId ObjectKey { get; private set; }
    public MediaType MediaType { get; private set; }
    public string OriginalFileName { get; private set; } = null!;

    public UserId? CreatedByUserId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public int? Width { get; private set; }
    public int? Height { get; private set; }

    public uint? DurationInMs { get; private set; }
}