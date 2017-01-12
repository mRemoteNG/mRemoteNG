using System;
using System.Windows.Forms;
using mRemoteNG.App;


namespace mRemoteNG.UI.Controls
{
    public class StatusImageList
    {
        public static ImageList GetImageList()
        {
            var imageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new System.Drawing.Size(16, 16),
                TransparentColor = System.Drawing.Color.Transparent
            };
            FillImageList(imageList);
            return imageList;
        }

        private static void FillImageList(ImageList imageList)
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