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
    public class Rule34xxxProvider : IPictureProvider
    {
        private readonly string _url;
        private readonly string _countPlaceHolder;

        public Rule34xxxProvider(string url, string countPlaceHolder)
        {
            _url = url;
            _countPlaceHolder = countPlaceHolder;
        }

        public IEnumerable<Picture> Provide(int count)
        {
            IEnumerable<Post> picturesFromApi = GetPicturesFromApi(count);
            picturesFromApi = FiltrateByFormat(picturesFromApi, new string[] { "jpg", "jpeg", "png" });

            IEnumerable<Picture> pictures = picturesFromApi.Select(x => (Picture)x).ToList();

            /*
            foreach (var picture in pictures)
            {
                var assetUri = ImageDownloader.DownloadToAssets(@"C:/Users/ULTRO/Documents/Rule34xxxImages",
                    picture.PreviewUrl ?? picture.Url, picture.Hash, picture.Url.Split('.').Last());
            }
            */

            pictures = FixTags(pictures);
            return pictures;
        }

        private IEnumerable<Post> GetPicturesFromApi(int count)
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
            var pictures = JsonConvert.DeserializeObject<Rule34xxxResponse>(responceString).Posts;
            return pictures;
        }

        private IEnumerable<Post> FiltrateByFormat(IEnumerable<Post> pictures, IEnumerable<string> availableFormats)
        {
            var filtratedList = new List<Post>();
            foreach (var picture in pictures)
            {
                string pictureFormat = picture.FileUrl.Split('.').Last();
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
    }

    public class Rule34xxxResponse
    {
        [JsonProperty("count")]
        public string Count { get; set; }

        [JsonProperty("posts")]
        public List<Post> Posts { get; set; }     
    }

    public class Post
    {
        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("score")]
        public string Score { get; set; }

        [JsonProperty("file_url")]
        public string FileUrl { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("sample_url")]
        public string SampleUrl { get; set; }

        [JsonProperty("sample_width")]
        public string SampleWidth { get; set; }

        [JsonProperty("sample_height")]
        public string SampleHeight { get; set; }

        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; }

        [JsonProperty("rating")]
        public string Rating { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("width")]
        public string Width { get; set; }

        [JsonProperty("change")]
        public string Change { get; set; }

        [JsonProperty("md5")]
        public string Md5 { get; set; }

        [JsonProperty("creator_id")]
        public string CreatorId { get; set; }

        [JsonProperty("has_children")]
        public string HasChildren { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("has_notes")]
        public string HasNotes { get; set; }

        [JsonProperty("has_comments")]
        public string HasComments { get; set; }

        [JsonProperty("preview_width")]
        public string PreviewWidth { get; set; }

        [JsonProperty("preview_height")]
        public string PreviewHeight { get; set; }

        [JsonProperty("comments_url")]
        public string CommentsUrl { get; set; }

        [JsonProperty("creator_url")]
        public string CreatorUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }


        public static implicit operator Picture(Post response)
        {
            var picture = new Picture()
            {
                Hash = response.Md5,
                PostUrl = @"https://rule34.xxx/index.php?page=post&s=view&id="+response.Id,
                Url = response.FileUrl,
                PreviewUrl = response.SampleUrl,
                Rating = float.Parse(response.Score),
                Tags = response.Tags.Select(x => new Tag() { Name = x})
            };
            return picture;
        }
    }
}
