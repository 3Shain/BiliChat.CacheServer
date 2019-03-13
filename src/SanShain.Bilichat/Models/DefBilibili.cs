using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SanShain.Bilichat.Models
{
    public class ResponseUserInfo
    {
        public int code { get; set; }
        public string message { get; set; }
        public UserInfo data { get; set; }
    }

    public class UserInfo
    {
        public long mid { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public string face { get; set; }
        public string sign { get; set; }
        public int rank { get; set; }
        public int level { get; set; }
        public long jointime { get; set; }
        public int moral { get; set; }
        public int silence { get; set; }
        public string birthday { get; set; }
        public string coins { get; set; }
        public bool fans_badge { get; set; }
        /*
        public object official { get; set; }
        public object vip { get; set; }*/
        /*
        public bool is_followed { get; set; }
        public object theme{get;set;}
        */
        public string top_photo { get; set; }
    }

    public class ResponseLiveInfo
    {
        public int code { get; set; }
        public string msg { get; set; }
        public LiveInfo data { get; set; }
    }

    public class LiveInfo
    {
        public int room_id { get; set; }
        public int short_id { get; set; }
        public int uid { get; set; }
        public int need_p2p { get; set; }
        public bool is_hidden { get; set; }
        public bool is_locked { get; set; }
        public bool is_portrait { get; set; }
        public int live_status { get; set; }
        public long hidden_till { get; set; }
        public long lock_till { get; set; }
        public bool encrypted { get; set; }
        public bool pwd_verified { get; set; }
        public long live_time { get; set; }
        public int room_shield { get; set; }
        public int is_sp { get; set; }
        public int special_type { get; set; }
    }

    public class MemoryAvatarData : IBinary
    {
        public byte[] Data { get; set; }
        public string ContentType { get; set; }

        public byte[] ToBinary()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(Data.Length);
                    bw.Write(Data);
                    bw.Write(ContentType);
                }
                return ms.ToArray();
            }
        }

        public void FromBinary(byte[] v)
        {
            using (MemoryStream ms = new MemoryStream(v))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Data = br.ReadBytes(br.ReadInt32());
                    ContentType = br.ReadString();
                }
            }
        }
    }

    public interface IBinary
    {
        byte[] ToBinary();
        void FromBinary(byte[] v);
    }
}
