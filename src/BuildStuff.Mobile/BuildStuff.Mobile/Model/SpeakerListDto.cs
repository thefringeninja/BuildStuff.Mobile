using System.Runtime.Serialization;

namespace BuildStuff.Mobile.Model
{
    [DataContract]
    public class SpeakerListDto
    {
        [DataMember] public readonly string Id;
        [DataMember] public readonly string Name;
        [DataMember] public readonly byte[] Image;

        private SpeakerListDto()
        {
            
        }
        public SpeakerListDto(string id, string name, byte[] image)
        {
            Id = id;
            Name = name;
            Image = image;
        }
    }
}