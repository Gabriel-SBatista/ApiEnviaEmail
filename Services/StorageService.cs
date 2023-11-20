using Amazon.S3;
using Amazon.S3.Transfer;

namespace APIEnviaEmail.Services;

public class StorageService
{
    private readonly IAmazonS3 _client;


    public StorageService()
    {
        _client = new AmazonS3Client("AKIA3EPVUSDLTHBBWOA3", "LjVdNNoanvSGTKcjJ9gs2w6L708EjGHWoRwZpYGS", Amazon.RegionEndpoint.SAEast1);
    }

    public async Task UploadArquivoAsync(string nomeArquivo, Stream arquivo)
    {
        var uploadRequest = new TransferUtilityUploadRequest
        {
            BucketName = "gabrielsb-bucket",
            Key = nomeArquivo,
            InputStream = arquivo
        };

        var fileTrasnferUtillity = new TransferUtility(_client);
        await fileTrasnferUtillity.UploadAsync(uploadRequest);
    }
}
