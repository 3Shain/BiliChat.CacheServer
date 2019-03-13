using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SanShain.Bilichat.Extensions;
using SanShain.Bilichat.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SanShain.Bilichat.Services
{
    public class CacheService
    {
        public const string PREFIX_AVATAR = "avt_{0}";
        public const string PREFIX_USER = "user_{0}";
        public const string PREFIX_LIVE = "live_{0}";
        public static readonly TimeSpan AVATAR_EMPTY_ABS_EXPIRE_TIME = TimeSpan.FromDays(1);
        public static readonly TimeSpan AVATAR_ABS_EXPIRE_TIME = TimeSpan.FromDays(30);
        public static readonly TimeSpan USER_ABS_EXPIRE_TIME = TimeSpan.FromDays(30);
        public static readonly TimeSpan LIVE_ABS_EXPIRE_TIME = TimeSpan.FromDays(30);

        private IDistributedCache _Cache { get; set; }
        private IHttpClientFactory _Http { get; set; }

        public CacheService(IDistributedCache cache, IHttpClientFactory http)
        {
            _Cache = cache;
            _Http = http;
        }

        public async Task<byte[]> GetJpegAvatar(int memberId)
        {
            return await _Cache.GetAsync(string.Format(PREFIX_AVATAR,memberId)) ??await FetchAvatar(memberId);
        }
        
        public async Task<UserInfo> GetUserInfo(int memberId)
        {
            return await _Cache.GetJsonObjectAsync<UserInfo>(string.Format(PREFIX_USER, memberId)) ??await FetchUserInfo(memberId);
        }

        public async Task<LiveInfo> GetLiveInfo(int roomId)
        {
            return await _Cache.GetJsonObjectAsync<LiveInfo>(string.Format(PREFIX_LIVE, roomId)) ??await FetchLiveInfo(roomId);
        }
        
        private async Task<byte[]> FetchAvatar(int memberId)
        {
            var user = await GetUserInfo(memberId);
            if (user == null)
            {
                return null; //no such user
            }
            if (user.face.EndsWith("noface.gif"))
            {
                await _Cache.SetAsync(string.Format(PREFIX_AVATAR, memberId), new byte[0], new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = AVATAR_EMPTY_ABS_EXPIRE_TIME
                });
                return new byte[0];
            }
            byte[] rawResp = await _Http.CreateClient().GetByteArrayAsync(user.face);
            //process 
            Image<Rgba32> image = Image.Load(rawResp);
            image.Mutate(x => x.Resize(48, 48));
            using (MemoryStream ms = new MemoryStream())
            {
                image.SaveAsJpeg(ms);
                byte[] processed = ms.ToArray();

                //save
                await _Cache.SetAsync(string.Format(PREFIX_AVATAR, memberId), processed,new DistributedCacheEntryOptions() {
                    AbsoluteExpirationRelativeToNow=AVATAR_ABS_EXPIRE_TIME
                });

                return processed;
            }
        }

        private async Task<UserInfo>FetchUserInfo(int memberId)
        {
            ResponseUserInfo resp = JsonConvert.DeserializeObject<ResponseUserInfo>(await _Http.CreateClient().GetStringAsync($"https://api.bilibili.com/x/space/acc/info?mid={memberId}"));

            await _Cache.SetJsonObjectAsync(string.Format(PREFIX_USER, memberId), resp.data, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = USER_ABS_EXPIRE_TIME
            });
            return resp.data;
        }

        private async Task<LiveInfo> FetchLiveInfo(int roomId)
        {
            ResponseLiveInfo resp = JsonConvert.DeserializeObject<ResponseLiveInfo>(await _Http.CreateClient().GetStringAsync($"https://api.live.bilibili.com/room/v1/Room/room_init?id={roomId}"));

            await _Cache.SetJsonObjectAsync(string.Format(PREFIX_LIVE, roomId), resp.data, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = LIVE_ABS_EXPIRE_TIME
            });
            return resp.data;
        }
    }
}
