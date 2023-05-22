using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevanPangInterview.Events.App
{
    public struct StartSinglePlayer    { }
    public struct StartMultiPlayer     { }
    public struct ShowLeaderboards     { }
    public struct Escape               { }
                                      
    public struct GameplayFinished     { }
    
    public struct LeaderboardsFinished { }
}
