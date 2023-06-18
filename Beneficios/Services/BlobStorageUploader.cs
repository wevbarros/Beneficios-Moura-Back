using Azure.Storage.Blobs;
using System;
using System.IO;
using Azure;

namespace Beneficios.Services;
public class BlobStorageUploader
{
  private const string connectionString = "STRING DE CONEXÃO COM O AZURE";
  private const string containerName = "CONTAINER";
  public void UploadImageToBlob(string imagePath)
  {
    try
    {
      BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
      BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
      BlobClient blobClient = containerClient.GetBlobClient(imagePath.Substring(imagePath.IndexOf("\\") + 1));
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