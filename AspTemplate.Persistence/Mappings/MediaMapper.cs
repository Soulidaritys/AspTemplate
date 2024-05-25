using AspTemplate.Core.Models;
using AspTemplate.Persistence.Entities;

namespace AspTemplate.Persistence.Mappings;

public class MediaMapper
{
    public MediaEntity ToEntity(Media model)
    {
        var target = new MediaEntity();
        target.ObjectKey = model.ObjectKey.Value;
        target.MediaType = model.MediaType;
        target.OriginalFileName = model.OriginalFileName;
        target.CreatedByUserId = model.CreatedByUserId?.Value;
        target.CreatedAt = model.CreatedAt;
        target.Width = model.Width;
        target.Height = model.Height;
        target.DurationInMs = model.DurationInMs;

        return target;
    }

    public Media ToDomain(MediaEntity entity)
    {
        var target = new Media(
            new MediaId(entity.ObjectKey),
            entity.MediaType,
            entity.OriginalFileName,
            entity.CreatedByUserId != null ? new UserId(entity.CreatedByUserId.Value) : null,
            entity.CreatedAt,
            entity.Width,
            entity.Height,
            entity.DurationInMs
        );

        return target;
    }
}