﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LewdMaid.Models
{
    public static class ImageDownloader
    {
        public static string DownloadToAssets(string assetsUri, string imageUrl, string name, string format)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(assetsUri)
                .Append(@"/")
                .Append(name)
                .Append(".")
                .Append(format);

            string assetUri = stringBuilder.ToString();

            using (var client = new WebClient())
            {
                client.DownloadFile(new Uri(imageUrl), assetUri);
            }

            return assetUri;
        }
    }
}
