using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDPSounds
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.strRDPSoundBringToThisComputer))]
        BringToThisComputer = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strRDPSoundLeaveAtRemoteComputer))]
        LeaveAtRemoteComputer = 1,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.strRDPSoundDoNotPlay))]
        DoNotPlay = 2
    }
}