using Avalonia.X11;

using LewdMaid.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LewdMaid.Models
{
    public class GelbooruProvider : IPictureProvider
    {
        private readonly string _url;
        private readonly string _countPlaceHolder;

        public GelbooruProvider(string url, string countPlaceHolder)
        {
            _url = url;
            _countPlaceHolder = countPlaceHolder;
        }

        public IEnumerable<Picture> Provide(int count)
        {
            IEnumerable<GelbooruResponse> picturesFromApi = GetPicturesFromApi(count);
            picturesFromApi = FiltrateByFormat(picturesFromApi, new string[] {"jpg", "jpeg", "png"});
            IEnumerable<Picture> pictures = picturesFromApi.Cast<Picture>();          
         
            pictures = FixTags(pictures);
            return pictures;
        }

        private IEnumerable<GelbooruResponse> GetPicturesFromApi(int count)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url.Replace(_countPlaceHolder, count.ToString()));
            request.Timeout = 25000;
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception) //На случай отсутствия подключения к интернету
            {            
                return null;
            }
            using var reader = new StreamReader(response.GetResponseStream());

            var responceString = reader.ReadToEnd();
            var pictures = JsonConvert.DeserializeObject<List<GelbooruResponse>>(responceString);
            return pictures;

        }

        private IEnumerable<GelbooruResponse> FiltrateByFormat(IEnumerable<GelbooruResponse> pictures, IEnumerable<string> availableFormats)
        {
            var filtratedList = new List<GelbooruResponse>();
            foreach (var picture in pictures)
            {
                string pictureFormat = picture.Image.Split('.').Last();
                if (availableFormats.Contains(pictureFormat))
                {
                    filtratedList.Add(picture);
                }
                
            }
            return filtratedList;
        }

        private IEnumerable<Picture> FixTags(IEnumerable<Picture> pictures)
        {
            foreach (var picture in pictures)
            {
                var passedTags = new List<Tag>();
                foreach (var tag in picture.Tags)
                {
                    if (IsUnavaibleTag(tag.Name))
                    {
                        continue;
                    }
                    string name = String.Concat(" #", tag.Name.Replace("-", "_")
                        .Replace("/", "_")
                        .Replace("`", "_")
                        .Replace("'", "_")
                        .Replace(".", "_")
                        .Replace("(", "")
                        .Replace(")", ""));
                    Tag fixedTag = new Tag()
                    {
                        Name = name
                    };
                    passedTags.Add(fixedTag);
                }
                picture.Tags = passedTags.ToArray();
            }
            return pictures;
        }

        private bool IsUnavaibleTag(string tag)
        {
            Regex reg = new Regex(@"[a-zA-Z]*[\d!+\/&?=:><;^@]+[a-zA-Z]*");
            return reg.IsMatch(tag) || tag.Equals("") || tag.Equals(" ");
        }


        private class GelbooruResponse
        {
            [JsonProperty("directory")]
            public string Directory { get; set; }
            [JsonProperty("hash")]
            public string Hash { get; set; }
            [JsonProperty("height")]
            public string Height { get; set; }
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("image")]
            public string Image { get; set; }
            [JsonProperty("change")]
            public string Change { get; set; }
            [JsonProperty("owner")]
            public string Owner { get; set; }
            [JsonProperty("parent_id")]
            public string ParentId { get; set; }
            [JsonProperty("rating")]
            public string Rating { get; set; }
            [JsonProperty("sample")]
            public bool Sample { get; set; }
            [JsonProperty("sample_height")]
            public string SampleHeight { get; set; }
            [JsonProperty("sample_width")]
            public string SampleWidth { get; set; }
            [JsonProperty("score")]
            public string Score { get; set; }
            [JsonProperty("tags")]
            public string Tags { get; set; }
            [JsonProperty("width")]
            public string Width { get; set; }
            [JsonProperty("file_url")]
            public string FileUrl { get; set; }

            public static implicit operator Picture(GelbooruResponse response)
            {
                var picture = new Picture()
                {
                    Hash = response.Hash,
                    Url = response.FileUrl,
                    PreviewUrl = response.GetSampleUrl(),
                    Rating = float.Parse(response.Score),
                    Tags = response.Tags.Split(' ').Select(x => new Tag() { Name = x }),
                    Size = 0
                };
                return picture;
            }

            public string GetSampleUrl()
            {
                if (!Sample)
                {
                    return "";
                }
                var tmp = FileUrl.Replace("images", "samples");
                tmp = tmp.Insert(tmp.LastIndexOf('/') + 1, "sample_");
                tmp = tmp.Remove(tmp.LastIndexOf('.')) + ".jpg";
                return tmp;
            }
        }
    }   
}
