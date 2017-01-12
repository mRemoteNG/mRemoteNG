using System.Collections.Generic;
using mRemoteNG.Connection;


namespace mRemoteNG.Tree
{
    public class TreeNodeDoubleClickHandler : ITreeNodeClickHandler
    {
        public IEnumerable<ITreeNodeClickHandler> ClickHandlers { get; set; } = new ITreeNodeClickHandler[0];

        public void Execute(ConnectionInfo clickedNode)
        {
            if (clickedNode == null) return;
            foreach (var handler in ClickHandlers)
            {
                handler.Execute(clickedNode);
            }
        }
    }
}