﻿using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDPSounds
    {
        [LocalizedAttributes.LocalizedDescription("strRDPSoundBringToThisComputer")]
        BringToThisComputer = 0,

        [LocalizedAttributes.LocalizedDescription("strRDPSoundLeaveAtRemoteComputer")]
        LeaveAtRemoteComputer = 1,

        [LocalizedAttributes.LocalizedDescription("strRDPSoundDoNotPlay")]
        DoNotPlay = 2
    }
}