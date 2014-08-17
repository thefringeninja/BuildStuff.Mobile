using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuildStuff.Mobile.Model
{
    public delegate Task<IEnumerable<SpeakerListDto>> GetSpeakerList(int? page = null);

    public delegate Task<SpeakerDetailDto> GetSpeakerDetail(string id);
}
