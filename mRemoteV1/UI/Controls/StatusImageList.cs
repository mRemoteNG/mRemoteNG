using System;
using System.Drawing;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;


namespace mRemoteNG.UI.Controls
{
    public class StatusImageList
    {
        public ImageList GetImageList()
        {
            var imageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(16, 16),
                TransparentColor = Color.Transparent
            };
            FillImageList(imageList);
            return imageList;
        }

        public object ImageGetter(object rowObject)
        {
            if (rowObject is RootPuttySessionsNodeInfo) return "PuttySessions";
            if (rowObject is RootNodeInfo) return "Root";
            if (rowObject is ContainerInfo) return "Folder";
            var connection = rowObject as ConnectionInfo;
            if (connection == null) return "";
            return connection.OpenConnections.Count > 0 ? "Play" : "Pause";
        }

        private void FillImageList(ImageList imageList)
        {
            try
            {
                imageList.Images.Add(Resources.Root);
                imageList.Images.SetKeyName(0, "Root");
                imageList.Images.Add(Resources.Folder);
                imageList.Images.SetKeyName(1, "Folder");
                imageList.Images.Add(Resources.Play);
                imageList.Images.SetKeyName(2, "Play");
                imageList.Images.Add(Resources.Pause);
                imageList.Images.SetKeyName(3, "Pause");
                imageList.Images.Add(Resources.PuttySessions);
                imageList.Images.SetKeyName(4, "PuttySessions");
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace($"Unable to fill the image list of type {nameof(StatusImageList)}", ex);
            }
        }
    }
}