using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System;
using System.IO;
using Azure;

namespace Beneficios.Services;
public class BlobStorageUploader
{
  private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=beneficiosmourastorage;AccountKey=/cNM+V96Eiz00cDERmAziHNfn4U96kPAnvoEkJmmSAyNm7XfzOseCw/B3xNRc2cbwo66U5RVXv+j+AStSa/T5g==;EndpointSuffix=core.windows.net";
  private const string containerName = "content-beneficios-moura";
  public void UploadImageToBlob(string imagePath)
  {
    try
    {
      BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
      BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
      BlobClient blobClient = containerClient.GetBlobClient(imagePath.Substring(imagePath.IndexOf("\\") + 1));

      var options = new BlobUploadOptions
      {
        HttpHeaders = new BlobHttpHeaders { ContentType = "image/webp" }
      };

      using (FileStream fs = File.OpenRead(imagePath))
      {
        blobClient.Upload(fs, options);
      }

      Console.WriteLine("Upload concluído com sucesso.");
    }
    catch (RequestFailedException ex)
    {
      Console.WriteLine($"Erro ao fazer upload do blob: {ex.Message}");
    }

  }

}