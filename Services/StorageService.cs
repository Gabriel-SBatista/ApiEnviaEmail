using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace APIEnviaEmail.Services;

public class StorageService
{
    private readonly IAmazonS3 _client;
    private readonly IConfiguration _configuration;

    public StorageService(IConfiguration configuration)
    {
        _configuration = configuration;
        _client = new AmazonS3Client(_configuration.GetSection("AWSS3").GetRequiredSection("AccesKeyId").Value, _configuration.GetSection("AWSS3").GetRequiredSection("AccesKey").Value, 
            Amazon.RegionEndpoint.SAEast1);
    }

    public async Task UploadArquivoAsync(string nomeArquivo, Stream arquivo)
    {
        var uploadRequest = new TransferUtilityUploadRequest
        {
            BucketName = _configuration.GetSection("AWSS3").GetRequiredSection("Bucket").Value,
            Key = nomeArquivo,
            InputStream = arquivo
        };

        var fileTrasnferUtillity = new TransferUtility(_client);
        await fileTrasnferUtillity.UploadAsync(uploadRequest);
    }

    public async Task<Stream> DownloadArquivoAsync(string key)
    {
        var request = new GetObjectRequest
        {
            BucketName = _configuration.GetSection("AWSS3").GetRequiredSection("Bucket").Value,
            Key = key
        };

        var response = await _client.GetObjectAsync(request);

        using (var streamReader = new StreamReader(response.ResponseStream))
        {
            var stream = streamReader.BaseStream;
            return stream;
        }
    }
}
