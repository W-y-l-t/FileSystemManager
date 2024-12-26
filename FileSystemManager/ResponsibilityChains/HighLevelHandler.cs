using FileSystemManager.ResponsibilityChains.HighLevelHandlers;

namespace FileSystemManager.ResponsibilityChains;

public class HighLevelHandler : AbstractHandler
{
    public HighLevelHandler()
    {
        this.SetNext(new ConnectionHandler())
            .SetNext(new FileHandler())
            .SetNext(new TreeHandler())
            .SetNext(new SystemHandler());
    }
}