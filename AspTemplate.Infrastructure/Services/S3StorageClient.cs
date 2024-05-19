using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AspTemplate.Infrastructure.Services;

/// <summary>
/// S3 storage client
/// </summary>
public class S3StorageClient
{
    private readonly IAmazonS3 _s3Client;
    private readonly ILogger<S3StorageClient> _logger;

    public S3StorageClient(IAmazonS3 s3Client, ILogger<S3StorageClient> logger)
    {
        _s3Client = s3Client;
        _logger = logger;
    }

    public async Task<bool> TryUploadFile(IFormFile file, string bucketName, string? key = null)
    {
        try
        {
            if (!await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName))
                return false;

            key ??= Path.GetRandomFileName() + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName).ToLowerInvariant();
            
            var transferUtilityConfig = new TransferUtilityConfig
            {
                ConcurrentServiceRequests = 5,
                MinSizeBeforePartUpload = 20 * 1024 * 1024,
            };

            using var transferUtility = new TransferUtility(_s3Client, transferUtilityConfig);
            var uploadRequest = new TransferUtilityUploadRequest
            {
                Key = key,
                BucketName = bucketName,
                InputStream = file.OpenReadStream(),
                PartSize = 20 * 1024 * 1024,
                StorageClass = S3StorageClass.Standard,
                ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256,
            };

            await transferUtility.UploadAsync(uploadRequest);
            return true;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error uploading file to S3");
            return false;
        }
    }

    public async Task<Stream> DownloadFile(string bucketName, string key, CancellationToken ct = default)
    {
        try
        {
            var response = await _s3Client.GetObjectAsync(bucketName, key, null, ct);
            return response.ResponseStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file from S3");
            throw;
        }
    }

    public async Task DeleteFile(string bucketName, string key, CancellationToken ct = default)
    {
        try
        {
            await _s3Client.DeleteObjectAsync(bucketName, key, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file from S3");
            throw;
        }
    }

    public async Task<bool> FileExists(string bucketName, string key, CancellationToken ct = default)
    {
        try
        {
            var response = await _s3Client.GetObjectMetadataAsync(bucketName, key, ct);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if file exists in S3");
            throw;
        }
    }

    public async Task<string> GetFileUrl(string bucketName, string key, CancellationToken ct = default)
    {
        try
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = key,
                Expires = DateTime.UtcNow.AddHours(1),
            };

            var url = await _s3Client.GetPreSignedURLAsync(request);

            return url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting file URL from S3");
            throw;
        }
    }
}
