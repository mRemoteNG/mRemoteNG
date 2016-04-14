using System;

namespace mRemoteNG.App.Update
{
    public class AnnouncementInfo
    {
        #region Public Properties
        public bool IsValid { get; set; }
        public string Name { get; set; }
        public Uri Address { get; set; }
        #endregion

        #region Public Methods
        public static AnnouncementInfo FromString(string input)
        {
            AnnouncementInfo newInfo = new AnnouncementInfo();
            if (string.IsNullOrEmpty(input))
            {
                newInfo.IsValid = false;
            }
            else
            {
                UpdateFile updateFile = new UpdateFile(input);
                newInfo.Name = updateFile.GetString("Name");
                newInfo.Address = updateFile.GetUri("URL");
                newInfo.IsValid = true;
            }
            return newInfo;
        }
        #endregion
    }
}