using System;
using System.Collections.Generic;
using mRemoteNG.Connection;


namespace mRemoteNG.Tree
{
    public class TreeNodeCompositeClickHandler : ITreeNodeClickHandler
    {
        public IEnumerable<ITreeNodeClickHandler> ClickHandlers { get; set; } = new ITreeNodeClickHandler[0];

        public void Execute(ConnectionInfo clickedNode)
        {
            if (clickedNode == null)
                throw new ArgumentNullException(nameof(clickedNode));
            foreach (var handler in ClickHandlers)
            {
                handler.Execute(clickedNode);
            }
        }
    }
}