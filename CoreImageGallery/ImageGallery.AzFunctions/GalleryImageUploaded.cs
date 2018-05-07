using System;
using System.IO;
using ImageGallery.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Watermarker
{
    public static class GalleryImageUploaded
    {
        const string WatermarkMessage = "CoreImageGallery";

        [FunctionName("Watermarker")]
        public static void Run([BlobTrigger("images/{name}")]Stream inputBlob,
                               [Blob("images-watermarked/{name}", FileAccess.Write)] Stream outputBlob,
                               [DocumentDB("pyimagegallerydb", "", ConnectionStringSetting = "CosmosDBConnection"] Image image,
                               string name,
                               TraceWriter log)
        {

            try
            {
                ImageMarker.WriteWatermark(WatermarkMessage, inputBlob, outputBlob);
                log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {inputBlob.Length} Bytes");
            }
            catch (Exception e)
            {
                log.Info($"Watermarking failed: {e.Message}");
            }
        }
    }
}