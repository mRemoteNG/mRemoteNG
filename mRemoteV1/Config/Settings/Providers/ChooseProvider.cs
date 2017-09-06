﻿using System.Configuration;

namespace mRemoteNG.Config.Settings.Providers
{
#if PORTABLE
    public class ChooseProvider : PortableSettingsProvider
#else
    public class ChooseProvider : LocalFileSettingsProvider
#endif
    { }
}