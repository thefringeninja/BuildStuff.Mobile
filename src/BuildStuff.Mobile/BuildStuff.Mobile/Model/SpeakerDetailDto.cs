using System;
using System.Runtime.Serialization;

namespace BuildStuff.Mobile.Model
{
    [DataContract]
    public class SpeakerDetailDto
    {
        [DataMember] public readonly string Id;
        [DataMember] public readonly string Name;
        [DataMember] public readonly byte[] Image;
        [DataMember] public readonly string Description;
        [DataMember] public readonly string TwitterHandle;
        [DataMember] public readonly Uri Website;

        public SpeakerDetailDto(string id, string name, byte[] image, string description, string twitterHandle, Uri website)
        {
            Id = id;
            Name = name;
            Image = image;
            Description = description;
            TwitterHandle = twitterHandle;
            Website = website;
        }
    }
}