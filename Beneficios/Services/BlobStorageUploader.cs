using Azure.Storage.Blobs;
using System;
using System.IO;
using Azure;

namespace Beneficios.Services;
public class BlobStorageUploader
{
  private const string connectionString = "";
  private const string containerName = "";

  public void UploadImageToBlob(string imagePath, string blobName)
  {
    try
    {
      BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
      BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
      BlobClient blobClient = containerClient.GetBlobClient(blobName);

      using (FileStream fs = File.OpenRead(imagePath))
      {
        blobClient.Upload(fs, true);
      }

      Console.WriteLine("Upload concluído com sucesso.");
    }
    catch (RequestFailedException ex)
    {
      Console.WriteLine($"Erro ao fazer upload do blob: {ex.Message}");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
    }
  }

}